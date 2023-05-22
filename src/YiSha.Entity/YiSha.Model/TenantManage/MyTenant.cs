using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using YiSha.Entity;
using YiSha.Entity.TenantManage;

namespace YiSha.Model.TenantManage
{
    public class MyTenant:MyTenantEntity
    {
       
      
        public string TenantCode
        {
            get; set;
        }
     
        public string TenantName
        {
            get; set;
        }
      
        public int TenantType
        {
            get;set;
        }
     
        public byte? TenantIsEnable
        {
            get; set;
        }
    }
}
