﻿using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:55
    /// 描 述：实体类
    /// </summary>
    public partial class ExecEnvironmentEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        public ExecEnvironmentEntity()
         {

         }
    }
}
