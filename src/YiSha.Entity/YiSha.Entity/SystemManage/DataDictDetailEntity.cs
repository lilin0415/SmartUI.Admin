using System;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Web.Code;

namespace YiSha.Entity.SystemManage
{
    [Table("sysdatadictdetail")]
    public class DataDictDetailEntity : BaseEntity, ICreatableEntity,IModifiableEntity,IDeletableEntity,IVersionableEntity
    {
        public const string _TblName = "sysdatadictdetail";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string DictType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? DictSort { get; set; }
        /// <summary>
        /// 字典键
        /// </summary>
        /// <returns></returns>
        public int? DictKey { get; set; }
        /// <summary>
        /// 字典值
        /// </summary>
        /// <returns></returns>
        public string DictValue { get; set; }
        public string ListClass { get; set; }
        public int? DictStatus { get; set; }
        public int? IsDefault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }

        public byte? IsSystem
        {
            get; set;
        }


        public override void Create()
        {
            var operatorInfo = Operator.Instance.CurrentInfo();

            this.IsSystem = 0;

            base.Create();
        }
    }
}
