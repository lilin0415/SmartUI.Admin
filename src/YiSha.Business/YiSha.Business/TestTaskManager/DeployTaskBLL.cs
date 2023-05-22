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
    /// 日 期：2022-10-12 21:21
    /// 描 述：业务类
    /// </summary>
    public class DeployTaskBLL
    {
        private DeployTaskService deployTaskService = new DeployTaskService();

        #region 获取数据
        public async Task<TData<List<DeployTaskEntity>>> GetList(DeployTaskListParam param)
        {
            TData<List<DeployTaskEntity>> obj = new TData<List<DeployTaskEntity>>();
            obj.Result = await deployTaskService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<DeployTaskEntity>>> GetPageList(DeployTaskListParam param, Pagination pagination)
        {
            TData<List<DeployTaskEntity>> obj = new TData<List<DeployTaskEntity>>();
            obj.Result = await deployTaskService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<DeployTaskEntity>> GetEntity(long id)
        {
            TData<DeployTaskEntity> obj = new TData<DeployTaskEntity>();
            obj.Result = await deployTaskService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(DeployTaskEntity entity)
        {
            TData<string> obj = new TData<string>();
            await deployTaskService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await deployTaskService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
