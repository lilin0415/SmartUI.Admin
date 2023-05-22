using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.DeviceManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-12 20:46
    /// 描 述：实体类
    /// </summary>
    [Table("device")]
    public partial class DeviceEntity
    {
      
        /// <summary>
        /// 产品
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Guid", "char","", "", "N", "")] 
         public string Guid { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Name", "varchar","", "", "N", "")] 
         public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("IP", "varchar","", "", "N", "")] 
         public string IP { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("MAC", "varchar","", "", "N", "")] 
         public string MAC { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("LoginName", "varchar","", "", "N", "")] 
         public string LoginName { get; set; }
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
         [DbFieldAttribute("UserName", "varchar","", "", "N", "")] 
         public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("UserLoginTime", "datetime","", "", "N", "CURRENT_TIMESTAMP")] 
         public DateTime? UserLoginTime { get; set; }
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

        /// <summary>
        /// 产品
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        [DbFieldAttribute("AppToken", "bigint", "", "", "N", "")]
        public string AppToken
        {
            get; set;
        }
        /// <summary>
        /// 产品
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        [DbFieldAttribute("AppVersion", "bigint", "", "", "N", "")]
        public string AppVersion
        {
            get; set;
        }
        /// <summary>
        /// 产品
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        [DbFieldAttribute("AppName", "bigint", "", "", "N", "")]
        public string AppName
        {
            get; set;
        }
        /// <summary>
        /// 产品
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        [DbFieldAttribute("AppType", "bigint", "", "", "N", "")]
        public byte? AppType
        {
            get; set;
        }
    }
}
