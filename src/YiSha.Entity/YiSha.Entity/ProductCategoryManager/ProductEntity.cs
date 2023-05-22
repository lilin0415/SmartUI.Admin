using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ProductCategoryManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-23 23:04
    /// 描 述：实体类
    /// </summary>

    public partial class ProductEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity, ITreeNodeEntity
    {

        [NotMapped]
        public string DisplayName
        {
            get
            {
                return this.Name;
            }
            set
            {
            }
        }
    }
}
