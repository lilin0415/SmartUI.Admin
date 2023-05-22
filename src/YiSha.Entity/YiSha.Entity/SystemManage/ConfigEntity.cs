﻿using System;
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
    public partial class ConfigEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        public ConfigEntity()
         {

         }
    }
}
