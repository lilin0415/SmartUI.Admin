using Koo.Utilities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using YiSha.Util;

namespace YiSha.Entity
{
    public interface ICreatableEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        [Description("创建时间")]
        public DateTime? BaseCreateTime
        {
            get; set;
        }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? BaseCreatorId
        {
            get; set;
        }
    }
    public interface IModifiableEntity
    {
        /// <summary>
        /// 修改时间
        /// </summary>

        DateTime? BaseModifyTime
        {
            get; set;
        }

        /// <summary>
        /// 修改人ID
        /// </summary>
        long? BaseModifierId
        {
            get; set;
        }
    }
    public interface IDeletableEntity
    {
        /// <summary>
        /// 是否删除 1是，0否
        /// </summary>
        [JsonIgnore]
        int? BaseIsDelete
        {
            get; set;
        }
    }
    public interface IVersionableEntity
    {

        /// <summary>
        /// 数据更新版本，控制并发
        /// </summary>
        int? BaseVersion
        {
            get; set;
        }
    }
}
