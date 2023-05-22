using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Model.Publishes
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum VarValueSource
    {
        /// <summary>
        /// 当前变量的值为继承的默认值
        /// </summary>
        Inherit,
        /// <summary>
        /// 当前变量的值为自定义的
        /// </summary>
        Local,
    }
}
