using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using YiSha.Entity;
using YiSha.Entity.TestTaskManager;

namespace YiSha.Model.TestTaskManager
{
    public class TenantDeviceModel:TenantDeviceEntity
    {
        /// <summary>
        /// 项目
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("Name", "varchar", "", "", "N", "")]
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("IP", "varchar", "", "", "N", "")]
        public string IP
        {
            get; set;
        }
        /// <summary>
        /// 编码
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("MAC", "varchar", "", "", "N", "")]
        public string MAC
        {
            get; set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("LoginName", "varchar", "", "", "N", "")]
        public string LoginName
        {
            get; set;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("UserName", "varchar", "", "", "N", "")]
        public string UserName
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        [DbFieldAttribute("UserLoginTime", "datetime", "", "", "N", "CURRENT_TIMESTAMP")]
        public DateTime? UserLoginTime
        {
            get; set;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[JsonConverter(typeof(DateTimeJsonConverter))]
        //[DbFieldAttribute("LastActiveTime", "datetime", "", "", "N", "CURRENT_TIMESTAMP")]
        //public DateTime? LastActiveTime
        //{
        //    get; set;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[DbFieldAttribute("Remark", "text", "", "", "N", "")]
        //public string Remark
        //{
        //    get; set;
        //}
        public bool IsDeployed
        {
            get;set;
        }

        public bool CheckIsDeployed(DeployTaskEntity deployTask)
        {
            if (deployTask == null)
            {
                this.IsDeployed = false;
                return false;
            }

            if (!string.IsNullOrEmpty(deployTask.AppToken) && this.AppToken == deployTask.AppToken
                || !string.IsNullOrEmpty(deployTask.DeviceGuid) && this.DeviceGuid == deployTask.DeviceGuid)
            {
                this.IsDeployed = true;
                return true;
            }
            else
            {
                this.IsDeployed = false;
                return false;
            }
        }
    }
}
