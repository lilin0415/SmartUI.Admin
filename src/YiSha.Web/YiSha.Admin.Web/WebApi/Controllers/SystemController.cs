using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Koo.Utilities.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.Web.WebApi.Controllers;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.SystemManage;
using YiSha.Model;
using YiSha.Model.Param;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Service.Cache;
using YiSha.Service.SystemManage;
using YiSha.Util.Model;

namespace YiSha.Admin.WebApi.Controllers
{
    //[Route("api/[controller]/[action]")]
    //[ApiController]
    
    public class SystemController : BaseApiController
    {
        private ConfigService configService = new ConfigService();

        #region 获取数据
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<SystemConfigModel>> GetConfig()
        {
            ConfigCache configCache = new ConfigCache();
            var data = await configCache.GetConfigModel();

            data = JsonHelper.JsonCopy(data);
            data.PasswordPrivateKey=string.Empty;

            TData<SystemConfigModel> obj = new TData<SystemConfigModel>();
            obj.Status = true;
            obj.Result= data;
            return obj;
        }

        #endregion


    }
}