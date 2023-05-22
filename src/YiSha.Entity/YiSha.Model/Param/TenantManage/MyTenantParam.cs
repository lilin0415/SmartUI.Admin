using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YiSha.Util;

namespace YiSha.Model.Param.TenantManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-26 22:01
    /// 描 述：实体查询类
    /// </summary>
    public class MyTenantListParam
    {
        public string UserName
        {
            get; set;
        }

        public string Mobile
        {
            get; set;
        }

        public int? UserStatus
        {
            get; set;
        }

        public long? DepartmentId
        {
            get; set;
        }

        public List<long> ChildrenDepartmentIdList
        {
            get; set;
        }

        public string UserIds
        {
            get; set;
        }
        public string MyTenantIds
        {get;set;
        }
        public DateTime? StartTime
        {
            get;set;
        }
        public DateTime? EndTime
        {
            get;set;
        }
    }
}
