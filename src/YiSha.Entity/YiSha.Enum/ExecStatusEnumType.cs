using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace YiSha.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExecStatusEnumType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("已就绪")]
        Ready = 0,
        /// <summary>
        /// 初始化中，如下载模本等执行之前的过程
        /// </summary>
        [Description("初始化中")]
        Initing = 1,
        /// <summary>
        /// 已开始执行
        /// </summary>
        [Description("运行中")]
        Running = 2,
        /// <summary>
        /// 已暂停
        /// </summary>
        [Description("已暂停")]
        Paused = 3,
        /// <summary>
        /// 已结束
        /// </summary>
        [Description("已结束")]
        Finished = 4,

        /// <summary>
        /// 用于区分从暂停到运行的事件状态
        /// </summary>
        Resumed = 10,
    }
}
