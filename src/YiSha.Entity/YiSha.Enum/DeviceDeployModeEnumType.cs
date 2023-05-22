using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    public enum DeviceDeployModeEnumType
    {
      
        [Description("所有")]
        All=0,
        [Description("单个客户端")]
        One = 1,
        [Description("客户端组")]
        Multiple = 2,
    }
}
