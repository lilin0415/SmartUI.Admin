using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    public enum AllowJoinTypeEnumType
    {
        [Description("禁止加入")]
        Forbid=0,
        [Description("需要审核")]
        JoinAndAudit =1,
        [Description("直接加入")]
        JoinWithoutAudit =2,
    }
}
