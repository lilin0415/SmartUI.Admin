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
using YiSha.Model.TestTaskManager;

namespace YiSha.Business.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-16 18:02
    /// 描 述：业务类
    /// </summary>
    public class TaskExecRecordBLL
    {
        private TaskExecRecordService taskExecRecordService = new TaskExecRecordService();

        #region 获取数据
        public async Task<TData<List<TaskExecRecordEntity>>> GetList(TaskExecRecordListParam param)
        {
            TData<List<TaskExecRecordEntity>> obj = new TData<List<TaskExecRecordEntity>>();
            obj.Result = await taskExecRecordService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<TaskExecRecordModel>>> GetPageList(TaskExecRecordListParam param, Pagination pagination)
        {
            TData<List<TaskExecRecordModel>> obj = new TData<List<TaskExecRecordModel>>();
            obj.Result = await taskExecRecordService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<TaskExecRecordEntity>> GetEntity(long id)
        {
            TData<TaskExecRecordEntity> obj = new TData<TaskExecRecordEntity>();
            obj.Result = await taskExecRecordService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(TaskExecRecordEntity entity)
        {
            TData<string> obj = new TData<string>();
            await taskExecRecordService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await taskExecRecordService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
