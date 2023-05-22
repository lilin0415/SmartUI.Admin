using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-24 10:41
    /// 描 述：实体类
    /// </summary>
    [Table("usermsg")]
    public partial class UserMsgEntity
    {
public static readonly string TblName = "usermsg";        /// <summary>
        /// 发送方id	
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("FromId", "bigint","", "", "N", "")] 
         public long? FromId { get; set; }
        /// <summary>
        /// 接收方id	
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
         [DbFieldAttribute("ToId", "bigint","", "", "N", "")] 
         public long? ToId { get; set; }
     
        /// <summary>
        /// 消息类型
        /// </summary>
        /// <returns></returns>
        [DbFieldAttribute("MsgType", "tinyint","", "", "N", "")] 
         public byte? MsgType { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Title", "varchar","", "", "N", "")] 
         public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Content", "text","", "", "N", "")] 
         public string Content { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ContentType", "tinyint","", "", "N", "")] 
         public byte? ContentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("Token", "varchar","", "", "N", "")] 
         public string Token { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("IsRead", "tinyint","", "", "N", "")] 
         public byte? IsRead { get; set; }
        /// <summary>
        /// 阅读时间
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("ReadTime", "datetime","", "", "N", "")] 
         public DateTime? ReadTime { get; set; }
        /// <summary>
        /// 回复状态
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("AckStatus", "tinyint","", "", "N", "")] 
         public byte? AckStatus { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("AcKTime", "datetime","", "", "N", "")] 
         public DateTime? AcKTime { get; set; }
        /// <summary>
        /// 来源方是否已删除
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("FromIsDelete", "tinyint","", "", "N", "")] 
         public byte? FromIsDelete { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("FromDeleteTime", "datetime","", "", "N", "")] 
         public DateTime? FromDeleteTime { get; set; }
        /// <summary>
        /// 接收方是否已删除
        /// </summary>
        /// <returns></returns>
         [DbFieldAttribute("ToIsDelete", "tinyint","", "", "N", "")] 
         public byte? ToIsDelete { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
         [DbFieldAttribute("ToDeleteTime", "datetime","", "", "N", "")] 
         public DateTime? ToDeleteTime { get; set; }
    }
}
