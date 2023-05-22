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

namespace YiSha.Admin.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
  
    public class UserController : BaseApiController
    {
        private UserBLL userBLL = new UserBLL();

        #region 获取数据       
        #endregion

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

            TData<UserEntity> userObj = await userBLL.CheckLogin(requestModel.UserName, requestModel.Password, (int)PlatformEnum.WebApi, (AppTypeEnumType)requestModel.AppType);

            if (userObj.Status)
            {
                var userEntity = userObj.Result;


                var userTokenService = new UserTokenService();
                await userTokenService.AddOrUpdate(userEntity.Id, userEntity.UserToken, appType, requestModel.DeviceGuid, requestModel.AppVersion);

                await Operator.Instance.AddCurrent(userEntity.UserToken,true);

                var operatorInfo = await Operator.Instance.GetUserByToken(userEntity.UserToken);

                var deviceEntity = requestModel.ToDeviceEntity();
                deviceEntity.UserId = userObj.Result.Id;

                var deviceService = new DeviceService();
                await deviceService.AddOrSaveForm(deviceEntity);


                var r = operatorInfo.MapTo<LoginResponseModel>();
                r.TenantList = new List<TenantLiteMode>();
                foreach (var x in operatorInfo.MyTenantList)
                {
                    //if (x.TenantIsEnable == 1)
                    {
                        var t = new TenantLiteMode();
                        t.Id = x.TenantId;
                        t.Code = x.TenantCode;
                        t.Name = x.TenantName;
                        r.TenantList.Add(t);
                    }
                }
                
                obj.Result = r;
            }
            obj.Status = userObj.Status;
            obj.Message = userObj.Message;
            return obj;
        }
        #endregion

        [HttpPost]
        public async Task<TData<BindTenantResponseModel>> BindTenant(BindTenantRequestModel requestModel)
        {
            try
            {
                TenantDeviceService service = new TenantDeviceService();
                var r = await service.BindTenant(requestModel);
                return TData.CreateSuccessd(r);
            }
            catch (Exception ex)
            {
                return TData.CreateFailed(new BindTenantResponseModel(), ex.Message);
            }

        }

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