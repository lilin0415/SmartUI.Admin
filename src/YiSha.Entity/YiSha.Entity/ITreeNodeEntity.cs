using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Entity
{
    public interface ITreeNodeEntity: IDisplayName
    {
        long? Id
        {
            get;set;
        }
        long? ParentId
        {
            get;set;
        }
    }
}
