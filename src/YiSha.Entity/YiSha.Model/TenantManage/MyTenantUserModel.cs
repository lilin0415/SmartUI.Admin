using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.TenantManage;

namespace YiSha.Model.TenantManage
{
    public class MyTenantUserModel : MyTenantEntity
    {
        /// <summary>
        /// 间称
        /// </summary>
        /// <returns></returns>

        public string DepartmentName
        {
            get; set;
        }

        /// <summary>
        /// 职位Id
        /// </summary>
        [NotMapped]
        public string PositionIds
        {
            get; set;
        }

        /// <summary>
        /// 角色Id
        /// </summary>
        [NotMapped]
        public string RoleIds
        {
            get; set;
        }

        #region 用户信息
        [Description("用户名")]
        public string UserName
        {
            get; set;
        }
      
        [Description("真实姓名")]
        public string RealName
        {
            get; set;
        }
        [Description("性别")]
        public int? Gender
        {
            get; set;
        }
        public string Birthday
        {
            get; set;
        }
        public string Portrait
        {
            get; set;
        }
        public string Email
        {
            get; set;
        }
        public string Mobile
        {
            get; set;
        }
        public string QQ
        {
            get; set;
        }
        public string Wechat
        {
            get; set;
        }
        #endregion
    }
}
