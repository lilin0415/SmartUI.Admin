using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.Param.TestTaskManager;
using Koo.Utilities.Helpers;
using Org.BouncyCastle.Asn1.Ocsp;
using NPOI.SS.Formula.Functions;
using YiSha.Web.Code;
using Quartz.Util;
using YiSha.Model.DeviceManager;
using YiSha.Entity.DeviceManager;
using YiSha.Enum;
using YiSha.Model.Param.DeviceManager;

namespace YiSha.Service.DeviceManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-12 07:17
    /// 描 述：服务类
    /// </summary>
    public class DeviceService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<DeviceModel>> GetList(DeviceListParam param)
        {
            var expression = ListFilter(param);
            var list = await BaseRepository().FindList(expression);
            var items = list.ToList();
            return items.MapListTo<DeviceModel>();
        }
        /// <summary>
        /// 查询我的设备
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<List<DeviceEntity>> GetMyPageList(DeviceListParam param, Pagination pagination)
        {
            var currentOpe = GetCurrentUser();

            var expression = ListFilter(param);
            expression = expression.And(x => x.UserId == currentOpe.UserId);

            var list = await BaseRepository().FindList(expression, pagination);
            return list.OrderByDescending(x=>x.LastActiveTime).ToList();
        }

        /// <summary>
        /// 查询所有在线客户端
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<List<DeviceModel>> GetOnlinePageList(DeviceListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);

            if (string.IsNullOrWhiteSpace(pagination.Sort) || pagination.Sort == "Id")
            {
                pagination.Sort = "LastActiveTime";
                pagination.SortType = "desc";
            }

            var list = await BaseRepository().FindList(expression, pagination);
            return list.ToList().MapListTo<DeviceModel>();
        }

        public async Task<DeviceEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<DeviceEntity>(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<List<DeviceEntity>> GetConsumerList(DeviceListParam param)
        {
            var consumeMode = param.ConsumeMode.GetValueOrDefault();
            if (consumeMode == (int)TaskConsumeModeEnumType.SingleClient)
            {
                var expression = ListFilter(param);
                var list = await BaseRepository().FindList(expression);
                var items = list.OrderByDescending(x => x.LastActiveTime).ToList();
                return items;

            }
            else if (consumeMode == (int)TaskConsumeModeEnumType.ClientGroup)
            {
                List<DeviceEntity> ret = new List<DeviceEntity>();
                //throw new Exception("使用DeviceGroupDetailService");
                var groupParam = new DeviceGroupDetailListParam();
                groupParam.GroupId = param.ConsumerId;
                var details = await new DeviceGroupDetailService().GetList(groupParam);
                ret.AddRange(details);
                return ret;
            }
            else
            {
                throw new Exception("消费模式错误");
            }

        }
        #endregion

        #region 提交数据


        public async Task SaveForm(DeviceEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                entity.Create();
                TenantHelper.VerifyIsMyDataOnCreate(entity);
                await BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify();
                TenantHelper.VerifyIsMyData(entity);
                await BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            VerifyIsMyDataOnDelete<DeviceEntity>(ids);
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await BaseRepository().Delete<DeviceEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<DeviceEntity, bool>> ListFilter(DeviceListParam param)
        {
            var expression = LinqExtensions.True<DeviceEntity>();
            if (param != null)
            {
                if (param.ConsumeMode.HasValue && param.ConsumerId.HasValue && param.ConsumerId.Value>0)
                {
                    if (param.ConsumeMode.Value == (int)TaskConsumeModeEnumType.SingleClient)
                    {
                        expression = expression.And(x => x.Id == param.ConsumerId);
                    }
                    else if (param.ConsumeMode.Value == (int)TaskConsumeModeEnumType.ClientGroup)
                    {
                        //throw new Exception("使用DeviceGroupDetailService");
                        //var details = await new DeviceGroupDetailService().GetList()
                        //expression = expression.And(x => x.Id == param.ConsumerId);
                    }
                }
               
            }
            return expression;
        }
        #endregion

        #region api登录注册设备
        /// <summary>
        /// api登录注册设备
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateDevice(DeviceEntity entity)
        {
            var existed = await BaseRepository().FindEntity<DeviceEntity>(x => x.UserId == entity.UserId && x.Guid == entity.Guid);
            if (existed == null)
            {
                entity.Create();
                //Request Header中没有用户及租户信息，无需验证
                //TenantHelper.VerifyIsMyDataOnCreate(entity, false);
                return await BaseRepository().Insert(entity);
            }
            else
            {
                //使用新的数据更新已有的
                entity.Id = existed.Id;
                entity.Modify();

                //TenantHelper.VerifyIsMyData(entity, false);
                return await BaseRepository().Update(entity);
            }
        }
        #endregion
    }
}
