using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YiSha.Util;

namespace YiSha.Model.Param.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-16 18:04
    /// 描 述：实体查询类
    /// </summary>
    public class CaseExecRecordListParam
    {
        /// <summary>
        /// 作业ID
        /// </summary>
        public string Guid
        {
            get; set;
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskExecGuid
        {
            get; set;
        }
       /// <summary>
       /// 任务名称
       /// </summary>
        public string TaskExecName
        {
            get; set;
        }
        /// <summary>
        /// 用例编码
        /// </summary>
        public string CaseCode
        {
            get; set;
        }
        /// <summary>
        /// 用例名称
        /// </summary>
        public string CaseName
        {
            get; set;
        }
    }
}
