using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-03-25 19:49
    /// 描 述：实体类
    /// </summary>
    [Table("caseexeclog")]
    public partial class CaseExecLogEntity
    {
public static readonly string TblName = "caseexeclog";        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("CaseExecId", "bigint","", "", "N", "")] 
         public long? CaseExecId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("LogId", "varchar","", "", "N", "")] 
         public string LogId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("LogType", "tinyint","", "", "N", "")] 
         public byte? LogType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("DateTime", "datetime","", "", "N", "")] 
         public DateTime? DateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Message", "varchar","", "", "N", "")] 
         public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Level", "tinyint","", "", "N", "")] 
         public byte? Level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("TransStep", "tinyint","", "", "N", "")] 
         public byte? TransStep { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ExecutionPathName", "varchar","", "", "N", "")] 
         public string ExecutionPathName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ExecutionPathId", "varchar","", "", "N", "")] 
         public string ExecutionPathId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ExecutionPathMd5", "varchar","", "", "N", "")] 
         public string ExecutionPathMd5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ExecutorId", "varchar","", "", "N", "")] 
         public string ExecutorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ExecutorTypeName", "varchar","", "", "N", "")] 
         public string ExecutorTypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ExecutorName", "varchar","", "", "N", "")] 
         public string ExecutorName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("LineNumber", "int","", "", "N", "")] 
         public int? LineNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("EndTime", "datetime", "", "", "N", "")] 
         public DateTime? EndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ElapsedTime", "int","", "", "N", "")] 
         public int? ElapsedTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Status", "tinyint","", "", "N", "")] 
         public byte? Status { get; set; }
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
         [DbFieldAttribute("AssertStatus", "tinyint","", "", "N", "")] 
         public byte? AssertStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Assert", "varchar","", "", "N", "")] 
         public string Assert { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("InputParameters", "varchar","", "", "N", "")] 
         public string InputParameters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("OutputParameters", "varchar","", "", "N", "")] 
         public string OutputParameters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("BeforeScreenshot", "varchar","", "", "N", "")] 
         public string BeforeScreenshot { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("AfterScreenshot", "varchar","", "", "N", "")] 
         public string AfterScreenshot { get; set; }
    }
}
