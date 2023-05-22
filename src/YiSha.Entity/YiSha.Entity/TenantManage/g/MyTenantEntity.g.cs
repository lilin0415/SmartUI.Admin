using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TenantManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-26 22:01
    /// 描 述：实体类
    /// </summary>
    [Table("mytenant")]
    public partial class MyTenantEntity
    {
public static readonly string TblName = "mytenant";        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("UserId", "bigint","", "", "N", "")] 
         public long? UserId { get; set; }
        /// <summary>
        /// 间称
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("DepartmentId", "bigint","", "", "N", "")] 
         public long? DepartmentId { get; set; }
        /// <summary>
        /// 助记码
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("RoleType", "tinyint","", "", "N", "")] 
         public byte? RoleType { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("JoinTime", "datetime","", "", "N", "CURRENT_TIMESTAMP")] 
         public DateTime? JoinTime { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("LeftTime", "datetime","", "", "N", "CURRENT_TIMESTAMP")] 
         public DateTime? LeftTime
        { get; set; }
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
         [DbFieldAttribute("IsLeft", "tinyint","", "", "N", "")] 
         public byte? IsLeft { get; set; }


        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("IsEnable", "tinyint","", "", "N", "")] 
         public byte? IsEnable { get; set; }

        [DbFieldAttribute("Remark", "varchar", "", "", "N", "")]
        public string Remark
        {
            get; set;
        }
    }
}
