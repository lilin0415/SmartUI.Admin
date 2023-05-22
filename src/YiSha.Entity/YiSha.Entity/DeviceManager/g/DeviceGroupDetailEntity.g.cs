using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.DeviceManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-04-22 09:42
    /// 描 述：实体类
    /// </summary>
    [Table("devicegroupdetail")]
    public partial class DeviceGroupDetailEntity
    {
public static readonly string TblName = "devicegroupdetail";        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("GroupId", "bigint","", "", "N", "")] 
         public long? GroupId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("DeviceId", "bigint","", "", "N", "")] 
         public long? DeviceId { get; set; }
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
