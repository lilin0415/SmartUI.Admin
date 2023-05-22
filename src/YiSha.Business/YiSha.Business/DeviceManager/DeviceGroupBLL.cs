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

namespace YiSha.Business.DeviceManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-04-22 09:41
    /// 描 述：业务类
    /// </summary>
    public class DeviceGroupBLL
    {
        private DeviceGroupService deviceGroupService = new DeviceGroupService();

        #region 获取数据
        public async Task<TData<List<DeviceGroupEntity>>> GetList(DeviceGroupListParam param)
        {
            TData<List<DeviceGroupEntity>> obj = new TData<List<DeviceGroupEntity>>();
            obj.Result = await deviceGroupService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<DeviceGroupEntity>>> GetPageList(DeviceGroupListParam param, Pagination pagination)
        {
            TData<List<DeviceGroupEntity>> obj = new TData<List<DeviceGroupEntity>>();
            obj.Result = await deviceGroupService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<DeviceGroupEntity>> GetEntity(long id)
        {
            TData<DeviceGroupEntity> obj = new TData<DeviceGroupEntity>();
            obj.Result = await deviceGroupService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(DeviceGroupEntity entity)
        {
            TData<string> obj = new TData<string>();
            await deviceGroupService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await deviceGroupService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
