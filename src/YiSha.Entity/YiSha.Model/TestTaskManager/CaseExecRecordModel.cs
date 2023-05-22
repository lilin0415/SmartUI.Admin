using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using YiSha.Entity.TestTaskManager;
using YiSha.Enum;

namespace YiSha.Model.TestTaskManager
{
    public class CaseExecRecordModel : CaseExecRecordEntity
    {
        public string ProjectMD5
        {
            get;set;
        }
        public int TotalCaseCount
        {
            get;set;
        }
        public string TaskName
        {
            get;set;
        }

        public string TaskExecName
        {
            get;set;
        }
       
        public string GroupName
        {
            get;set;
        }
        public string TaskExecGuid
        {
            get;set;
        }
        public int ConsumeMode
        {
            get;set;
        }
        [JsonConverter(typeof(StringJsonConverter))]
        public long? ConsumerId
        {
            get;set;
        }

        public CompositeStatusEnumType? CompositeStatus
        {
            get
            {
                //id=-1，没有锁定数据的时候，ExecStatus为空

                if (this.ExecStatus == null)
                {
                    return null;
                }
                if (this.ExecStatus == (int)ExecStatusEnumType.Finished)
                {
                    return (CompositeStatusEnumType)this.FinishStatus;
                }
                return (CompositeStatusEnumType)this.ExecStatus;
            }
        }
    }
}
