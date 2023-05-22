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
using Koo.Utilities.Helpers;
using YiSha.Enum;
using YiSha.Model.TestTaskManager;
using YiSha.Service.TestCaseManager;
using YiSha.Web.Code;
using YiSha.Model.Param.TestCaseManager;
using System.Diagnostics;
using YiSha.Model.WebApis;
using Koo.Utilities.Exceptions;
using YiSha.Data.EF;
using System.Threading;
using YiSha.Service.SignalRManager;
using YiSha.Entity.Exceptions;

namespace YiSha.Service.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-16 18:02
    /// 描 述：服务类
    /// </summary>
    public class TaskExecRecordService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<TaskExecRecordEntity>> GetList(TaskExecRecordListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<TaskExecRecordModel>> GetPageList(TaskExecRecordListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            var items = list.ToList();
            return items.MapListTo<TaskExecRecordModel>();
        }

        public async Task<TaskExecRecordEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<TaskExecRecordEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task<long> SaveForm(TaskExecRecordEntity entity)
        {
            throw new ForbidUpdateExection();
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

        public async Task DeleteForm(string ids)
        {
            throw new ForbidDeleteExection();
            this.VerifyIsMyDataOnDelete<TaskExecRecordEntity>(ids);
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<TaskExecRecordEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<TaskExecRecordEntity, bool>> ListFilter(TaskExecRecordListParam param)
        {
            var expression = CreateFilter<TaskExecRecordEntity>();
            if (param == null)
            {
                return expression;
            }

            if (SqlStringHelper.IsSafeSqWhere(param.Guid))
            {
                expression = expression.And(x => x.Guid == param.Guid);
            }
            if (SecurityHelper.IsSafeSqlParam(param.Name))
            {
                expression = expression.And(x => x.Name.Contains(param.Name));
            }

            if (param.TaskId.HasValue)
            {
                expression = expression.And(x => x.TaskId == param.TaskId);
            }

            //if (SecurityHelper.IsSafeSqlParam(param.UserName))
            //{
            //    expression = expression.And(x => x.UserName.Contains(param.UserName));
            //}
            return expression;
        }
        private Expression<Func<TaskExecRecordEntity, bool>> ListFilterSql(TaskExecRecordListParam param)
        {
            
            var expression = CreateFilter<TaskExecRecordEntity>();
            if (param == null)
            {
                return expression;
            }

            if (SqlStringHelper.IsSafeSqWhere(param.Guid))
            {
                expression = expression.And(x => x.Guid == param.Guid);
            }
            if (SecurityHelper.IsSafeSqlParam(param.Name))
            {
                expression = expression.And(x => x.Name.Contains(param.Name));
            }

            if (param.TaskId.HasValue)
            {
                expression = expression.And(x => x.TaskId == param.TaskId);
            }

            //if (SecurityHelper.IsSafeSqlParam(param.UserName))
            //{
            //    expression = expression.And(x => x.UserName.Contains(param.UserName));
            //}
            return expression;
        }
        #endregion

        #region 添加作业
        //TaskSourceType
        public async Task AddAutoRecord(long taskId)
        {
            await AddRecordInternal(taskId, TaskSourceType.AutoTask);
        }
        public async Task AddManuallyRecord(long taskId)
        {
            await AddRecordInternal(taskId, TaskSourceType.ManuallyTask);
        }
        private async Task AddRecordInternal(long taskId, TaskSourceType sourceType)
        {
            var testTaskService = new TestTaskService();
            var detail = await testTaskService.GetTaskItemDetailByTaskId(taskId);
            if (!detail.Any())
            {
                throw new TaskItemIsEmptyException($"此任务中未包含任何{GlobalContext.SystemConfig.CaseName}，不能执行");
            }

            long taskExecId = 0;
            var taskInDb = await testTaskService.GetEntity(taskId);

            var trans = await this.BaseRepository().BeginTrans();
            try
            {
                #region 1、生成任务 TaskExecRecord

                #region 计算任务中包含的用例中
                var totalCount = 0;
                foreach (var item in detail)
                {
                    if (item is TestTaskCaseItemModel)
                    {
                        totalCount += 1;
                    }
                    else if (item is TestTaskCaseGroupItemModel caseGroup)
                    {
                        totalCount += caseGroup.Items.Count;
                    }
                }
                #endregion

                var taskRecord = taskInDb.MapTo<TaskExecRecordEntity>();
                taskRecord.Id = null;
                taskRecord.TaskId = taskId;
                taskRecord.Name = taskInDb.Name + "-" + RandomHelper.NextStringAlphaNumeric(4);
                taskRecord.SourceType = (byte)sourceType;
                taskRecord.NextStartTime = DateTimeHelper.Empty;
                taskRecord.StartTime = DateTimeHelper.Empty;
                taskRecord.EndTime = DateTimeHelper.Empty;
                taskRecord.TotalCaseCount= totalCount;
                taskRecord.Guid = GuidHelper.NewId();
                taskRecord.ResetBaseCreatorAndModifier();
                taskRecord.Create();

                //未包含任务用例
                if (!detail.Any())
                {
                    taskRecord.StartTime = DateTimeHelper.Now;
                    taskRecord.EndTime = DateTimeHelper.Empty;
                    taskRecord.ExecStatus = (byte)ExecStatusEnumType.Finished;
                    taskRecord.FinishStatus = (byte)FinishStatusEnumType.Succeeded;
                    taskRecord.Reason = $"任务未包含任何{GlobalContext.SystemConfig.CaseName}";
                }
                await trans.Insert(taskRecord);
                #endregion

                taskExecId = taskRecord.Id.Value;

                int sortNum = 0;

                //如果是第一个有效的作业，因为有的用例为禁用状态，
                bool isFirstValidItem = true;

                foreach (var item in detail)
                {
                    //单个的用例
                    if (item is TestTaskCaseItemModel taskItem)
                    {
                        #region 单个的用例
                        //2、添加测试用例
                        var caseRecord = await CreateCase(taskExecId, taskItem);

                        if (caseRecord.IsEnable == 1)
                        {
                            //如果为第一个，则可以消费
                            if (isFirstValidItem)
                            {
                                caseRecord.ConsumeStatus = (byte)ConsumeStatusEnumType.Ready;
                            }
                            else if (taskRecord.IsParallelMode == 1)
                            {
                                //并行并且当前支持并行
                                //一般情况下，如果一个任务里面只要有一个不支持并行，则整个任务都不可并行
                                Debug.Assert(caseRecord.SupportParallel == 1);
                                caseRecord.ConsumeStatus = (byte)ConsumeStatusEnumType.Ready;
                            }
                            else
                            {
                                caseRecord.ConsumeStatus = (byte)ConsumeStatusEnumType.Pending;
                            }

                            isFirstValidItem = false;
                        }
                        else
                        {
                            //禁用的直接跳过
                            caseRecord.ConsumeStatus = (byte)ConsumeStatusEnumType.Invalid;
                            caseRecord.ConsumedTime = DateTimeHelper.Empty;

                            caseRecord.ExecStatus= (byte)ExecStatusEnumType.Finished;
                            caseRecord.EndTime = DateTimeHelper.Now;
                            caseRecord.FinishStatus = (byte)FinishStatusEnumType.Skipped;
                            caseRecord.Reason = $"当前{GlobalContext.SystemConfig.CaseName}已被禁用，跳过执行";
                        }

                        caseRecord.SortNum = ++sortNum;
                        caseRecord.ResetBaseCreatorAndModifier();
                        caseRecord.Create();
                        await trans.Insert(caseRecord);

                        #endregion
                    }
                    else
                    {
                        //3、添加用例组
                        #region 添加用例组
                        var caseGroup = item as TestTaskCaseGroupItemModel;

                        var caseGroupRecord = CreateCaseGroup(taskExecId, caseGroup);

                        caseGroupRecord.ConsumeStatus = (byte)ConsumeStatusEnumType.Invalid;
                        caseGroupRecord.SortNum = ++sortNum;
                        caseGroupRecord.ResetBaseCreatorAndModifier();
                        caseGroupRecord.Create();
                        await trans.Insert(caseGroupRecord);

                        foreach (var subTaskItem in caseGroup.Items)
                        {
                            var caseRecord = await CreateCase(taskExecId, subTaskItem);
                            caseRecord.GroupId = caseGroup.GroupId;

                            if (caseRecord.IsEnable == 1 /*&& caseGroup.IsEnable==1 */)//并且组启用
                            {
                                //如果为第一个，不管任务是并行还是串行，不管是组里面的第一个，还是非组里面的，只要是第一个，都可以消费
                                if (isFirstValidItem)
                                {
                                    caseRecord.ConsumeStatus = (byte)ConsumeStatusEnumType.Ready;
                                }
                                else if (taskRecord.IsParallelMode == 1)//如果当前任务为并行的
                                {  
                                    //并行并且当前支持并行
                                   //一般情况下，如果一个任务里面只要有一个不支持并行，则整个任务都不可并行
                                    Debug.Assert(caseRecord.SupportParallel == 1);

                                    //如果组为并行的，则组里面所有明细都并行
                                    if (caseGroup.IsParallelMode == 1)
                                    {
                                        caseRecord.ConsumeStatus = (byte)ConsumeStatusEnumType.Ready;
                                    }
                                    else
                                    {
                                        //如果组为串行，则组中第一个可运行，后面的排队
                                        if (subTaskItem == caseGroup.Items.First())
                                        {
                                            caseRecord.ConsumeStatus = (byte)ConsumeStatusEnumType.Ready;
                                        }
                                        else
                                        {
                                            caseRecord.ConsumeStatus = (byte)ConsumeStatusEnumType.Pending;
                                        }
                                    }
                                }
                                else
                                {
                                    //串行任务且不是第一个，排队
                                    caseRecord.ConsumeStatus = (byte)ConsumeStatusEnumType.Pending;
                                }

                                isFirstValidItem = false;
                            }
                            else
                            {
                                //禁用的直接跳过
                                caseRecord.ConsumeStatus = (byte)ConsumeStatusEnumType.Invalid;
                                caseRecord.ConsumedTime = DateTimeHelper.Empty;

                                caseRecord.ExecStatus = (byte)ExecStatusEnumType.Finished;
                                caseRecord.EndTime = DateTimeHelper.Now;
                                caseRecord.FinishStatus = (byte)FinishStatusEnumType.Skipped;
                                caseRecord.Reason = $"当前{GlobalContext.SystemConfig.CaseName}已被禁用，跳过执行";
                            }
                            
                            caseRecord.SortNum = ++sortNum;
                            caseRecord.ResetBaseCreatorAndModifier();
                            caseRecord.Create();
                            await trans.Insert(caseRecord);
                        }

                        #endregion
                    }
                }
                await trans.CommitTrans();
            }
            catch (Exception ex)
            {
                await trans.RollbackTrans();
                throw;
            }

            try
            {
                var body = new OnNewJobCreatedBody()
                {
                    JobId = taskExecId
                };
                await GlobalContext.GetService<ClientHub>()?.OnNewJobCreated(body);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private async Task<CaseExecRecordEntity> CreateCase(long taskExecId, TestTaskCaseItemModel taskItem)
        {
            var caseRecord = new CaseExecRecordEntity();
            caseRecord.Id = null;
            caseRecord.TaskId = taskItem.TaskId;
            caseRecord.TaskItemId = taskItem.TaskItemId;
            caseRecord.TaskExecId = taskExecId;
          
            caseRecord.GroupId = 0;
            caseRecord.CaseId = taskItem.CaseId;
            caseRecord.EnvId = taskItem.EnvId;
            caseRecord.ProjectGuid = taskItem.ProjectGuid;
            caseRecord.ProjectVersion = taskItem.SpecialVersion;

            caseRecord.Code = taskItem.Code;
            caseRecord.Name = taskItem.Name;

            caseRecord.StartTime = DateTimeHelper.Empty;
            caseRecord.EndTime = DateTimeHelper.Empty;

            caseRecord.ExecStatus = 0;
            caseRecord.FinishStatus = 0;
            caseRecord.Reason = String.Empty;
            caseRecord.VarJson = taskItem.VarJson;
            //如果没有设置变量，把则默认的变量配置保存下来
            if (string.IsNullOrEmpty(caseRecord.VarJson))
            {
                var varJsonService = new VarJsonService();
                var varJson = await varJsonService.GetTestCaseVarJsonByTestCaseId(taskItem.CaseId.Value, true);
                if (varJson.Status)
                {
                    caseRecord.VarJson = varJson.Result.ToVarJson();
                }
            }

            //caseRecord.IsConsumed = 0;
            caseRecord.ConsumedTime = DateTimeHelper.Empty;
            caseRecord.UserId= 0;
            caseRecord.UserName = string.Empty;

            //在添加执行明细的时候，如果用例已禁用，则添加的明细也是禁用的，不会执行
            caseRecord.IsEnable = (byte)taskItem.IsEnable;
            caseRecord.SortNum = taskItem.SortNum;

            caseRecord.Priority=taskItem.Priority;
            caseRecord.TypeId = taskItem.TypeId;

            caseRecord.TotalAssertionCount = taskItem.AssertionCount;
            caseRecord.SucceedAssertionCount = 0;
            caseRecord.FailedAssertionCount = 0;

            caseRecord.SupportParallel = taskItem.SupportParallel;
            caseRecord.LogFilePath = string.Empty;
            caseRecord.Guid = GuidHelper.NewId();

            return caseRecord;
        }
        private CaseExecRecordEntity CreateCaseGroup(long taskExecId,TestTaskCaseGroupItemModel taskItem)
        {
            var caseRecord = new CaseExecRecordEntity();
            caseRecord.Id = null;
            caseRecord.TaskId = taskItem.TaskId;
            caseRecord.TaskItemId = taskItem.TaskItemId;
            caseRecord.TaskExecId = taskExecId;
          
            caseRecord.GroupId = taskItem.GroupId;
            caseRecord.CaseId = 0;
            caseRecord.EnvId = taskItem.EnvId;
            caseRecord.ProjectGuid = String.Empty;
            caseRecord.ProjectVersion = String.Empty;

            caseRecord.Code = taskItem.Code;
            caseRecord.Name = taskItem.Name;

            caseRecord.StartTime = DateTimeHelper.Empty;
            caseRecord.EndTime = DateTimeHelper.Empty;

            caseRecord.ExecStatus = 0;
            caseRecord.FinishStatus = 0;
            caseRecord.Reason = String.Empty;
            caseRecord.VarJson = String.Empty;

            //caseRecord.IsConsumed = 0;
            caseRecord.ConsumedTime = DateTimeHelper.Empty;
            caseRecord.UserId = 0;
            caseRecord.UserName = string.Empty;
            caseRecord.SortNum = taskItem.SortNum;

            //在添加执行明细的时候，如果用例已禁用，则添加的明细也是禁用的，不会执行
            caseRecord.IsEnable = (byte)taskItem.IsEnable;
            caseRecord.SortNum = taskItem.SortNum;

            caseRecord.Priority = 0;
            caseRecord.TypeId = 0;

            caseRecord.TotalAssertionCount = 0;
            caseRecord.SucceedAssertionCount = 0;
            caseRecord.FailedAssertionCount = 0;

            caseRecord.SupportParallel = 0;
            caseRecord.LogFilePath = string.Empty;
            caseRecord.Guid = GuidHelper.NewId();
            return caseRecord;
        }

        #endregion


        #region 取消任务
        public async Task CancelTaskExec(long taskExecId)
        {
            var entity = await GetEntity(taskExecId);

            var db = this.BaseRepository().db as MySqlDatabase;
            await db.BeginTrans(async trans => {

                var startTime = DateTimeHelper.Now.ToLongString();
                var endTime = DateTimeHelper.Now.ToLongString();
                var execStatus = ExecStatusEnumType.Finished;
                var finishStatus = FinishStatusEnumType.Cancelled;
                var reason = "从控制台取消";

                #region 取消作业
                //更新未完成的作业，为取消状态（结束时间、结束状态）
                var sql = $"update  caseexecrecord " +
                $"  set {nameof(CaseExecRecordEntity.ExecStatus)}={(int)execStatus} ";
                sql += $", ConsumeStatus= (case ConsumeStatus ";
                sql += $"                   when {(int)ConsumeStatusEnumType.Pending} then {(int)ConsumeStatusEnumType.Cancelled} ";
                sql += $"                   when {(int)ConsumeStatusEnumType.Ready} then {(int)ConsumeStatusEnumType.Cancelled} ";
                sql += $"                   else ConsumeStatus  end ) ";
                sql += $", StartTime= (case StartTime when '1970-01-01 00:00:00' then '{startTime}' else StartTime  end ) ";
                sql += $", {nameof(CaseExecRecordEntity.EndTime)}='{endTime}' ";
                sql += $", {nameof(CaseExecRecordEntity.FinishStatus)}={(int)finishStatus}";
                sql += $", {nameof(CaseExecRecordEntity.Reason)}='{reason}'";
                sql += $" where {nameof(CaseExecRecordEntity.TaskExecId)}={taskExecId} ";
                sql += $"   and {nameof(CaseExecRecordEntity.FinishStatus)}={(int)FinishStatusEnumType.None} ";
                var updateFinished = await trans.ExecuteNonQueryAsync(System.Data.CommandType.Text, sql);
                #endregion

                #region 取消任务 更新任务中用例的进度，开始时间、结束时间、结束状态
                sql = $"update  {TaskExecRecordEntity.TblName} set ";
                sql += $" CancelledCaseCount=CancelledCaseCount+{updateFinished} ";

                sql += $", StartTime= (case StartTime when '1970-01-01 00:00:00' then '{startTime}' else StartTime  end ) ";

                sql += $", {nameof(TaskExecRecordEntity.ExecStatus)}={(int)execStatus} ";
                sql += $", {nameof(TaskExecRecordEntity.EndTime)}='{endTime}' ";
                sql += $", {nameof(TaskExecRecordEntity.FinishStatus)}={(int)finishStatus}";
                sql += $", {nameof(TaskExecRecordEntity.Reason)}='{reason}'";

                sql += $" where {nameof(TaskExecRecordEntity.Id)}={taskExecId} ";
                await trans.ExecuteNonQueryAsync(System.Data.CommandType.Text, sql);

                #endregion
            });


        }
        #endregion

    }
}
