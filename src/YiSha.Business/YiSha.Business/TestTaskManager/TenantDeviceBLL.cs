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
    /// 日 期：2022-11-08 22:14
    /// 描 述：业务类
    /// </summary>
    public class TenantDeviceBLL
    {
        private TenantDeviceService tenantDeviceService = new TenantDeviceService();

        #region 获取数据
        public async Task<TData<List<TenantDeviceModel>>> GetList(TenantDeviceListParam param)
        {
            TData<List<TenantDeviceModel>> obj = new TData<List<TenantDeviceModel>>();
            obj.Result = await tenantDeviceService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<TenantDeviceModel>>> GetPageList(TenantDeviceListParam param, Pagination pagination)
        {
            TData<List<TenantDeviceModel>> obj = new TData<List<TenantDeviceModel>>();
            obj.Result = await tenantDeviceService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<TenantDeviceEntity>> GetEntity(long id)
        {
            TData<TenantDeviceEntity> obj = new TData<TenantDeviceEntity>();
            obj.Result = await tenantDeviceService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(TenantDeviceEntity entity)
        {
            TData<string> obj = new TData<string>();
            await tenantDeviceService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await tenantDeviceService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
