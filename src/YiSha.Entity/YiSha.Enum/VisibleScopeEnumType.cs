using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    public enum VisibleScopeEnumType
    {
        [Description("关闭")]
        Private=0,
        [Description("所有人可见")]
        Public=1,
    }
}
