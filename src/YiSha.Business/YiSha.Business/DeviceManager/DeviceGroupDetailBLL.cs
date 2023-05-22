using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.DeviceManager;
using YiSha.Model.Param.DeviceManager;
using YiSha.Service.DeviceManager;
using YiSha.Model.DeviceManager;

namespace YiSha.Business.DeviceManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-04-22 09:42
    /// 描 述：业务类
    /// </summary>
    public class DeviceGroupDetailBLL
    {
        private DeviceGroupDetailService deviceGroupDetailService = new DeviceGroupDetailService();

        #region 获取数据
        public async Task<TData<List<DeviceGroupDetailModel>>> GetList(DeviceGroupDetailListParam param)
        {
            var obj = new TData<List<DeviceGroupDetailModel>>();
            obj.Result = await deviceGroupDetailService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<DeviceGroupDetailModel>>> GetPageList(DeviceGroupDetailListParam param, Pagination pagination)
        {
            TData<List<DeviceGroupDetailModel>> obj = new TData<List<DeviceGroupDetailModel>>();
            obj.Result = await deviceGroupDetailService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<DeviceGroupDetailEntity>> GetEntity(long id)
        {
            TData<DeviceGroupDetailEntity> obj = new TData<DeviceGroupDetailEntity>();
            obj.Result = await deviceGroupDetailService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(DeviceGroupDetailEntity entity)
        {
            TData<string> obj = new TData<string>();
            await deviceGroupDetailService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await deviceGroupDetailService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
