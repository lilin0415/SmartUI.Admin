using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-13 14:13
    /// 描 述：实体类
    /// </summary>
    [Table("sysusertoken")]
    public partial class UserTokenEntity
    {
public static readonly string TblName = "sysusertoken";        /// <summary>
        /// 用户名
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("UserId", "bigint","", "", "N", "")] 
         public long? UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Token", "char","", "", "N", "")] 
         public string Token { get; set; }
        /// <summary>
        /// 密码盐值
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("DeviceGuid", "varchar","", "", "N", "")] 
         public string DeviceGuid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("AppType", "tinyint","", "", "N", "")] 
         public byte? AppType { get; set; }
        /// <summary>
        /// 性别(0未知 1男 2女)
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("AppVersion", "varchar","", "", "N", "")] 
         public string AppVersion { get; set; }
        /// <summary>
        /// 首次登录时间
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("FirstVisit", "datetime","", "", "N", "")] 
         public DateTime? FirstVisit { get; set; }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("LastVisit", "datetime","", "", "N", "")] 
         public DateTime? LastVisit { get; set; }
    }
}
