using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-16 22:13
    /// 描 述：实体类
    /// </summary>
    [Table("taskexecrecord")]
    public partial class TaskExecRecordEntity
    {
        public static string TblName = "taskexecrecord";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("EnvId", "bigint","", "", "N", "")] 
         public long? EnvId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("TaskId", "bigint","", "", "N", "")] 
         public long? TaskId { get; set; }
        /// <summary>
        /// 任务组名称
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Name", "varchar","", "", "N", "")] 
         public string Name { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("IsTemp", "tinyint","", "", "N", "")] 
         public byte? IsTemp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("SourceType", "tinyint","", "", "N", "")] 
         public byte? SourceType { get; set; }
        /// <summary>
        /// 任务状态(0禁用 1启用)
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
         [DbFieldAttribute("Reason", "varchar","", "", "N", "")] 
         public string Reason { get; set; }
        /// <summary>
        /// cron表达式
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("CronExpression", "varchar","", "", "N", "")] 
         public string CronExpression { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("NextStartTime", "datetime","", "", "N", "CURRENT_TIMESTAMP")] 
         public DateTime? NextStartTime { get; set; }
        /// <summary>
        /// 运行开始时间
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("StartTime", "datetime","", "", "N", "")] 
         public DateTime? StartTime { get; set; }
        /// <summary>
        /// 运行结束时间
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("EndTime", "datetime","", "", "N", "")] 
         public DateTime? EndTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Remark", "varchar","", "", "N", "")] 
         public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("StopWhenError", "tinyint","", "", "N", "")] 
         public byte? StopWhenError { get; set; }
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
         [DbFieldAttribute("IsEnable", "tinyint","", "", "N", "")] 
         public byte? IsEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("DeviceDeployMode", "tinyint","", "", "N", "")] 
         public byte? DeviceDeployMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("MultipleInstances", "tinyint","", "", "N", "")] 
         public byte? MultipleInstances { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("TotalCaseCount", "int", "", "", "N", "")]
        public int? TotalCaseCount
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("SucceedCaseCount", "int", "", "", "N", "")]
        public int? SucceedCaseCount
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("FailedCaseCount", "int", "", "", "N", "")]
        public int? FailedCaseCount
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("CancelledCaseCount", "int", "", "", "N", "")]
        public int? CancelledCaseCount
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("ConsumeMode", "tinyint", "", "", "N", "")]
        public byte? ConsumeMode
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        [DbFieldAttribute("ConsumerId", "bigint", "", "", "N", "")]
        public long? ConsumerId
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
