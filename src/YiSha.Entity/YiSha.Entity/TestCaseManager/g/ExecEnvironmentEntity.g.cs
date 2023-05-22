using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:55
    /// 描 述：实体类
    /// </summary>
    [Table("execenvironment")]
    public partial class ExecEnvironmentEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("Code", "varchar", "", "", "N", "")]
        public string Code
        {
            get; set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("Name", "varchar","", "", "N", "")] 
         public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Remark", "varchar","", "", "N", "")] 
         public string Remark { get; set; }
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
