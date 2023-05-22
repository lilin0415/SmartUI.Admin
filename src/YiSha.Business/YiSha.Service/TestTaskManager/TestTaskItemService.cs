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
using Koo.Utilities.Helpers;
using YiSha.Model.TestCaseManager;
using Koo.Utilities.Exceptions;

namespace YiSha.Service.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-02 16:21
    /// 描 述：服务类
    /// </summary>
    public class TestTaskItemService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<TestTaskItemEntity>> GetListByTaskId(long taskId)
        {
            var expression = CreateFilter<TestTaskItemEntity>();
            expression = expression.And(x => x.TaskId == taskId);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }
        
         

        public async Task<List<TestTaskItemEntity>> GetList(TestTaskItemListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<TestTaskItemModel>> GetPageList(TestTaskItemListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList().MapListTo<TestTaskItemModel>();
        }

        public async Task<TestTaskItemEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<TestTaskItemEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task<long> SaveForm(TestTaskItemEntity entity)
        {
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
        {this.VerifyIsMyDataOnDelete<TestTaskItemEntity>(ids);
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<TestTaskItemEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<TestTaskItemEntity, bool>> ListFilter(TestTaskItemListParam param)
        {
            var expression = CreateFilter<TestTaskItemEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion




        #region 添加用例列表
        public async Task AddCaseList(long taskId, TestCaseModel[] caseList)
        {
            var testTaskService = new TestTaskService();
            var testTask = await testTaskService.GetEntity(taskId, false);
            if (testTask.IsParallelMode == 1)
            {
                var allSupportParallel = await testTaskService.QueryTestCaseCallSupportParallel(caseList.Select(x => x.Id.GetValueOrDefault()).ToList());
                if (!allSupportParallel)
                {
                    throw new BizException($"此计划为[并行执行{GlobalContext.SystemConfig.CaseName}]模式，但选择数据中包含不支持并行的{GlobalContext.SystemConfig.CaseName}");
                }
            }

            var sql = $"select max(SortNum) from testtaskitem where TaskId ={taskId}";

            var maxSortNum = await base.BaseRepository().GetValue<int>(sql);
            var deployItems = new List<TestTaskItemEntity>();
            foreach (var x in caseList)
            {
                var deployItem = new TestTaskItemEntity();
            
                deployItem.TaskId = taskId;
                deployItem.GroupId = 0;
                deployItem.CaseId = x.Id;
                deployItem.ProjectGuid = x.ProjectGuid;
                deployItem.IsEnable = x.IsEnable;//这个IsEnable没意义，在生成作业的时候实时根据TestCase的IsEnable来生成是否可执行的作业
                deployItem.SortNum = ++maxSortNum;
                deployItems.Add(deployItem);
            }


            //await this.BaseRepository().Delete<TestTaskItemEntity>(x => x.TaskId == taskId);
            var trans = await this.BaseRepository().BeginTrans();
            try
            {
                foreach (var entity in deployItems)
                {
                    entity.Create();
                    this.VerifyIsMyDataOnCreate(entity);
                    await trans.Insert(entity);
                }

                await trans.CommitTrans();
            }
            catch
            {
                await trans.RollbackTrans();
                throw;
            }

            
            //await testTaskService.CheckAndUpdateParallelMode(taskId);
        }
        #endregion

        public async Task UpdateSortNums(List<SavingTestTaskItem> savingItems)
        {
            if (savingItems.Any())
            {
                var trans = await base.BaseRepository().BeginTrans();
                try
                {
                    foreach (var item in savingItems)
                    {
                        var entity = await trans.FindEntity<TestTaskItemEntity>(item.Id.Value);
                        entity.SortNum = item.SortNum;
                        entity.Modify();
                        await trans.Update(entity);
                    }
                    await trans.CommitTrans();
                }
                catch
                {
                 
                    await trans.RollbackTrans();
                    throw;
                }
            }
        }
    }
}
