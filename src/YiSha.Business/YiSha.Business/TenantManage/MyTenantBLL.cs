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
    /// 日 期：2022-10-26 22:01
    /// 描 述：业务类
    /// </summary>
    public class MyTenantBLL
    {
        private MyTenantService myTenantService = new MyTenantService();

        #region 获取数据
        public async Task<TData<List<MyTenantEntity>>> GetList(MyTenantListParam param)
        {
            TData<List<MyTenantEntity>> obj = new TData<List<MyTenantEntity>>();
            obj.Result = await myTenantService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<MyTenantEntity>>> GetPageList(MyTenantListParam param, Pagination pagination)
        {
            TData<List<MyTenantEntity>> obj = new TData<List<MyTenantEntity>>();
            obj.Result = await myTenantService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<MyTenantEntity>> GetEntity(long id)
        {
            TData<MyTenantEntity> obj = new TData<MyTenantEntity>();
            obj.Result = await myTenantService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
       
        public async Task<TData<string>> SaveForm(MyTenantEntity entity)
        {
            TData<string> obj = new TData<string>();
            await myTenantService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await myTenantService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
