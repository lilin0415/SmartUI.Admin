using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-16 22:12
    /// 描 述：实体类
    /// </summary>
    [Table("caseexecrecord")]
    public partial class CaseExecRecordEntity
    {
        public static string TblName = "caseexecrecord";

        /// <summary>
        /// 应用类型
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
         [DbFieldAttribute("TaskItemId", "bigint","", "", "N", "")] 
         public long? TaskItemId { get; set; }
        /// <summary>
        /// 功能模块
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("TaskExecId", "bigint","", "", "N", "")] 
         public long? TaskExecId
        { get; set; }
     
        /// <summary>
        /// 编码
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("GroupId", "bigint","", "", "N", "")] 
         public long? GroupId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("CaseId", "bigint","", "", "N", "")] 
         public long? CaseId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("EnvId", "bigint","", "", "N", "")] 
         public long? EnvId { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ProjectGuid", "varchar","", "", "N", "")] 
         public string ProjectGuid { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ProjectVersion", "char","", "", "N", "")] 
         public string ProjectVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Code", "varchar","", "", "N", "")] 
         public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Name", "varchar","", "", "N", "")] 
         public string Name { get; set; }
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
        /// 纳税人识别号
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ExecStatus", "tinyint","", "", "N", "")] 
         public byte? ExecStatus { get; set; }
        /// <summary>
        /// 增值税税率
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
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("VarJson", "text","", "", "N", "")] 
         public string VarJson { get; set; }
      
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Remark", "varchar","", "", "N", "")] 
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
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("ConsumeStatus", "tinyint", "", "", "N", "")]
        public byte? ConsumeStatus
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        [DbFieldAttribute("ConsumedTime", "datetime", "", "", "N", "CURRENT_TIMESTAMP")]
        public DateTime? ConsumedTime
        {
            get; set;
        }
        [JsonConverter(typeof(StringJsonConverter))]
        public long? UserId
        {
            get; set;
        }
        public string UserName
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("AppToken", "char", "", "", "N", "")]
        public string AppToken
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("ClientVersion", "varchar", "", "", "N", "")]
        public string AppVersion
        {
            get; set;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("DeviceGuid", "char", "", "", "N", "")]
        public string DeviceGuid
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("DeviceName", "varchar", "", "", "N", "")]
        public string DeviceName
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("DeviceIP", "varchar", "", "", "N", "")]
        public string DeviceIP
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("DeviceMAC", "varchar", "", "", "N", "")]
        public string DeviceMAC
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("DeviceLoginName", "varchar", "", "", "N", "")]
        public string DeviceLoginName
        {
            get; set;
        }
        /// <summary>
        /// 应用类型
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        [DbFieldAttribute("TotalAssertionCount", "bigint", "", "", "N", "")]
        public int? TotalAssertionCount
        {
            get; set;
        }
        /// <summary>
        /// 应用类型
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        [DbFieldAttribute("SucceedAssertionCount", "bigint", "", "", "N", "")]
        public int? SucceedAssertionCount
        {
            get; set;
        }
        /// <summary>
        /// 应用类型
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        [DbFieldAttribute("FailedAssertionCount", "bigint", "", "", "N", "")]
        public int? FailedAssertionCount
        {
            get; set;
        }
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("SupportParallel", "tinyint", "", "", "N", "")]
        public byte? SupportParallel
        {
            get; set;
        }
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

        [DbFieldAttribute("LogFilePath", "varchar", "", "", "N", "")]
        public string LogFilePath
        {
            get; set;
        }

        
        [DbFieldAttribute("Guid", "varchar", "", "", "N", "")]
        public string Guid
        {
            get; set;
        }
    }
}
