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
    public partial class TenantEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        

        public TenantEntity()
         {

         }

     
    }
}
