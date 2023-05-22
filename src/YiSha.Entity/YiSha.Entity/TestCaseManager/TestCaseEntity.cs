using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:13
    /// 描 述：实体类
    /// </summary>
    public partial class TestCaseEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        public TestCaseEntity()
         {

         }
        [NotMapped]
        public bool HasBeenUsed
        {
            get;set;
        }
        //[NotMapped]
        //public long? ProductId
        //{
        //    get; set;
        //}
        //[NotMapped]
        //public string ProductName
        //{
        //    get; set;
        //}
        //[NotMapped]
        //public long? CateId
        //{
        //    get; set;
        //}
        /// <summary>
        /// 分类名称
        /// </summary>
        //[NotMapped]
        //public string CateName
        //{
        //    get; set;
        //}

        /// <summary>
        /// 产品、模块的完整名称
        /// </summary>
        [NotMapped]
        public string ProductCateFullName
        {
            get; set;
        }


        /// <summary>
        /// 模板名称
        /// </summary>
        [NotMapped]
        public string ProjectName
        {
            get; set;
        }
        /// <summary>
        /// 断言数量
        /// </summary>
        /// <returns></returns>
        [NotMapped]
        public int? AssertionCount
        {
            get; set;
        }

        /// <summary>
        /// 支持并行,在生成作业的时候通过sql查询值
        /// </summary>
        /// <returns></returns>
        [NotMapped]
        public byte? SupportParallel
        {
            get; set;
        }
    }
}
