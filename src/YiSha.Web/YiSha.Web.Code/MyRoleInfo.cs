using Koo.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using YiSha.Enum;

namespace YiSha.Web.Code
{
    public class MyRoleInfo
    {
      
        public long? Id
        {
            get;set;
        }

        public string RoleName
        {
            get; set;
        }
     
        public int? RoleStatus
        {
            get; set;
        }
      
        public byte? IsSystem
        {
            get; set;
        }
      
        public byte? RoleType
        {
            get; set;
        }



        [NotMapped]
        public string RoleTypeDisplayName
        {
            get
            {
                return DescriptionHelper.GetDescription((RoleTypeEnumType)this.RoleType);
            }
        }

        /// 角色对应的菜单，页面和按钮
        /// </summary>
        [NotMapped]
        public string MenuIds
        {
            get; set;
        }
    }
}
