using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    public enum TaskSourceType
    {
        /// <summary>
        /// 计划自动生成
        /// </summary>
        [Description("自动")]
        AutoTask=1,
        /// <summary>
        /// 手动点击计划生成的任务
        /// </summary>
        [Description("手动")]
        ManuallyTask=2
    }
}
