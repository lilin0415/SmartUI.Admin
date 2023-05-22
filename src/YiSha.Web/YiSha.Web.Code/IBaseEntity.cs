using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Web.Code
{
    public interface IBaseEntity
    {
        long? Id
        {
            get;set;
        }
        long? BaseCreatorId
        {get;set;
        }
    }
 
}
