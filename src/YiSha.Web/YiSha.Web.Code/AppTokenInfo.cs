using Koo.Utilities.Exceptions;
using Koo.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YiSha.Web.Code
{
    public class AppTokenInfo
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

        public string ToToken()
        {
            var str = $"v1_0_{this.UserId}_{this.DeviceGuid}_{this.AppType}";
            var ret = "B" + SecurityHelper.DESEncryptToBase64(str, "%W.2022@", "[-&HdN7/");

            return ret;
        }

        public static AppTokenInfo FromToken(string str)
        {
            Debug.Assert(str[0] == 'B');

            str = str.Substring(1);
            var raw = SecurityHelper.DESDecryptFromBase64(str, "%W.2022@", "[-&HdN7/");
           
            var items = raw.Split('_');
            if (items.Count() != 5)
            {
                throw new DataInvalidException("AppToken格式无效");
            }

            var info = new AppTokenInfo();
          
            info.UserId = DataConverter.ToLong(items[2]);
            info.DeviceGuid = items[3];
            info.AppType = DataConverter.ToInt(items[4]).GetValueOrDefault();
            return info;
        }

      

    }
}
