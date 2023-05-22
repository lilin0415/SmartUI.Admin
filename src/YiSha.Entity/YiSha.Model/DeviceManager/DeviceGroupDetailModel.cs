using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using YiSha.Entity;
using YiSha.Entity.DeviceManager;
using YiSha.Entity.TestTaskManager;

namespace YiSha.Model.DeviceManager
{
    public class DeviceGroupDetailModel: DeviceEntity
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? DetailId
        {
            get; set;
        }
        [JsonConverter(typeof(StringJsonConverter))]
        public long? GroupId
        {
            get;set;
        }
       
    }
}
