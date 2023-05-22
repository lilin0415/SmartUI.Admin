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
using Microsoft.AspNetCore.Mvc;
using YiSha.Model.WebApis;
using YiSha.Service.OrganizationManage;
using YiSha.Web.Code;
using Koo.Utilities.Helpers;
using Koo.Utilities.Exceptions;
using YiSha.Model.TestTaskManager;
using Newtonsoft.Json.Linq;
using YiSha.Cache.Factory;
using System.Diagnostics;

namespace YiSha.Service.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-08 22:14
    /// 描 述：服务类
    /// </summary>
    public class TenantDeviceService :  BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<TenantDeviceModel>> GetList(TenantDeviceListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList<TenantDeviceModel>(expression);
            var items = list.ToList();
            return items;//.MapListTo<TenantDeviceModel>();
        }

        public async Task<List<TenantDeviceModel>> GetPageList(TenantDeviceListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList<TenantDeviceModel>(expression, pagination);
            return list.list.ToList();
        }

        public async Task<TenantDeviceEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<TenantDeviceEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(TenantDeviceEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                entity.Create();
                this.VerifyIsMyDataOnCreate(entity);
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify();
                this.VerifyIsMyDataOnModify(entity);
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<TenantDeviceEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private string ListFilter(TenantDeviceListParam param)
        {
            var currOpe = this.GetCurrentUser();

            var sql = $"select a.*" +
                $" ,b.Name" +
                $" ,b.IP" +
                $" ,b.MAC" +
                $" ,b.LoginName" +
                $" ,b.UserName" +
                $" ,b.UserLoginTime" +
                $" from TenantDevice a" +
                $"      inner join Device b " +
                $"          on a.UserId = b.UserId and a.DeviceGuid = b.Guid " +
                $" where 1=1 ";

            return sql;

            //var expression = LinqExtensions.True<TenantDeviceEntity>();
            //if (param != null)
            //{
            //}
            //return expression;
        }
        #endregion


        public async Task<BindTenantResponseModel> BindTenant(BindTenantRequestModel request)
        {
            if (string.IsNullOrEmpty(request.UserToken))
            {
                throw new ArgumentIsEmptyException("认证Token为空");
            }
            if (request.TenantId.GetValueOrDefault() == 0)
            {
                throw new ArgumentIsEmptyException("组织为空");
            }

            if (string.IsNullOrEmpty(request.DeviceGuid))
            {
                throw new ArgumentIsEmptyException("设备编号为空");
            }

            var operatorInfo=CacheFactory.Cache.GetCache<OperatorInfo>(request.UserToken);
            if (operatorInfo == null)
            {
                throw new DataNotExistedException("未找到登录用户的信息");
            }

            var accessToken = TokenHelper.GeneAccessToken(request.TenantId, operatorInfo.UserId, request.DeviceGuid,request.AppType);

            //用于唯一确定一个客户端
            var appToken = TokenHelper.GeneDeviceToken(request.TenantId, operatorInfo.UserId, request.DeviceGuid,request.AppType);

            //AppToken里面已经包含TenantId信息，
            var entity = await this.BaseRepository().FindEntity<TenantDeviceEntity>(x => x.AppToken == appToken);
            if (entity == null)
            {
                entity = new TenantDeviceEntity();
               
                entity.UserId = operatorInfo.UserId;
                entity.DeviceGuid = request.DeviceGuid;
                entity.AppToken = appToken;
                entity.AppVersion = request.AppVersion;
                entity.LastActiveTime = DateTimeHelper.Now;
                entity.IsEnable = 1;

                entity.Create();
                this.VerifyIsMyDataOnCreate(entity);
                await this.BaseRepository().Insert(entity);
            }
            else
            {
              
                entity.UserId = operatorInfo.UserId;
                entity.DeviceGuid = request.DeviceGuid;
                entity.AppToken = appToken;
                entity.AppVersion = request.AppVersion;
                entity.LastActiveTime = DateTimeHelper.Now;
                entity.IsEnable = 1;

                entity.Modify();

                await this.BaseRepository().Update(entity);
            }

           

            var response = new BindTenantResponseModel();
            response.AccessToken= accessToken;
            response.AppToken = appToken;

            response.DepartmentId = operatorInfo.DepartmentId;
            response.DepartmentName = operatorInfo.DepartmentName;
            response.PositionIds = operatorInfo.PositionIds;
            response.PositionNames = operatorInfo.PositionNames;
          
            return response;
        }
    }
}
