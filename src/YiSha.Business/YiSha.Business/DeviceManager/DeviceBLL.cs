using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.Param.TestTaskManager;
using YiSha.Service.DeviceManager;
using YiSha.Model.DeviceManager;
using YiSha.Entity.DeviceManager;

namespace YiSha.Business.DeviceManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-12 07:17
    /// 描 述：业务类
    /// </summary>
    public class DeviceBLL
    {
        private DeviceService deviceService = new DeviceService();

        #region 获取数据
        public async Task<TData<List<DeviceModel>>> GetList(DeviceListParam param)
        {
            TData<List<DeviceModel>> obj = new TData<List<DeviceModel>>();
            obj.Result = await deviceService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }
        /// <summary>
        /// 查询我的设备
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<TData<List<DeviceEntity>>> GetMyPageList(DeviceListParam param, Pagination pagination)
        {
            TData<List<DeviceEntity>> obj = new TData<List<DeviceEntity>>();
            obj.Result = await deviceService.GetMyPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }
        /// <summary>
        /// 查找所有在线客户端
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<TData<List<DeviceModel>>> GetOnlinePageList(DeviceListParam param, Pagination pagination)
        {
            TData<List<DeviceModel>> obj = new TData<List<DeviceModel>>();
            obj.Result = await deviceService.GetOnlinePageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }
        public async Task<TData<DeviceEntity>> GetEntity(long id)
        {
            TData<DeviceEntity> obj = new TData<DeviceEntity>();
            obj.Result = await deviceService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        /// <summary>
        /// 查看作业的消费端
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<TData<List<DeviceEntity>>> GetConsumerList(DeviceListParam param)
        {
            var obj = new TData<List<DeviceEntity>>();
            obj.Result = await deviceService.GetConsumerList(param);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(DeviceEntity entity)
        {
            TData<string> obj = new TData<string>();
            await deviceService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await deviceService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
