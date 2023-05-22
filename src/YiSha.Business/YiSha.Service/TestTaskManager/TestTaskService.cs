using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.Param.TestTaskManager;
using YiSha.Model.TestTaskManager;
using YiSha.Model.TestCaseManager;
using YiSha.Service.TestCaseManager;
using Koo.Utilities.Helpers;
using YiSha.Enum;
using YiSha.Model.Param.TestCaseManager;
using System.Diagnostics;
using YiSha.Model.WebApis;
using Koo.Utilities.Exceptions;
using YiSha.Entity.ProjectManager;
using YiSha.Entity.TestCaseManager;
using Quartz;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using YiSha.Service.DeviceManager;
using NPOI.POIFS.FileSystem;

namespace YiSha.Service.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-02 16:20
    /// 描 述：服务类
    /// </summary>
    public class TestTaskService : BaseRepositoryService
    {
        #region 查询任务计划
        public async Task<List<TestTaskModel>> GetPageList(TestTaskListParam param, Pagination pagination)
        {

            var sql = $"select a.*, b.Name as EnvDisplayName from testtask a " +
                $"  left join execenvironment b" +
                $"      on a.EnvId = b.Id  " +
                $"  where 1=1 ";
          
            if (param != null)
            {
                if (param.Id.HasValue)
                {
                    sql += $" and a.Id ={param.Id.Value} ";
                }
                if (SecurityHelper.IsSafeSqlParam(param.Name))
                {
                    sql += $" and a.Name like '%{param.Name}%' ";
                }
                if (param.EnvId.HasValue)
                {
                    sql += $" and a.EnvId ={param.EnvId.Value} ";
                }
                if (SecurityHelper.IsSafeSqlParam(param.CaseCode) || SecurityHelper.IsSafeSqlParam(param.CaseName))
                {
                    var subQuery = $" select TaskId from {TestTaskItemEntity.TblName} a " +
                        $"  inner join testcase b  " +
                        $"      on  a.CaseId = b.Id " +
                        $"  where 1=1 " ;

                    if (SecurityHelper.IsSafeSqlParam(param.CaseCode))
                    {
                        subQuery+= $" and  b.Code like '%{param.CaseCode}%' ";
                    }
                    if (SecurityHelper.IsSafeSqlParam(param.CaseName))
                    {
                        subQuery += $" and  b.Name like '%{param.CaseName}%' ";
                    }
                    sql += $" and a.Id in( {subQuery} ) ";
                }
            }
            pagination.TotalCount = await this.BaseRepository().GetCount(sql);
            var list = await this.BaseRepository().FindList<TestTaskModel>(sql, pagination);
            return list.list.ToList();

            //var expression = ListFilter(param);
            //var list = await this.BaseRepository().FindList(expression, pagination);
            //var modelList = list.ToList().MapListTo<TestTaskModel>();

            //var envSerice = new ExecEnvironmentService();
            //var envList = await envSerice.GetList(new ExecEnvironmentListParam());
            //foreach (var model in modelList)
            //{
            //    model.EnvDisplayName = envList.FirstOrDefault(x => x.Id == model.EnvId)?.Name;
            //}

            //return modelList;
        }
        #endregion

        #region 获取数据
        public async Task<TData<List<TestTaskModel>>> GetListByClient(TestTaskListParam param)
        {
            var expression = CreateFilter<TestTaskEntity>();
            var list = await this.BaseRepository().FindList(expression);
            var ret = list.ToList().MapListTo<TestTaskModel>();

            return TData.CreateSuccessdValue(ret);
        }




        public async Task<List<TestTaskEntity>> GetList(TestTaskListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<TestTaskEntity>> GetListByIds(string ids)
        {
            ids = SecurityHelper.ToSafeSqlIds(ids);
            if (!string.IsNullOrEmpty(ids))
            {
                long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
                var items =await this.BaseRepository().FindList<TestTaskEntity>(x => idArr.Contains(x.Id.Value));
                return items.ToList();
            }
            return new List<TestTaskEntity> { };
        }


        public async Task<TestTaskEntity> GetEntity(long id,bool getConsumerDisplayName=false)
        {
            var entity = await this.BaseRepository().FindEntity<TestTaskEntity>(id);
            if (DateTimeHelper.IsEmpty(entity.FromTime))
            {
                entity.FromTime = null;
            }
            if (DateTimeHelper.IsEmpty(entity.ToTime))
            {
                entity.ToTime = null;
            }
            if (getConsumerDisplayName)
            {
                var mode = entity.ConsumeMode;
                if (mode == 1)
                {
                    var deviceService = new DeviceService();
                    var deviceEntity = await deviceService.GetEntity(entity.ConsumerId.GetValueOrDefault());
                    if (deviceEntity != null)
                    {
                        entity.ConsumerDisplayName = deviceEntity.UserName + $"({deviceEntity.Name},{deviceEntity.IP})";
                    }
                }
                else if (mode == 2)
                {
                    var deviceService = new DeviceGroupService();
                    var deviceEntity = await deviceService.GetEntity(entity.ConsumerId.GetValueOrDefault());
                    if (deviceEntity != null)
                    {
                        entity.ConsumerDisplayName = deviceEntity.Name;
                    }
                }
            }
            return entity;
        }
        #endregion

        #region 保存任务
        private bool ExistName(TestTaskEntity entity)
        {


            var expression = CreateFilter<TestTaskEntity>();
            //expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.Name == entity.Name);
            }
            else
            {
                expression = expression.And(t => t.Name == entity.Name && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }

        private void SetupCronValue(TestTaskEntity entity)
        {

            //起止时间，不自动设置起止时间，这样在运行的时候每次都是按照当前时间
            if (entity.FromTime == null)
            {
                //清空，如果直接为空值，是不会修改的
                entity.FromTime = DateTimeHelper.Empty1970;
            }

            if (entity.ToTime == null)
            {
                entity.ToTime = DateTimeHelper.Empty1970;
            }

            if (entity.IsEnable == 0)
            {
                entity.NextRunTime = DateTimeHelper.Empty1970;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(entity.CronExpression))
                {
                    entity.NextRunTime = DateTimeHelper.Empty1970;
                }
                else
                {
                    entity.CronExpression= entity.CronExpression.Trim();
                    var array = entity.CronExpression.Split(' ');
                    if (array.Length < 6)
                    {
                        throw new BizException("触发时间格式不正确");
                    }
                    if (array[0] == "*" || array[1] == "*" || array[2] == "*")
                    {
                        throw new BizException("请设置具体的秒、分、时");
                    }
                    CronExpression ce = null;
                    try
                    {
                        ce = new CronExpression(entity.CronExpression);
                    }
                    catch (Exception ex)
                    {
                        throw new BizException("触发时间格式不正确:" + ex.Message);
                    }

                    var fromTime = DateTimeHelper.IsEmpty(entity.FromTime) ? DateTimeHelper.Now : entity.FromTime.Value;
                    var next = ce.GetNextValidTimeAfter(fromTime);
                    entity.NextRunTime = next.Value.DateTime.AddHours(8);
                }
            }
        }
        public async Task<long> SaveForm(TestTaskEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                ExceptionFactory.ThrowCanNotBeEmptyException("名称不能为空");
            }
            entity.Name = entity.Name.Trim();

            if (this.ExistName(entity))
            {
                throw new DuplicationDataExection("名称已存在");
            }

            if (entity.IsParallelMode == 1)
            {
                //包含有不可并行的用例，整个任务都不可并行
                var canSupportParallel = await QueryTaskSupportParallel(entity.Id);
                if (!canSupportParallel)
                {
                    throw new BizException($"此计划为[并行执行{GlobalContext.SystemConfig.CaseName}]模式，但明细中包含不支持并行的{GlobalContext.SystemConfig.CaseName}");

                    entity.IsParallelMode = 0;
                }
            }
            this.SetupCronValue(entity);

            entity.IsTemp = 0;

            entity.DeviceDeployMode = (int)DeviceDeployModeEnumType.All;
            entity.MultipleInstances = (int)TaskInstancesPolicy.Queue;

            //entity.ConsumeMode=(int)TaskConsumeModeEnumType.All;
            //entity.ConsumerId = 0;

            //todo 禁用任务的时候，需要把未执行记录都禁用吗
            if (entity.Id.IsNullOrZero())
            {
                entity.Create();

                return await VerifyIsMyDataAndInsert(entity);
            }
            else
            {
                entity.Modify();

                return await VerifyIsMyDataAndUpdate(entity);
            }
        }

        /// <summary>
        /// 查询计划中的所有用例是否都支持并行运行，即当前计划是否支持并行
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<bool> QueryTaskSupportParallel(long? taskId)
        {
            if (taskId.GetValueOrDefault() == 0)
            {
                return true;
            }

            var sql = "select count(a.Id) from testtaskitem a ";
            sql += " inner join testcase b on a.CaseId = b.Id ";
            sql += " inner join publishedproject c on b.ProjectGuid=c.ProjectGuid and b.SpecialVersion=c.Version ";
            sql += $"  where a.TaskId = {taskId} ";
            sql += " and c.SupportParallel = 0 ";

            var notSupported = await this.BaseRepository().GetCountValue(sql);
            if (notSupported > 0)
            {
                return false;
                //有不支持并行的用例
                //修改当前任务不执行并行

            }
            return true;
        }

        /// <summary>
        /// 查询计划中的所有用例是否都支持并行运行，即当前计划是否支持并行
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<bool> QueryTestCaseCallSupportParallel(List<long> ids)
        {
            if (ids.Count==0)
            {
                return true;
            }

            var sql = "select count(b.Id) from testcase b ";
            sql += " inner join publishedproject c on b.ProjectGuid=c.ProjectGuid and b.SpecialVersion=c.Version ";
            sql += $"  where b.Id in ({string.Join(",",ids)}) ";
            sql += " and c.SupportParallel = 0 ";

            var notSupported = await this.BaseRepository().GetCountValue(sql);
            if (notSupported > 0)
            {
                return false;
                //有不支持并行的用例
                //修改当前任务不执行并行

            }
            return true;
        }
        public async Task<bool> CheckAndUpdateParallelMode(long taskId)
        {
            var canSupportParallel = await QueryTaskSupportParallel(taskId);
            if (!canSupportParallel)
            {
                //包含有不可并行的用例，整个任务都不可并行
                var sql = $"update testtask set IsParallelMode =0 where Id = {taskId}";
                await this.BaseRepository().ExecuteBySql(sql);
                return true;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 定时任务，修改任务的信息，执行次数等
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<long> UpdateFromScheduler(TestTaskEntity entity)
        {
            entity.Modify();
            entity.ExecedCount += 1;
            return await this.BaseRepository().Update(entity);

        }
        /// <summary>
        /// 禁用任务的状态，如最大执行次数超了
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<long> DisableFromScheduler(TestTaskEntity entity)
        {
            entity.IsEnable = 0;

            entity.Modify();
            return await this.BaseRepository().Update(entity);
        }

        /// <summary>
        /// 修改任务的状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<long> ChangeStatus(TestTaskEntity entity)
        {
            var dbEntity = await GetEntity(entity.Id.Value);
            dbEntity.IsEnable = entity.IsEnable;

            this.SetupCronValue(dbEntity);
            dbEntity.Modify();

            return await VerifyIsMyDataAndUpdate(dbEntity);
        }
     
        #region 删除
        public async Task DeleteForm(string ids)
        {
            ids = SecurityHelper.ToSafeSqlIds(ids);
            if (!string.IsNullOrEmpty(ids))
            {
                this.VerifyIsMyDataOnDelete<TestTaskEntity>(ids);

                var sql = $"select a.Name from testtask a " +
                    $"          inner join  taskexecrecord b " +
                    $"              on a.Id= b.TaskId  " +
                    $" where a.Id in({ids})";

                var names = await this.BaseRepository().GetList<string>(sql);
                names = names.Distinct().ToList();
                if (names.Any())
                {
                    throw new ForbidDeleteExection($"任务:{string.Join(",", names)} 已执行，不可删除");
                }

                long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
                await this.BaseRepository().Delete<TestTaskEntity>(idArr);
            }
        }
        #endregion

        #region 私有方法
        private Expression<Func<TestTaskEntity, bool>> ListFilter(TestTaskListParam param)
        {
            var expression = CreateFilter<TestTaskEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion



        #region 查询任务包含的用例
        public async Task<List<TestTaskCaseItemModel>> GetPageListByTaskId(long taskId)
        {
            //这个后面也需要加上组的功能

            var sql = $"select a.*," +
                $"      c.Name as ProjectName,c.SupportParallel, " +
                $"      b.id as TaskItemId, b.TaskId as TaskId, b.GroupId as GroupId, b.BaseCreateTime as TaskItemCreateTime,b.SortNum as SortNum " +
                $"  from testcase a " +
                $"      inner join testtaskitem b " +
                $"          on a.Id = b.CaseId  and  b.TaskId ={taskId}";
            sql += " inner join publishedproject c on a.ProjectGuid=c.ProjectGuid and a.SpecialVersion=c.Version ";
            sql += " order by b.SortNum asc ";

            var tuple = await this.BaseRepository().FindList<TestTaskCaseItemModel>(sql);

            var r = tuple.ToList().MapListTo<TestTaskCaseItemModel>();

            var envSerice = new ExecEnvironmentService();
            var envList = await envSerice.GetList(new ExecEnvironmentListParam());
            foreach (var model in r)
            {
                model.EnvDisplayName = envList.FirstOrDefault(x => x.Id == model.EnvId)?.Name;
                var usingVersionEnum = DataConverter.ToEnum<UsingVersionEnumType>(model.UsingVersion);
                model.UsingVersionDisplayName = DescriptionHelper.GetDescription(usingVersionEnum);
            }
            return r;
        }
        #endregion

        #region 查询任务下面的用例及用例组，用于生成任务执行记录
        /// <summary>
        /// 查询任务下面的用例及用例组
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<List<ITaskItemCaseBase>> GetTaskItemDetailByTaskId(long taskId)
        {
            var ret = new List<ITaskItemCaseBase>();

            //获取所有的用例，不包含组
            var sql = $"select a.*, b.Id as TaskItemId, b.TaskId as TaskId, b.GroupId as GroupId, b.SortNum as SortNum, b.BaseCreateTime as TaskItemCreateTime ";
            sql += ",c.AssertionCount AssertionCount, c.SupportParallel SupportParallel ";
            sql += $" from testcase a " +
                $"  inner join testtaskitem b on a.Id = b.CaseId and b.TaskId ={taskId} ";
            sql += " inner join publishedproject c on a.ProjectGuid=c.ProjectGuid and a.SpecialVersion=c.Version ";

            var tuple1 = await this.BaseRepository().FindList<TestTaskCaseItemModel>(sql);
            var itemList = tuple1.ToList();
            ret.AddRange(itemList);

            //获取所有的用例组
            sql = $"select a.*, b.Id as TaskItemId, b.TaskId as TaskId, b.GroupId as GroupId, b.SortNum as SortNum, b.BaseCreateTime as TaskItemCreateTime " +
                $" from testcasegroup a " +
                $"  inner join testtaskitem b on a.Id = b.GroupId and b.TaskId ={taskId}";
            var tuple2 = await this.BaseRepository().FindList<TestTaskCaseGroupItemModel>(sql);
            var groupItemList = tuple2.ToList();
            ret.AddRange(groupItemList);

            //获取组中所有的用例
            sql = $"select a.*, b.Id as TaskItemId, b.TaskId as TaskId, b.GroupId as GroupId, b.SortNum as SortNum ";
            sql += ",c.AssertionCount AssertionCount, c.SupportParallel SupportParallel ";
            sql += $" from testcase a " +
                $"  inner join testcasegroup d on a.Id in (d.CaseIds) " +
                $"  inner join testtaskitem b  on b.GroupId = d.Id and b.TaskId = {taskId} ";
            sql += " inner join publishedproject c on a.ProjectGuid=c.ProjectGuid and a.SpecialVersion=c.Version ";
            var tuple3 = await this.BaseRepository().FindList<TestTaskCaseItemModel>(sql);
            var itemsInGroupItemList = tuple3.ToList();


            var envSerice = new ExecEnvironmentService();
            var envList = await envSerice.GetList(new ExecEnvironmentListParam());

            foreach (var model in itemList)
            {
                model.EnvDisplayName = envList.FirstOrDefault(x => x.Id == model.EnvId)?.Name;
                var usingVersionEnum = DataConverter.ToEnum<UsingVersionEnumType>(model.UsingVersion);
                model.UsingVersionDisplayName = DescriptionHelper.GetDescription(usingVersionEnum);
            }

            foreach (var model in itemsInGroupItemList)
            {
                model.EnvDisplayName = envList.FirstOrDefault(x => x.Id == model.EnvId)?.Name;
                var usingVersionEnum = DataConverter.ToEnum<UsingVersionEnumType>(model.UsingVersion);
                model.UsingVersionDisplayName = DescriptionHelper.GetDescription(usingVersionEnum);
            }

            foreach (var groupItem in groupItemList)
            {
                if (string.IsNullOrWhiteSpace(groupItem.CaseIds))
                {
                    continue;
                }

                groupItem.Items = new List<TestTaskCaseItemModel>();
                var caseIds = groupItem.CaseIds.Split(',').ConvertAll<string, long>(x => long.Parse(x));
                int index = 0;
                foreach (var tempId in caseIds)
                {
                    var itemInGroup = itemsInGroupItemList.FirstOrDefault(x => x.Id == tempId);
                    if (itemInGroup != null)
                    {
                        //组内排序
                        itemInGroup.SortNum = ++index;
                        groupItem.Items.Add(itemInGroup);
                    }
                }
            }

            //sortnum目前没有值，先按照添加时间的顺序排序
            return ret.OrderBy(x => x.SortNum).ToList();

        }
        #endregion

        #region 作业结果后 更新任务的上次执行状态
        public async Task<bool> UpdatePrevStatus(UpdateTaskExecStatusBody body)
        {
            if (body.ExecStatus == ExecStatusEnumType.Finished)
            {
                var sql = $"update  testtask ";
                sql += $" set {nameof(TestTaskEntity.PrevStartTime)}='{body.StartTime}' ";
                sql += $", {nameof(TestTaskEntity.PrevEndTime)}='{body.EndTime}' ";
                sql += $", {nameof(TestTaskEntity.PrevFinishStatus)}={(int)body.FinishStatus}";
                sql += $", {nameof(TestTaskEntity.PrevReason)}='{body.Reason}'";
                sql += $" where {nameof(TestTaskEntity.Id)}={body.TaskId} ";

                await this.BaseRepository().ExecuteBySql(sql);
            }
         
            return true;
        }
        public async Task<bool> UpdatePrevStatus(long taskExecId)
        {
            var taskExecService = new TaskExecRecordService();
            var taskExec = await taskExecService.GetEntity(taskExecId);

            var sql = $"update  testtask ";
            sql += $", {nameof(TestTaskEntity.PrevStartTime)}='{taskExec.StartTime}' ";
            sql += $", {nameof(TestTaskEntity.PrevEndTime)}='{taskExec.EndTime}' ";
            sql += $", {nameof(TestTaskEntity.PrevFinishStatus)}={(int)taskExec.FinishStatus}";
            sql += $", {nameof(TestTaskEntity.PrevReason)}='{taskExec.Reason}'";
            sql += $" where {nameof(TestTaskEntity.Id)}={taskExec.TaskId} ";

            await this.BaseRepository().ExecuteBySql(sql);


            return true;
        }
        #endregion

    }
}
