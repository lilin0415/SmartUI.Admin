using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-04 16:19
    /// 描 述：实体类
    /// </summary>
    [Table("testcasegroup")]
    public partial class TestCaseGroupEntity
    {
        public static string TblName = "testcasegroup";
        /// <summary>
        /// 产品
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("EnvId", "bigint","", "", "N", "")] 
         public long? EnvId { get; set; }
        /// <summary>
        /// 功能模块
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Code", "varchar","", "", "N", "")] 
         public string Code { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Name", "varchar","", "", "N", "")] 
         public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("CaseIds", "varchar","", "", "N", "")] 
         public string CaseIds { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("IsContinueWhenError", "tinyint","", "", "N", "")] 
         public byte? IsContinueWhenError { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("IsParallelMode", "tinyint","", "", "N", "")] 
         public byte? IsParallelMode { get; set; }
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
         [DbFieldAttribute("ExecStatus", "tinyint","", "", "N", "")] 
         public byte? ExecStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("StartTime", "datetime","", "", "N", "")] 
         public DateTime? StartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("EndTime", "datetime","", "", "N", "")] 
         public DateTime? EndTime { get; set; }
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
         [DbFieldAttribute("Reason", "varchar","", "", "N", "")] 
         public string Reason { get; set; }
      
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("IsEnable", "tinyint","", "", "N", "")] 
         public byte? IsEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Remark", "varchar","", "", "N", "")] 
         public string Remark { get; set; }
    }
}
