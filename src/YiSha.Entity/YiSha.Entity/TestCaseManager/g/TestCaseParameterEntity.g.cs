using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:37
    /// 描 述：实体类
    /// </summary>
    [Table("testcaseparameter")]
    public partial class TestCaseParameterEntity
    {
     
        /// <summary>
        /// 产品
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("CaseId", "bigint","", "", "N", "")] 
         public long? CaseId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("SceneId", "bigint","", "", "N", "")] 
         public long? SceneId { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("DocumentGuiid", "char","", "", "N", "")] 
         public string DocumentGuiid { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("VarName", "varchar","", "", "N", "")] 
         public string VarName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Value", "varchar","", "", "N", "")] 
         public string Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("UseInheritedValue", "tinyint","", "", "N", "")] 
         public byte? UseInheritedValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("SortNum", "int","", "", "N", "")] 
         public int? SortNum { get; set; }
    }
}
