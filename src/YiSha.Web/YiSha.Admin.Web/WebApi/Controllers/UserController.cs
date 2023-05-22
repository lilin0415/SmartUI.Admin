using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Koo.Utilities.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Enum;
using YiSha.Model.Result.SystemManage;
using YiSha.Service.TestTaskManager;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Web.Code;
using YiSha.Model.WebApis;
using YiSha.Service.OrganizationManage;
using YiSha.Admin.Web.WebApi.Controllers;
using Newtonsoft.Json.Linq;
using YiSha.Cache.Factory;
using YiSha.Service.SystemManage;
using Koo.Utilities.Encryption;
using YiSha.Service.Cache;
using YiSha.Service.Cache;
using YiSha.Entity.TestTaskManager;
using YiSha.Service.DeviceManager;
using YiSha.Entity.DeviceManager;

namespace YiSha.Admin.WebApi.Controllers
{
    //[Route("api/[controller]/[action]")]
    //[ApiController]
  
    public class UserController : BaseApiController
    {
        private ConfigCache configCache = new ConfigCache();

        private UserBLL userBLL = new UserBLL();

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData<LoginResponseModel>> Login(LoginRequestModel requestModel)
        {
            TData<LoginResponseModel> obj = new TData<LoginResponseModel>();

            var appType = (AppTypeEnumType)requestModel.AppType;

            var systemConfig = await configCache.GetConfigModel();

            requestModel.Password = RSAEncryption.Decrypt(requestModel.Password, systemConfig.PasswordPrivateKey);
            TData<UserEntity> userObj = await userBLL.CheckLogin(requestModel.UserName, requestModel.Password, (int)PlatformEnum.WebApi, appType);

            if (userObj.Status)
            {
                var userEntity = userObj.Result;


                var userTokenService = new UserTokenService();
                await userTokenService.AddOrUpdate(userEntity.Id, userEntity.UserToken, appType, requestModel.DeviceGuid, requestModel.AppVersion);

                await Operator.Instance.AddCurrent(userEntity.UserToken, true);

                var operatorInfo = await Operator.Instance.GetUserByToken(userEntity.UserToken);

                var tokenInfo = new TokenManagerInfo(userEntity.Id.Value, requestModel.DeviceGuid, requestModel.AppType);

                operatorInfo.AccessToken = tokenInfo.AccessToken;
                operatorInfo.AppToken = tokenInfo.AppToken;
                operatorInfo.AppType = requestModel.AppType;


                #region 保存当前用户登录的客户端名称
                var deviceEntity = new DeviceEntity();

                deviceEntity.Guid = requestModel.DeviceGuid;
                deviceEntity.Name = requestModel.DeviceName;
                deviceEntity.IP = requestModel.DeviceIP;
                deviceEntity.MAC = requestModel.DeviceMAC;
                deviceEntity.LoginName = requestModel.DeviceLoginName;
                
                deviceEntity.UserId = userObj.Result.Id;
                deviceEntity.UserName = requestModel.UserName;
                deviceEntity.UserLoginTime = DateTime.Now;
                deviceEntity.LastActiveTime = DateTime.Now;
                
                deviceEntity.AppToken = tokenInfo.AppToken;
                deviceEntity.AppName = requestModel.AppName;
                deviceEntity.AppVersion = requestModel.AppVersion;
                deviceEntity.AppType = (byte)requestModel.AppType;

                var deviceService = new DeviceService();
                await deviceService.AddOrUpdateDevice(deviceEntity);
                #endregion

                var r = operatorInfo.MapTo<LoginResponseModel>();
             
                
                obj.Result = r;
            }
            obj.Status = userObj.Status;
            obj.Message = userObj.Message;
            return obj;
        }
        #endregion

      

        #region   
        /// <summary>
        /// 用户退出登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public TData LoginOff([FromQuery] string token)
        {
            var obj = new TData();
            Operator.Instance.RemoveCurrent(token);
            obj.Message = "登出成功";
            return obj;
        }
        #endregion

    }
}