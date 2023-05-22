using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using YiSha.Entity.TestTaskManager;
using YiSha.Web.Code;

namespace YiSha.Model.WebApis
{
    public class LoginRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string DeviceGuid
        {
            get; set;
        }
        public string DeviceName
        {
            get; set;
        }
        public string DeviceIP
        {
            get; set;
        }
        public string DeviceMAC
        {
            get; set;
        }
        public string DeviceLoginName
        {
            get; set;
        }
    
        public int AppType
        {
            get;set;
        }
        public string AppVersion
        {
            get;set;
        }
        public string AppName
        {
            get; set;
        }
    }

    public class LoginResponseModel
    {
        public long? UserId
        {
            get; set;
        }
        public int? UserStatus
        {
            get; set;
        }
        public int? IsOnline
        {
            get; set;
        }
        public string UserName
        {
            get; set;
        }
        public string RealName
        {
            get; set;
        }
    
      
        public int? IsSystem
        {
            get; set;
        }
        public string Portrait
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
        public string DeviceGuid
        {
            get; set;
        }
        public string AccessToken
        {
            get; set;
        }
        public string AppToken
        {
            get; set;
        }
        /// <summary>
        /// 间称
        /// </summary>
        /// <returns></returns>
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
    }
}
