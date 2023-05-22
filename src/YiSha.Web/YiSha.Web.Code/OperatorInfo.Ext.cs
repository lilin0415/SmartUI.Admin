using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace YiSha.Web.Code
{
    public partial class OperatorInfo
    {

        /// <summary>
        /// 间称
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? DepartmentId
        {
            get; set;
        }
        /// <summary>
        /// 间称
        /// </summary>
        /// <returns></returns>
     
        public string DepartmentName
        {
            get; set;
        }
        /// <summary>
        /// 助记码
        /// </summary>
        /// <returns></returns>
        [NotMapped]
        public byte? RoleType
        {
            get; set;
        }
       
     
        /// <summary>
        /// 间称
        /// </summary>
        /// <returns></returns>
        [NotMapped]
        public string PositionIds
        {
            get; set;
        }
        /// <summary>
        /// 间称
        /// </summary>
        /// <returns></returns>
        [NotMapped]
        public string PositionNames
        {
            get; set;
        }

        /// <summary>
        /// 当前租户下面我的所有的角色
        /// 租户创建人角色、管理员角色、一般用户角色、注册用户角色、游客角色
        /// </summary>
        [NotMapped]
        public List<MyRoleInfo> Roles
        {
            get; private set;
        }

        [NotMapped]
        public string RoleIds
        {
            get; private set;
        }

        [NotMapped]
        public string RoleNames
        {
            get; private set;
        }
       
        public void SetRoles(List<MyRoleInfo> roles)
        {
            this.Roles = roles;
            this.RoleIds = String.Join(",", roles.Select(x => x.Id));
            this.RoleNames = String.Join(",", roles.Select(x => x.RoleName));
           
        }


        //RoleIds
    }
}
