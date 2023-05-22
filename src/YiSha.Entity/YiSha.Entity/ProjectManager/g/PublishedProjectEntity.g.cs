using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.ProjectManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-20 22:16
    /// 描 述：实体类
    /// </summary>
    [Table("publishedproject")]
    public partial class PublishedProjectEntity
    {
        public static string TblName = "publishedproject";
     
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("ProductId", "bigint","", "", "N", "")] 
         public long? ProductId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("CateId", "bigint","", "", "N", "")] 
         public long? CateId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ProjectGuid", "varchar","", "", "N", "")] 
         public string ProjectGuid { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ProjectType", "tinyint","", "", "N", "")] 
         public byte? ProjectType { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Name", "varchar","", "", "N", "")] 
         public string Name { get; set; }
        /// <summary>
        /// 密码重置token
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Remark", "varchar","", "", "N", "")] 
         public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Version", "varchar","", "", "N", "")] 
         public string Version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("AlignedVersion", "char","", "", "N", "")] 
         public string AlignedVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ReleaseNote", "varchar","", "", "N", "")] 
         public string ReleaseNote { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("ReleaseDate", "datetime","", "", "N", "")] 
         public DateTime? ReleaseDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("OriginalVarJson", "text","", "", "N", "")] 
         public string OriginalVarJson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("VarJson", "text","", "", "N", "")] 
         public string VarJson { get; set; }
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("IsEnable", "tinyint","", "", "N", "")] 
         public byte? IsEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("VisiblePermission", "tinyint","", "", "N", "")] 
         public byte? VisiblePermission { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("FilePath", "varchar","", "", "N", "")] 
         public string FilePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("MD5", "varchar","", "", "Y", "")] 
         public string MD5 { get; set; }

        /// <summary>
        /// 断言数量
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("AssertionCount", "tinyint", "", "", "N", "")]
        public int? AssertionCount
        {
            get; set;
        }

        /// <summary>
        /// 支持并行
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("SupportParallel", "tinyint", "", "", "N", "")]
        public byte? SupportParallel
        {
            get; set;
        }
    }
}
