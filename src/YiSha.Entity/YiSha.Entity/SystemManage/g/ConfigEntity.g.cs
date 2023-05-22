using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-02-16 21:00
    /// 描 述：实体类
    /// </summary>
    [Table("config")]
    public partial class ConfigEntity
    {
public static readonly string TblName = "config";        /// <summary>
        /// 分类
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Category", "varchar","", "", "N", "")] 
         public string Category { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Code", "varchar","", "", "N", "")] 
         public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Name", "varchar","", "", "N", "")] 
         public string Name { get; set; }
        /// <summary>
        /// 数量类型
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("DataType", "tinyint","", "", "N", "")] 
         public byte? DataType { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Val", "varchar","", "", "N", "")] 
         public string Val { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Remark", "varchar","", "", "N", "")] 
         public string Remark { get; set; }
    }
}
