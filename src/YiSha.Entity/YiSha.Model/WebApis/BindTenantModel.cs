using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using YiSha.Entity.TestTaskManager;
using YiSha.Enum;

namespace YiSha.Model.WebApis
{
    public class BindTenantRequestModel
    {
        public string UserToken
        { get; set; }
        public long? TenantId
        { get; set; }

        public string DeviceGuid
        {
            get; set;
        }
        public int AppType
        {
            get;set;
        }

        public string AppVersion
        {
            get; set;
        }
       
    }

    public class BindTenantResponseModel
    {
        /// <summary>
        /// 访问token
        /// </summary>
        public string AccessToken
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string AppToken
        {
            get; set;
        }

        public long? DepartmentId
        {
            get; set;
        }

        [NotMapped]
        public string DepartmentName
        {
            get; set;
        }

        /// <summary>
        /// 岗位Id
        /// </summary>
        [NotMapped]
        public string PositionIds
        {
            get; set;
        }
        /// <summary>
        /// 岗位Id
        /// </summary>
        [NotMapped]
        public string PositionNames
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
      
    }
}
