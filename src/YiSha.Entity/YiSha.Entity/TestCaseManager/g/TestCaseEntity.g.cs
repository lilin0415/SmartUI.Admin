using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-09 22:48
    /// 描 述：实体类
    /// </summary>
    [Table("testcase")]
    public partial class TestCaseEntity
    {
        public static readonly string TblName = "testcase";
        /// <summary>
        /// 应用类型
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("ProductId", "bigint","", "", "N", "")] 
         public long? ProductId { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("CateId", "bigint","", "", "N", "")] 
         public long? CateId { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ProjectGuid", "char","", "", "N", "")] 
         public string ProjectGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("EnvId", "bigint","", "", "N", "")] 
         public long? EnvId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Code", "varchar","", "", "N", "")] 
         public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Name", "varchar","", "", "N", "")] 
         public string Name { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Summary", "varchar","", "", "N", "")] 
         public string Summary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("UsingVersion", "tinyint","", "", "N", "")] 
         public byte? UsingVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("SpecialVersion", "varchar","", "", "N", "")] 
         public string SpecialVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("PrevStartTime", "datetime","", "", "N", "CURRENT_TIMESTAMP")] 
         public DateTime? PrevStartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("PrevEndTime", "datetime","", "", "N", "CURRENT_TIMESTAMP")] 
         public DateTime? PrevEndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("PrevFinishStatus", "tinyint","", "", "N", "")] 
         public byte? PrevFinishStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("PrevReason", "varchar","", "", "N", "")] 
         public string PrevReason { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("StartTime", "datetime","", "", "N", "CURRENT_TIMESTAMP")] 
         public DateTime? StartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("EndTime", "datetime","", "", "N", "CURRENT_TIMESTAMP")] 
         public DateTime? EndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ExecStatus", "tinyint","", "", "N", "")] 
         public byte? ExecStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("FinishStatus", "tinyint","", "", "N", "")] 
         public byte? FinishStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("VarJson", "text","", "", "N", "")] 
         public string VarJson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Reason", "varchar","", "", "N", "")] 
         public string Reason { get; set; }
     
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Remark", "text","", "", "N", "")] 
         public string Remark { get; set; }
     
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("IsEnable", "tinyint","", "", "N", "")] 
         public byte? IsEnable { get; set; }
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("Priority", "tinyint", "", "", "N", "")]
        public byte? Priority
        {
            get; set;
        }

        /// <summary>
        /// 应用类型
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        [DbFieldAttribute("TypeId", "bigint", "", "", "N", "")]
        public long? TypeId
        {
            get; set;
        }
    }
}
