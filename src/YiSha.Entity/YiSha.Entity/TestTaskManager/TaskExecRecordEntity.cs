using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-16 18:02
    /// 描 述：实体类
    /// </summary>
    public partial class TaskExecRecordEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        public TaskExecRecordEntity()
         {

         }
    }
}
