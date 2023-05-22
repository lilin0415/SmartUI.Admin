using System;
using System.Collections.Generic;
using System.Text;
using YiSha.Enum;

namespace YiSha.Model.WebApis
{
    public class UpdateTaskExecStatusBody
    {
        public long TaskId
        {
            get;set;
        }

        public long TaskExecId
        {
            get; set;
        }
        public ExecStatusEnumType ExecStatus
        {
            get; set;
        }

        public FinishStatusEnumType? FinishStatus
        {
            get; set;
        }
        public string Reason
        {
            get; set;
        }
        public DateTime? StartTime
        {
            get; set;
        }
        public DateTime? EndTime
        {
            get; set;
        }
    }
}
