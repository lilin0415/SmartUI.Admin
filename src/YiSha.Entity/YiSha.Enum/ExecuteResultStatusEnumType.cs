using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    public enum ExecuteResultStatusEnumType
    {
        /// <summary>
        /// 命令未执行
        /// </summary>
        None = 0,
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
        ///  命令没执行完成，如子命令包含错误，直接抛出异常
        /// </summary>
        [Description("终止")]
        Aborted = 104,

    }
}
