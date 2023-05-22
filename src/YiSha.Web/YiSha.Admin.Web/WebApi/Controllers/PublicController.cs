using Koo.Utilities.Encryption;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.TestTaskManager;
using YiSha.Enum;
using YiSha.Model.WebApis;
using YiSha.Service.Cache;
using YiSha.Service.SystemManage;
using YiSha.Service.TestTaskManager;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.WebApi.Controllers
{
    public class PublicController: BaseApiController
    {
        //OfficialModel
        #region 检查版本
        /// <summary>
        /// 检查版本
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<OfficialModel>> GetOfficialInfo()
        {
            TData<OfficialModel> obj = new TData<OfficialModel>();

            obj.Result = OfficialModel.Instance;
            obj.Status = true;
            obj.Message = string.Empty;
            return obj;
        }
        #endregion

        #region 检查版本
        /// <summary>
        /// 检查版本
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<CheckNewVersionResponse>> CheckNewVersion(int appType,string appVersion /*CheckNewVersionRequest requestModel*/)
        {
            TData<CheckNewVersionResponse> obj = new TData<CheckNewVersionResponse>();

            //var appType = (AppTypeEnumType)requestModel.AppType;

            var res = new CheckNewVersionResponse();
            res.HasNewVersion = true;
            res.NewVersion = "xxx";
            res.IsOptional = false;
            res.DownloadUrl = "";
            obj.Result = res;
            obj.Status = true;
            obj.Message = string.Empty;
            return obj;
        }
        #endregion

    }
}
