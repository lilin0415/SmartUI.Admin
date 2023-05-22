using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    public enum UsingVersionEnumType
    {
        [Description("最新版本")]
        Lastest =0,
        [Description("指定版本")]
        Special =1,
        [Description("用例版本")]
        TestCaseVersion =2,
    }
}
