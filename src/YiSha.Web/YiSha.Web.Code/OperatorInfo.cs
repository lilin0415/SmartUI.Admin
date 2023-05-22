using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Enum;

namespace YiSha.Web.Code
{
    public partial class OperatorInfo
    {
        public static readonly OperatorInfo Empty = new OperatorInfo();

        public long? UserId { get; set; }
        public int? UserStatus { get; set; }
        public int? IsOnline { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }

        public string Portrait { get; set; }

        public int? IsSystem
        {
            get; set;
        }
        public int AppType
        {
            get; set;
        }
        public string UserToken
        {
            get; set;
        }
        /// <summary>
        /// 当前设置的Guid
        /// </summary>
        [NotMapped]
        public string DeviceGuid
        {
            get; set;
        }
        [NotMapped]
        public string AccessToken
        {
            get; set;
        }
        [NotMapped]
        public string AppToken
        {
            get; set;
        }

        /// <summary>
        /// 系统管理员
        /// 可以修改所有数据
        /// </summary>
        [NotMapped]
        public bool HasSystemRole
        {
            get
            {
                return this.IsSystem==1 || this.Roles.Any(x => x.RoleType == (int)RoleTypeEnumType.System);
            }
        }

        /// <summary>
        /// 普通管理员
        /// 
        /// </summary>
        [NotMapped]
        public bool HasAdminRole
        {
            get
            {
                //我的所有角色
                return this.Roles.Any(x => x.RoleType == (int)RoleTypeEnumType.Admin);
            }
        }

        /// <summary>
        /// 是否有管理权限
        /// </summary>
        [NotMapped]
        public bool HasManagerPower
        {
            get
            {
                return this.HasSystemRole || this.HasAdminRole;
            }
        }

       
    }


}
