using System;
using System.Collections.Generic;
using System.Text;
using YiSha.Entity.TenantManage;

namespace YiSha.Model.TenantManage
{
    public class TenantModel:TenantEntity
    {
        public string CreatorUserName
        {
            get;set;
        }
        //public string DefaultDepartmentName
        //{
        //    get;set;
        //}
    }
}
