using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.DeviceManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-12 07:17
    /// 描 述：实体类
    /// </summary>
    public partial class DeviceEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        public DeviceEntity()
         {

         }
    }
}
