using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TenantManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-26 22:00
    /// 描 述：实体类
    /// </summary>
    [Table("tenant")]
    public partial class TenantEntity
    {
public static readonly string TblName = "tenant";        /// <summary>
        /// 
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
        /// 间称
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ShortName", "varchar","", "", "N", "")] 
         public string ShortName { get; set; }
        /// <summary>
        /// 助记码
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("mnemonic", "varchar","", "", "N", "")] 
         public string mnemonic { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Contact", "varchar","", "", "N", "")] 
         public string Contact { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Phone", "varchar","", "", "N", "")] 
         public string Phone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("QQ", "varchar","", "", "N", "")] 
         public string QQ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Email", "varchar","", "", "N", "")] 
         public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("PostCode", "varchar","", "", "N", "")] 
         public string PostCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Addr", "varchar","", "", "N", "")] 
         public string Addr { get; set; }
        /// <summary>
        /// 纳税人识别号
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("taxpayer_no", "varchar","", "", "N", "")] 
         public string taxpayer_no { get; set; }
        /// <summary>
        /// 增值税税率
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("tax_rate", "decimal","", "", "N", "")] 
         public decimal? tax_rate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Type", "tinyint","", "", "N", "")] 
         public byte? Type { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("opening_bank", "varchar","", "", "N", "")] 
         public string opening_bank { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("bank_acnt", "varchar","", "", "N", "")] 
         public string bank_acnt { get; set; }
        /// <summary>
        /// 业务人员
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("biz_user_Id", "bigint","", "", "N", "")] 
         public long? biz_user_Id { get; set; }
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
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("VisibleScope", "tinyint", "", "", "N", "")]
        public byte? VisibleScope
        {
            get; set;
        }
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("AllowJoinType", "tinyint", "", "", "N", "")]
        public byte? AllowJoinType
        {
            get; set;
        }
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("DefaultDepartmentId", "bigint", "", "", "N", "")]
        public long? DefaultDepartmentId
        {
            get; set;
        }
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("DefaultRoleIds", "varchar", "", "", "N", "")]
        public string DefaultRoleIds
        {
            get; set;
        }
        /// <summary>
        /// 可用状态
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("DefaultPositionIds", "varchar", "", "", "N", "")]
        public string DefaultPositionIds
        {
            get; set;
        }
        /// <summary>
        /// 所有表的主键
        /// long返回到前端js的时候，会丢失精度，所以转成字符串
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public virtual long? TenantCreatorRoleId
        {
            get; set;
        }
    }
}
