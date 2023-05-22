using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    public enum TaskInstancesPolicy
    {
        [Description("并行运行新实例")]
        Parallel=0, 

        [Description("对新实例排队")]
        Queue=1,

        [Description("请勿启动新实例")]
        IgnoreNew=2,

        [Description("停止现有实例")]
        StopExisting=3
    }

}
