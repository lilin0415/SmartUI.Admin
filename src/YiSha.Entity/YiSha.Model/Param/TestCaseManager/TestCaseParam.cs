using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YiSha.Util;

namespace YiSha.Model.Param.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:13
    /// 描 述：实体查询类
    /// </summary>
    public class TestCaseListParam
    {
        public long? ProductId
        {
            get;set;
        }
        public long? CateId
        {
            get;set;
        }

        public string ProjectGuid
        {
            get;set;
        }
        public string ProjectName
        {
            get; set;
        }
        public string Code
        {
            get;set;
        }
        public string Name
        {
            get;set;
        }
        public bool? IsEnable
        {
            get;set;
        }

        /// <summary>
        /// 环境，
        /// 当QueryForTaskItems 任务明细页面搜索用例时，搜索指定的环境和0的
        /// </summary>
        public long? EnvId
        {
            get;set;
        }

        /// <summary>
        /// 搜索来源
        /// 用例列表管理页面
        /// QueryForTaskItems 任务明细页面搜索用例
        /// </summary>
        public string SearchSource
        {
            get;set;
        }
    }
}
