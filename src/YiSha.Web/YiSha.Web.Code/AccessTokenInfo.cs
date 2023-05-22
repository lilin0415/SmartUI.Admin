using Koo.Utilities.Exceptions;
using Koo.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YiSha.Web.Code
{
    public class AccessTokenInfo
    {
        public long? UserId
        {
            get;set;
        }
        public string DeviceGuid
        {
            get;set;
        }
        public int AppType
        {
            get;set;
        }

        private static string GeneRandomString()
        {
            return RandomHelper.NextString(4, "[0-9a-zA-z]");
        }

        public string ToToken()
        {
            var accessTokenStr = $"v1_{GeneRandomString()}_0_{this.UserId}_{GeneRandomString()}_{this.DeviceGuid}_{this.AppType}_{DateTimeHelper.NowAsLongString}_{TimeSpan.FromHours(12).TotalMinutes}";

            var ret = "A" + SecurityHelper.DESEncryptToBase64(accessTokenStr, "11/a;S.-", "[4+5/*YU");
            return ret;
        }


        public static AccessTokenInfo FromToken(string str)
        {
            Debug.Assert(str[0] == 'A');

            str = str.Substring(1);
            var raw = SecurityHelper.DESDecryptFromBase64(str, "11/a;S.-", "[4+5/*YU");

            var items = raw.Split('_');
            if (items.Count() != 9)
            {
                throw new DataInvalidException("AccessToken格式无效");
            }

            var info = new AccessTokenInfo();
         
            info.UserId = DataConverter.ToLong(items[3]);
            //items[4] 随机数
            info.DeviceGuid = items[5];
            info.AppType = DataConverter.ToInt(items[6]).GetValueOrDefault();

            var startTime = DataConverter.ToDateTime(items[7]);
            var timespan = TimeSpan.FromMinutes(DataConverter.ToInt(items[8]).GetValueOrDefault());
            return info;
        }

    }
}
