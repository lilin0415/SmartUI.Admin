using Koo.Utilities.Exceptions;
using Koo.Utilities.Helpers;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YiSha.Web.Code
{
    public class TokenManagerInfo
    {
        public string AccessToken
        {
            get;private set;
        }
        /// <summary>
        /// 用于唯一确定一个客户端
        /// 由用户id、电脑guid、客户端类型
        /// </summary>
        public string AppToken
        {
            get;private set;
        }

        public TokenManagerInfo(long userId,string deviceGuid,int appType)
        {
            this.AccessToken = TokenHelper.GeneAccessToken(userId, deviceGuid, appType);

            //用于唯一确定一个客户端
            this.AppToken = TokenHelper.GeneDeviceToken(userId, deviceGuid, appType);
        }
    }

    public class TokenHelper
    {
        public static string GeneAccessToken(long? userId, string deviceGuid, int appType)
        {
            var info = new AccessTokenInfo();
         
            info.UserId = userId;
            info.DeviceGuid = deviceGuid;
            info.AppType = appType;

            return info.ToToken();
        }
        public static AccessTokenInfo ParseAccessToken(string token)
        {
            return AccessTokenInfo.FromToken(token);
        }

        public static string GeneDeviceToken(long? userId,string deviceGuid,int appType)
        {
            var info = new AppTokenInfo();
            info.UserId = userId;
            info.DeviceGuid = deviceGuid;
            info.AppType = appType;

            return info.ToToken();
        }

        public static AppTokenInfo ParseAppToken(string token)
        {
            return AppTokenInfo.FromToken(token);
        }





    }
}
