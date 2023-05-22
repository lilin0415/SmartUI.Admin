using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ProjectManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-26 22:37
    /// 描 述：实体类
    /// </summary>
    public partial class PublishedProjectEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        public PublishedProjectEntity()
         {

         }
    }
}
