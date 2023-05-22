using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Model.Publishes
{
    /// <summary>
    /// 变量值类型
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum VariableDataType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        [Description("字符串")]
        String,
        /// <summary>
        /// 整数
        /// </summary>
        [Description("整数")]
        Int,
        /// <summary>
        /// 小数decimal
        /// </summary>
        [Description("小数")]
        Decimal,

        [Description("逻辑值")]
        Boolean,

        [Description("日期/时间")]
        DateTime,

        [Description("密码")]
        Password,

        [Description("列表")]
        ArrayList,

        [Description("字典")]
        Dictionary,

        /// <summary>
        /// 对象，可以代表数组、字典、表格等
        /// </summary>
        Object,
    }
}
