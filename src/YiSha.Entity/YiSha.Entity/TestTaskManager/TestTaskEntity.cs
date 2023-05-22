using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-02 16:20
    /// 描 述：实体类
    /// </summary>
    public partial class TestTaskEntity : BaseEntity, ICreatableEntity, IModifiableEntity, IDeletableEntity, IVersionableEntity
    {
        public TestTaskEntity()
         {

         }

        /// <summary>
        /// 推送模式时，指定的消费者
        /// </summary>
        [NotMapped]
        public string ConsumerDisplayName
        {
            get;set;
        }
    }
}
