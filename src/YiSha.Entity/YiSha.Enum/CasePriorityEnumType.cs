using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    public enum CasePriorityEnumType
    {
        /// <summary>
        /// 计划自动生成
        /// </summary>
        [Description("P1")]
        A=1,
        /// <summary>
        /// 手动点击计划生成的任务
        /// </summary>
        [Description("P2")]
        B=2,
        /// <summary>
        /// 手动点击计划生成的任务
        /// </summary>
        [Description("P3")]
        C = 3,
        /// <summary>
        /// 手动点击计划生成的任务
        /// </summary>
        [Description("P4")]
        D = 4,
        /// <summary>
        /// 手动点击计划生成的任务
        /// </summary>
        [Description("P5")]
        E = 5,
    }
}
