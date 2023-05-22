using Koo.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace YiSha.Model.TestTaskManager
{
    /// <summary>
    /// 执行用例接口
    /// 单个执行用例、用例组
    /// </summary>
    public interface ITaskItemCaseScene
    {
        [JsonConverter(typeof(StringJsonConverter))]
        long? TaskId
        {
            get; set;
        }
        [JsonConverter(typeof(StringJsonConverter))]
        long? TaskItemId
        {
            get; set;
        }
        [JsonConverter(typeof(StringJsonConverter))]
        long? GroupId
        {
            get; set;
        }
        [JsonConverter(typeof(StringJsonConverter))]
        long? CaseId
        {
            get; set;
        }

     
        string Code
        {
            get; set;
        }

        string Name
        {
            get; set;
        }

        byte? ExecStatus
        {
            get; set;
        }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        DateTime? StartTime
        {
            get; set;
        }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        DateTime? EndTime
        {
            get; set;
        }
        byte? FinishStatus
        {
            get; set;
        }

        string Reason
        {
            get; set;
        }
    }
}
