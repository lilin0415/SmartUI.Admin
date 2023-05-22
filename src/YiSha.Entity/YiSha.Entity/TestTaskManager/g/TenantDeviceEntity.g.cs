using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-08 22:14
    /// 描 述：实体类
    /// </summary>
    [Table("tenantdevice")]
    public partial class TenantDeviceEntity
    {
public static readonly string TblName = "tenantdevice";        /// <summary>
        /// 应用类型
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("AppToken", "char","", "", "N", "")] 
         public string AppToken { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("DeviceGuid", "char","", "", "N", "")] 
         public string DeviceGuid { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("UserId", "bigint","", "", "N", "")] 
         public long? UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("LastActiveTime", "datetime","", "", "N", "CURRENT_TIMESTAMP")] 
         public DateTime? LastActiveTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("AppVersion", "varchar","", "", "N", "")] 
         public string AppVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Remark", "text","", "", "N", "")] 
         public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("SortNum", "int","", "", "N", "")] 
         public int? SortNum { get; set; }
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("IsEnable", "tinyint","", "", "N", "")] 
         public byte? IsEnable { get; set; }
    }
}
