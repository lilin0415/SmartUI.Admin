using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YiSha.Util;

namespace YiSha.Model.Param.ProjectManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-26 22:37
    /// 描 述：实体查询类
    /// </summary>
    public class PublishedProjectListParam
    {
        public long? ProductId
        {get;set;
        }
        public long? CateId
        {get;set;
        }
        public string ProjectGuid
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
    }
}
