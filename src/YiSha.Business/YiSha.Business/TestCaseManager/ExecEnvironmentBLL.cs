using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.TestCaseManager;
using YiSha.Model.Param.TestCaseManager;
using YiSha.Service.TestCaseManager;

namespace YiSha.Business.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:55
    /// 描 述：业务类
    /// </summary>
    public class ExecEnvironmentBLL
    {
        private ExecEnvironmentService execEnvironmentService = new ExecEnvironmentService();

        #region 获取数据
        public async Task<TData<List<ExecEnvironmentEntity>>> GetList(ExecEnvironmentListParam param)
        {
            TData<List<ExecEnvironmentEntity>> obj = new TData<List<ExecEnvironmentEntity>>();
            obj.Result = await execEnvironmentService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<ExecEnvironmentEntity>>> GetPageList(ExecEnvironmentListParam param, Pagination pagination)
        {
            TData<List<ExecEnvironmentEntity>> obj = new TData<List<ExecEnvironmentEntity>>();
            obj.Result = await execEnvironmentService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<ExecEnvironmentEntity>> GetEntity(long id)
        {
            TData<ExecEnvironmentEntity> obj = new TData<ExecEnvironmentEntity>();
            obj.Result = await execEnvironmentService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(ExecEnvironmentEntity entity)
        {
            TData<string> obj = new TData<string>();
            await execEnvironmentService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await execEnvironmentService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
