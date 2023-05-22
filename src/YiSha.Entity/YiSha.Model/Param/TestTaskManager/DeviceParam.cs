using System;
using System.Collections.Generic;
using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using YiSha.Util;

namespace YiSha.Model.Param.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-12 07:17
    /// 描 述：实体查询类
    /// </summary>
    public class DeviceListParam
    {
        public long? UserId
        {
            get;set;
        }
        public int? ConsumeMode
        {
            get;set;
        }
        [JsonConverter(typeof(StringJsonConverter))]
        public long? ConsumerId
        {
            get;set;
        }
    }
}
