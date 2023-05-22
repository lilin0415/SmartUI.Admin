using Koo.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using YiSha.Entity.SystemManage;
using YiSha.Enum;

namespace YiSha.Model
{
    public class RoleModel:RoleEntity
    {
        public string RoleTypeDisplayName
        {
            get
            {
                return DescriptionHelper.GetDescription((RoleTypeEnumType)this.RoleType);
            }
        }
    }
}
