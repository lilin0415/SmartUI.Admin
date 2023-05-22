using Koo.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Entity.Exceptions
{
    public class TaskItemIsEmptyException:BizException
    {
        public TaskItemIsEmptyException(string msg):base(msg) 
        {
        }
    }
}
