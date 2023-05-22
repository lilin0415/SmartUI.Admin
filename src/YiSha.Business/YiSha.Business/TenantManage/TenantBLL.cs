using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.TenantManage;
using YiSha.Model.Param.TenantManage;
using YiSha.Service.TenantManage;
using YiSha.Model.TenantManage;

namespace YiSha.Business.TenantManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-26 22:00
    /// 描 述：业务类
    /// </summary>
    public class TenantBLL
    {
        private TenantService tenantService = new TenantService();

        #region 获取数据
        public async Task<TData<List<TenantEntity>>> GetList(TenantListParam param)
        {
            TData<List<TenantEntity>> obj = new TData<List<TenantEntity>>();
            obj.Result = await tenantService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<TenantModel>>> GetPageList(TenantListParam param, Pagination pagination)
        {
            TData<List<TenantModel>> obj = new TData<List<TenantModel>>();
            obj.Result = await tenantService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<TenantEntity>> GetEntity(long id)
        {
            TData<TenantEntity> obj = new TData<TenantEntity>();
            obj.Result = await tenantService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(TenantEntity entity)
        {
            TData<string> obj = new TData<string>();
            await tenantService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            obj.Message = "创建成功";
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await tenantService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
