using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Util;

namespace YiSha.Model.Result
{
    public enum ZtreeInfoNodeType
    {
        /// <summary>
        /// 产品
        /// </summary>
        product,
        //功能模块
        module,
        /// <summary>
        /// 用例模板
        /// </summary>
        project,
        /// <summary>
        /// 用例
        /// </summary>
        @case,
        /// <summary>
        /// 发布变量中的文档
        /// </summary>
        publishedDocument
    }

    public class ZtreeInfo
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public string id { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public string pId { get; set; }

        public string name { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public string nodeType
        {
            get;set;
        }

        public object tag
        {
            get;set;
        }

        [JsonIgnore]
        public object Obj
        {
            get;set;
        }
    }
}
