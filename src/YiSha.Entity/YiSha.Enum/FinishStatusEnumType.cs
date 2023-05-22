using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    /// <summary>
    /// 完成状态
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FinishStatusEnumType
    {
        [Description("无")]
        None = 0,
        [Description("成功")]
        Succeeded = 101,
        [Description("失败")]
        Failed = 102,
        [Description("已取消")]
        Cancelled = 103,
        [Description("终止")]
        Aborted = 104,

        /// <summary>
        /// 在执行的时候如用例已禁用,或者前一个错误，导致后面的都没有执行
        /// </summary>
        [Description("跳过")]
        Skipped =105,
    }

}
