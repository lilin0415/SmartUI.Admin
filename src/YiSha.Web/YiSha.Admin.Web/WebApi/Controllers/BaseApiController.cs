using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YiSha.Admin.WebApi.Controllers;
using YiSha.Util.Extension;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseApiController: ControllerBase
    {
        /// <summary>
        /// OperatorInfo
        /// </summary>
        /// <returns></returns>
        [NonActionAttribute]
        public async Task<OperatorInfo> GetCurrentUser()
        {
            return  await Operator.Instance.Current();
        }
        /// <summary>
        /// 
        /// </summary>
        public OperatorInfo CurrentUser
        {
            get
            {
                var task = GetCurrentUser();
                task.Wait();
                return task.Result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserToken
        {
            get
            {
                string token = this.Request.Headers["UserToken"].ParseToString();
                return token;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AccessToken
        {
            get
            {
                string token = this.Request.Headers["AccessToken"].ParseToString();
                return token;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AppToken
        {
            get
            {
                string token = this.Request.Headers["AppToken"].ParseToString();
                return token;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AppType
        {
            get
            {
                string token = this.Request.Headers["AppType"].ParseToString();
                return token;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AppVersion
        {
            get
            {
                string token = this.Request.Headers["AppVersion"].ParseToString();
                return token;
            }
        }
    }


    [AuthorizeApiFilterAttribute]
    public abstract class AuthorizedBaseApiController: BaseApiController
    {

    }
}
