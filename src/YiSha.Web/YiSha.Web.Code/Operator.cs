using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Cache.Factory;
using System.Net.Http;

namespace YiSha.Web.Code
{
    public class Operator
    {
        public static Operator Instance
        {
            get { return new Operator(); }
        }

        private string LoginProvider = GlobalContext.Configuration.GetSection("SystemConfig:LoginProvider").Value;
        private string TokenName = "UserToken"; //cookie name or session name

        public static bool IsWebApi
        {
            get
            {
                var context = GlobalContext.GetService<IHttpContextAccessor>()?.HttpContext;
                if (context.Request.Headers.ContainsKey("UserToken") || context.Request.Query.ContainsKey("UserToken"))
                {
                    return true;
                }
                return false;
            }
        }

        public async Task AddCurrent(string token,bool isWebApi)
        {
            if (isWebApi)
            {
                OperatorInfo user = await new DataRepository().GetUserByToken(token);
                if (user != null)
                {
                    CacheFactory.Cache.SetCache(token, user);
                }
                return;
            }

            switch (LoginProvider)
            {
                case "Cookie":
                    new CookieHelper().WriteCookie(TokenName, token);
                    break;

                case "Session":
                    new SessionHelper().WriteSession(TokenName, token);
                    break;

           
                default:
                    throw new Exception("未找到LoginProvider配置");
            }
        }

        /// <summary>
        /// Api接口需要传入userToken
        /// </summary>
        /// <param name="userToken"></param>
        public void RemoveCurrent(string userToken = "")
        {
            if (string.IsNullOrEmpty(userToken))
            {
                TryGetUserTokenFromCookieOrSession(out userToken);

                switch (LoginProvider)
                {
                    case "Cookie":
                        new CookieHelper().RemoveCookie(TokenName);
                        break;

                    case "Session":
                        new SessionHelper().RemoveSession(TokenName);
                        break;

                    default:
                        throw new Exception("未找到LoginProvider配置");
                }
            }
            else
            {

            }
            if (!string.IsNullOrEmpty(userToken))
            {
                CacheFactory.Cache.RemoveCache(userToken);
            }
            

        }


        public static bool TryGetUserTokenFromApi(out string userToken)
        {
            userToken = string.Empty;
            var context = GlobalContext.GetService<IHttpContextAccessor>()?.HttpContext;
            if (context == null)
            {
                //程序刚启动的时候，添加计划任务，这个时候还没有context
                return false;
            }
            if (context.Request.Headers.ContainsKey("UserToken") || context.Request.Query.ContainsKey("UserToken"))
            {
                if (context.Request.Headers.ContainsKey("UserToken"))
                {
                    userToken = context.Request.Headers["UserToken"];
                }
                if (context.Request.Query.ContainsKey("UserToken"))
                {
                    userToken = context.Request.Query["UserToken"];
                }

                return true;
            }
            return false;
        }

        public bool TryGetUserTokenFromCookieOrSession(out string token)
        {
            token = string.Empty;

            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();

            switch (LoginProvider)
            {
                case "Cookie":
                    if (hca?.HttpContext != null)
                    {
                        token = new CookieHelper().GetCookie(TokenName);
                        return true;
                    }
                    break;

                case "Session":
                    if (hca?.HttpContext != null)
                    {
                        token = new SessionHelper().GetSession(TokenName);
                        return true;
                    }
                    break;
            }
            return false;


        }

        public async Task<OperatorInfo> GetUserByToken(string token)
        {
            OperatorInfo user = null;
            if (string.IsNullOrEmpty(token))
            {
                return user;
            }

            token = token.Trim('"');
            user = CacheFactory.Cache.GetCache<OperatorInfo>(token);
            if (user == null)
            {
                user = await new DataRepository().GetUserByToken(token);
                if (user != null)
                {
                    CacheFactory.Cache.SetCache(token, user);
                }
            }
            return user;
        }

        /// <summary>
        /// 定时任务来说，当前是没有用户的？
        /// </summary>
     
        /// <returns></returns>
        public async Task<OperatorInfo> Current()
        {
            if (TryGetUserTokenFromApi(out string apiUserToken))
            {
                return await GetUserByToken(apiUserToken);
            }
            else if(TryGetUserTokenFromCookieOrSession(out string userToken))
            {
                return await GetUserByToken(userToken);
            }
            return null;
        }

        public OperatorInfo CurrentInfo()
        {
            var operatorInfoTask = Operator.Instance.Current();
            operatorInfoTask.Wait();
            var operatorInfo = operatorInfoTask.Result;
            return operatorInfo;
        }
    }
}
