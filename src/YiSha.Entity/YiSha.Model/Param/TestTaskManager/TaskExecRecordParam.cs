using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YiSha.Util;

namespace YiSha.Model.Param.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-16 18:02
    /// 描 述：实体查询类
    /// </summary>
    public class TaskExecRecordListParam
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string Guid
        {
            get;set;
        }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name
        {
            get;set;
        }
        /// <summary>
        /// 计划ID
        /// </summary>
        public long? TaskId
        {
            get;set;
        }
        /// <summary>
        /// 计划名称
        /// </summary>
        public string TaskName
        {
            get;set;
        }

    }
}
