using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Model.TestTaskManager
{
    public class SavingTestTaskItem
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Id
        {
            get;set;
        }
        public int? SortNum
        {
        get;set;}
    }
}
