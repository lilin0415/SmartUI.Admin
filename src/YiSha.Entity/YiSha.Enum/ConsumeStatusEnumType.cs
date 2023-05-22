using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    public enum ConsumeStatusEnumType
    {
        /// <summary>
        /// 对于串行的作业先排队中，
        /// 只有上一个作业完成成，下一个作业才激活（处于待推送）
        /// 取作业的时候，取的都是待推送的。是状态是由上一个作业完成的时候设置的
        /// </summary>
        [Description("排队中")]
        Pending = 0,
        [Description("待推送")]
        Ready=1,
        [Description("已推送")]
        Consumed =2,
       
        [Description("已取消")]
        Cancelled =3,

        /// <summary>
        /// 无效（组的消费状态是无效的）、或者已禁用的，
        /// </summary>
        [Description("无效")]
        Invalid =7,
    }
}
