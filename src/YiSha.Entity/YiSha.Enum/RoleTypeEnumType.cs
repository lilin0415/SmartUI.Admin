using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Enum
{
    /// <summary>
    /// 游客、注册用户、一般用户、管理员、创始人、系统管理员
    /// </summary>
    public enum RoleTypeEnumType
    {
        //[Description("游客")]
        //Guest =0,
     
        //[Description("注册用户")]
        //RegisterUser=3,

        [Description("普通用户")]
        NormalUser = 6,
      
        [Description("管理员")]
        Admin=20,
      
        [Description("系统管理员")]
        System=99, 
      
    }
}
