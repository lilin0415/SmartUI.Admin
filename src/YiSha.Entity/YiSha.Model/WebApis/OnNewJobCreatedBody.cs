using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Model.WebApis
{
    public class OnNewJobCreatedBody
    {
        public long? TenantId
        {
            get;set;
        }
        public long? JobId
        {
            get;set;
        }
    }
}
