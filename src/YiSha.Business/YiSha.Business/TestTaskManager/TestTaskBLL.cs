using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.Param.TestTaskManager;
using YiSha.Service.TestTaskManager;

namespace YiSha.Business.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-02 16:20
    /// 描 述：业务类
    /// </summary>
    public class TestTaskBLL
    {
        private TestTaskService testTaskService = new TestTaskService();

        #region 获取数据
        public async Task<TData<List<TestTaskEntity>>> GetList(TestTaskListParam param)
        {
            TData<List<TestTaskEntity>> obj = new TData<List<TestTaskEntity>>();
            obj.Result = await testTaskService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }
        public async Task<TData<List<TestTaskEntity>>> GetListByIds(string taskIds)
        {
            TData<List<TestTaskEntity>> obj = new TData<List<TestTaskEntity>>();
            obj.Result = await testTaskService.GetListByIds(taskIds);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }
        //public async Task<TData<List<TestTaskEntity>>> GetPageList(TestTaskListParam param, Pagination pagination)
        //{
        //    TData<List<TestTaskEntity>> obj = new TData<List<TestTaskEntity>>();
        //    obj.Result = await testTaskService.GetPageList(param, pagination);
        //    obj.Total = pagination.TotalCount;
        //    obj.Status = true;
        //    return obj;
        //}

        public async Task<TData<TestTaskEntity>> GetEntity(long id, bool getConsumerDisplayName = false)
        {
            TData<TestTaskEntity> obj = new TData<TestTaskEntity>();
            obj.Result = await testTaskService.GetEntity(id, getConsumerDisplayName);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 保存、修改状态、删除

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TData<string>> SaveForm(TestTaskEntity entity)
        {
            TData<string> obj = new TData<string>();
            await testTaskService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TData<string>> ChangeStatusJson(TestTaskEntity entity)
        {
            TData<string> obj = new TData<string>();
            await testTaskService.ChangeStatus(entity);
            obj.Status = true;
            return obj;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await testTaskService.DeleteForm(ids);
        
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
