using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{

    /// <summary>
    /// 执行状态和结束状态的组合结果
    /// </summary>
    public enum CompositeStatusEnumType
    {
        /// <summary>
        /// 已就绪
        /// .badge
        /// .badge-white(已就绪)
        /// .badge-inverse(队列中...)
        /// </summary>
        [Description("已就绪")]
        Ready = 0,
        /// <summary>
        /// 初始化中，如下载模本等执行之前的过程
        /// badge-info
        /// </summary>
        [Description("初始化中")]
        Initing = 1,
        /// <summary>
        /// 已开始执行
        /// .badge-primary
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
        /// <summary>
        /// .badge-success
        /// </summary>

        [Description("成功")]
        Succeed = 101,
        /// <summary>
        /// .badge-danger
        /// </summary>
        [Description("失败")]
        Failed = 102,
        /// <summary>
        ///  .badge-warning
        /// </summary>
        [Description("取消")]
        Cancelled = 103,
        /// <summary>
        ///  .badge-warning
        /// </summary>
        [Description("终止")]
        Aborted = 104,
    }
}
