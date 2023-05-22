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
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Web.Code;
using Koo.Utilities.Helpers;
using Koo.Utilities.Exceptions;
using YiSha.Cache.Factory;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.WebApis;

namespace YiSha.Service.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-13 14:13
    /// 描 述：服务类
    /// </summary>
    public class UserTokenService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<UserTokenEntity>> GetListByUserId(long userId)
        {
            var expression = LinqExtensions.True<UserTokenEntity>();
            expression = expression.And(x => x.UserId == userId);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }
        public async Task<List<UserTokenEntity>> GetList(UserTokenListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<UserTokenEntity>> GetPageList(UserTokenListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<UserTokenEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<UserTokenEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(UserTokenEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify();
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<UserTokenEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<UserTokenEntity, bool>> ListFilter(UserTokenListParam param)
        {
            var expression = LinqExtensions.True<UserTokenEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion

        public async Task AddOrUpdate(long? userId, string token, AppTypeEnumType appType,string deviceGuid="",string appVersion="")
        {
            var entity = new UserTokenEntity();
            entity.UserId = userId;
            entity.Token = token;
            entity.AppType = (byte)appType;
            entity.DeviceGuid = deviceGuid;
            entity.AppVersion = appVersion;

            entity.FirstVisit = DateTimeHelper.Now;
            entity.LastVisit = DateTimeHelper.Now;

            entity.Create();

            await base.BaseRepository().Insert(entity);
        }

        public async Task<BindTenantResponseModel> BindTenant(BindTenantRequestModel request)
        {
            if (string.IsNullOrEmpty(request.UserToken))
            {
                throw new ArgumentIsEmptyException("认证Token为空");
            }

            if (string.IsNullOrEmpty(request.DeviceGuid))
            {
                throw new ArgumentIsEmptyException("设备编号为空");
            }

            var operatorInfo = CacheFactory.Cache.GetCache<OperatorInfo>(request.UserToken);
            if (operatorInfo == null)
            {
                throw new DataNotExistedException("未找到登录用户的信息");
            }

            var accessToken = TokenHelper.GeneAccessToken(operatorInfo.UserId, request.DeviceGuid, request.AppType);

            //用于唯一确定一个客户端
            var appToken = TokenHelper.GeneDeviceToken(operatorInfo.UserId, request.DeviceGuid, request.AppType);

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
                //this.VerifyIsMyDataOnCreate(entity);
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
            response.AccessToken = accessToken;
            response.AppToken = appToken;

            response.DepartmentId = operatorInfo.DepartmentId;
            response.DepartmentName = operatorInfo.DepartmentName;
            response.PositionIds = operatorInfo.PositionIds;
            response.PositionNames = operatorInfo.PositionNames;

            return response;
        }
    }
}
