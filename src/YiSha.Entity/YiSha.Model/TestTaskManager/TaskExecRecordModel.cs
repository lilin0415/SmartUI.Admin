using System;
using System.Collections.Generic;
using System.Text;
using YiSha.Entity.TestTaskManager;
using YiSha.Enum;

namespace YiSha.Model.TestTaskManager
{
    public class TaskExecRecordModel: TaskExecRecordEntity
    {
        public CompositeStatusEnumType CompositeStatus
        {
            get
            {
                if (this.ExecStatus == (int)ExecStatusEnumType.Finished)
                {
                    return (CompositeStatusEnumType)this.FinishStatus;
                }
                return (CompositeStatusEnumType)this.ExecStatus;
            }
        }
    }
}
