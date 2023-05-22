using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-02 16:21
    /// 描 述：实体类
    /// </summary>
    [Table("testtaskitem")]
    public partial class TestTaskItemEntity
    {

        public static string TblName = "testtaskitem";

        /// <summary>
        /// 产品
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("TaskId", "bigint","", "", "N", "")] 
         public long? TaskId { get; set; }
        /// <summary>
        /// 功能模块
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("GroupId", "bigint","", "", "N", "")] 
         public long? GroupId { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("CaseId", "bigint","", "", "N", "")] 
         public long? CaseId { get; set; }
    
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ProjectGuid", "char","", "", "N", "")] 
         public string ProjectGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("SortNum", "int","", "", "N", "")] 
         public int? SortNum { get; set; }
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("IsEnable", "tinyint","", "", "N", "")] 
         public byte? IsEnable { get; set; }
    }
}
