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
    public partial class UserMsgEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        public UserMsgEntity()
         {

         }

        public override void Create()
        {
            this.ReadTime = DateTimeHelper.Empty1970;
            this.AcKTime = DateTimeHelper.Empty1970;
            this.FromDeleteTime = DateTimeHelper.Empty1970;
            this.ToDeleteTime= DateTimeHelper.Empty1970;

            base.Create();
        }
    }
}
