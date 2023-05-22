using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Model.Publishes
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DocumentType
    {
        Project = 0,
        MainFlowDocument = 11,
        SubFlowDocument = 12,
        ExecDocument = 21,
    }
}
