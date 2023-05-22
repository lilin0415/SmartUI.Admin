using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    public enum TaskConsumeModeEnumType
    {
      
        [Description("不限制")]
        All=0,
        [Description("客户端")]
        SingleClient = 1,
        [Description("客户端组")]
        ClientGroup = 2,
    }
}
