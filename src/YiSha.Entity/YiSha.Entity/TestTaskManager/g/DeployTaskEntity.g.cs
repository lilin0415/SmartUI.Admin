using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-12 21:21
    /// 描 述：实体类
    /// </summary>
    [Table("deploytask")]
    public partial class DeployTaskEntity
    {
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("TaskId", "bigint","", "", "N", "")] 
         public long? TaskId { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("AppToken", "bigint","", "", "N", "")] 
         public string AppToken { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("DeviceGuid", "char","", "", "N", "")] 
         public string DeviceGuid { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("UserId", "bigint","", "", "N", "")] 
         public long? UserId { get; set; }
    }
}
