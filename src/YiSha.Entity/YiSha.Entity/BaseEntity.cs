using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Web.Code;
using YiSha.IdGenerator;
using Koo.Utilities.Helpers;
using Koo.Utilities.Exceptions;
using NPOI.POIFS.FileSystem;
using YiSha.Data.Repository;

namespace YiSha.Entity
{
    public class BaseCreatableEntity : IBaseEntity, ICreatableEntity
    {

        /// <summary>
        /// 所有表的主键
        /// long返回到前端js的时候，会丢失精度，所以转成字符串
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public virtual long? Id
        {
            get; set;
        }

        #region 创建信息
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        [Description("创建时间")]
        public virtual DateTime? BaseCreateTime
        {
            get; set;
        }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public virtual long? BaseCreatorId
        {
            get; set;
        }
        #endregion
        public void ResetBaseCreatorAndModifier()
        {
            this.BaseCreateTime = null;
            this.BaseCreatorId = null;
        }

        #region create modify
        public virtual void Create()
        {
            this.SetupCreatorInfo();
            this.SetupModifyInfo();

        }

        public virtual void SetupCreatorInfo()
        {
           
            if (this.Id == null || this.Id == 0)
            {
                this.Id = IdGeneratorHelper.Instance.GetId();
            }

            if (this.BaseCreateTime == null)
            {
                this.BaseCreateTime = DateTimeHelper.Now;
            }

            if (this.BaseCreatorId == null)
            {
                OperatorInfo user = Operator.Instance.CurrentInfo();
                if (user != null)
                {
                    this.BaseCreatorId = user.UserId;
                }
            }

            if (this.BaseCreatorId == null)
            {
                this.BaseCreatorId = 0;
            }
        }

        public virtual void Modify()
        {
            if (this.Id == null)
            {
                throw new BizException("数据Id为空，无法更新");
            }
            SetupModifyInfo();
        }

        public virtual void SetupModifyInfo()
        {
           
        }
        #endregion
    }
    /// <summary>
    /// 数据库实体的基类，所有的数据库实体属性类型都是可空值类型，为了在做条件查询的时候进行判断
    /// 虽然是可空值类型，null的属性值，在底层会根据属性类型赋值默认值，字符串是string.empty，数值是0，日期是1970-01-01
    /// </summary>
    public class BaseEntity:IBaseEntity
    {
      
        /// <summary>
        /// 所有表的主键
        /// long返回到前端js的时候，会丢失精度，所以转成字符串
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public virtual long? Id { get; set; }

        #region 创建信息
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        [Description("创建时间")]
        public virtual DateTime? BaseCreateTime
        {
            get; set;
        }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public virtual long? BaseCreatorId
        {
            get; set;
        }
        #endregion

        #region 修改信息
        /// <summary>
        /// 修改时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        [Description("修改时间")]
        public virtual DateTime? BaseModifyTime
        {
            get; set;
        }

        /// <summary>
        /// 修改人ID
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public virtual long? BaseModifierId
        {
            get; set;
        }
        #endregion

        /// <summary>
        /// 数据更新版本，控制并发
        /// </summary>
        public virtual int? BaseVersion
        {
            get; set;
        }

        /// <summary>
        /// 是否删除 1是，0否
        /// </summary>
        [JsonIgnore]
        public virtual int? BaseIsDelete
        {
            get; set;
        }
        public void ResetBaseCreatorAndModifier()
        {
            this.BaseCreateTime = null;
            this.BaseCreatorId = null;
            this.BaseModifyTime = DateTimeHelper.Now;
            this.BaseModifierId = null;
        }

        #region create modify
        public virtual void Create()
        {
            this.SetupCreatorInfo();
            this.SetupModifyInfo();

        }

        public virtual void SetupCreatorInfo()
        {
            if (this.BaseIsDelete == null)
            {
                this.BaseIsDelete = 0;
            }

            if (this.Id == null|| this.Id==0)
            {
                this.Id = IdGeneratorHelper.Instance.GetId();
            }
            
            if (this.BaseCreateTime == null)
            {
                this.BaseCreateTime = DateTimeHelper.Now;
            }

            if (this.BaseCreatorId == null)
            {
                OperatorInfo user = Operator.Instance.CurrentInfo();
                if (user != null)
                {
                    this.BaseCreatorId = user.UserId;
                }
            }

            if (this.BaseCreatorId == null)
            {
                this.BaseCreatorId = 0;
            }
        }

        public virtual void Modify()
        {
            if (this.Id == null)
            {
                throw new BizException("数据Id为空，无法更新");
            }
            SetupModifyInfo();
        }

        public virtual void SetupModifyInfo()
        {
            //this.BaseVersion = 0;
            this.BaseModifyTime = DateTimeHelper.Now;

            if (this.BaseModifierId == null)
            {
                OperatorInfo user = Operator.Instance.CurrentInfo();
                if (user != null)
                {
                    this.BaseModifierId = user.UserId;
                }
            }

            if (this.BaseModifierId == null)
            {
                this.BaseModifierId = 0;
            }
        }
        #endregion
    }


    public class BaseField
    {
        public static string[] BaseFieldList = new string[]
        {
            "Id",
            "BaseIsDelete",
            "BaseCreateTime",
            "BaseModifyTime",
            "BaseCreatorId",
            "BaseModifierId",
            "BaseVersion",
           
        };
    }
}
