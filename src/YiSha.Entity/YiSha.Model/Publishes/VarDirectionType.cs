using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Model.Publishes
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InOutDirectionEnumType
    {
        Input,
        Output,
    }
}
