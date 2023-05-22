using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-02 16:21
    /// 描 述：实体类
    /// </summary>
    public partial class TestTaskItemEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        public TestTaskItemEntity()
         {

         }
    }
}
