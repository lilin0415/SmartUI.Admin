using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiSha.Model.Publishes
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum VariableScope
    {
        None,
        /// <summary>
        /// 系统
        /// </summary>
        [Description("系统")]
        System,
        /// <summary>
        /// 当前项目
        /// </summary>
        [Description("当前项目")]
        Project,
        /// <summary>
        /// 当前文档
        /// </summary>
        [Description("当前文档")]
        Document,
        /// <summary>
        /// 局部变量
        /// 如基于3个选中命令，创建一个新的命令，原来3个命令中的使用的变量将作为这个新命令里面的局部变量
        /// 或者For循环命令中定义一个变量，For循环体中所有的子命令可以使用这些命令
        /// </summary>
        [Description("局部变量")]
        Local,
    }
}
