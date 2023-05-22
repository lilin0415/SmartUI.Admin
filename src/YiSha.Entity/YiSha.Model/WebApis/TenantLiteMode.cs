using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Model.WebApis
{
    public class TenantLiteMode
    {
        public long? Id
        {
            get; set;
        }
        public string Code
        {
            get;set;
        }
        public string Name
        {
            get; set;
        }
        public bool IsDefault
        {
            get; set;
        }
    }
}
