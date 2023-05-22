using Koo.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Util.Model
{
    /// <summary>
    /// 数据传输对象
    /// </summary>
    public class TData
    {
        private bool _status;
        /// <summary>
        /// 操作结果，Tag为1代表成功，0代表失败，其他的验证返回结果，可根据需要设置
        /// </summary>
        public bool Status
        {
            get
            {
                return _status;
            }
            set
            {
                this._status = value;
                if (this.StatusCode == null || this.StatusCode == 0 || this.StatusCode == 1)
                {
                    this.StatusCode = value ? 1 : 0;
                }
            }
        }
        
        public int? StatusCode
        {
            get; set;
        } = 0;

        /// <summary>
        /// 提示信息或异常信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 扩展Message
        /// </summary>
        public string Description { get; set; }

        public virtual object Result
        {
            get;set;
        }
        public static TData<T> CreateSuccessdValue<T>(T val,string msg="")
        {
            TData<T> ret = new TData<T>(val);
            ret.Status = true;
            return ret;
        }
        public static TData CreateSuccessdMsg(string msg, object val=null)
        {
            TData ret = new TData();
            ret.Status = true;
            ret.Message = msg;
            ret.Result = val;
            return ret;
        }
        public static TData<T> CreateSuccessdMsg<T>(string msg, T val = default(T))
        {
            TData<T> ret = new TData<T>();
            ret.Status = true;
            ret.Message = msg;
            ret.Result = val;
            return ret;
        }
        #region 生成错误的结果
        public static TData<T> CreateFailedValue<T>(T val,string msg="")
        {
            TData<T> ret = new TData<T>(val);
            ret.Status = false;
            ret.Message = msg;
            return ret;
        }
        public static TData<T> CreateFailedMsg<T>(string msg, T val = default(T))
        {
            TData<T> ret = new TData<T>();
            ret.Status = false;
            ret.Message = msg;
            ret.Result = val;
            return ret;
        }
        public static TData<T> CreateFailedMsg<T>(Exception ex, T val = default(T))
        {
            TData<T> ret = new TData<T>();
            ret.Status = false;
            if (ex is BizException biz)
            {
                ret.Message = biz.Message;
            }
            else
            {
                ret.Message = ex.Message;
            }
            ret.Result = val;
            return ret;
        }
        public static TData CreateFailedMsg(string msg, object val = null)
        {
            TData ret = new TData();
            ret.Status = false;
            ret.Message = msg;
            ret.Result = val;
            return ret;
        }
        public static TData CreateFailedMsg(Exception ex, object val = null)
        {
            TData ret = new TData();
            ret.Status = false;
            ret.Message = ex.ToString();
            ret.Result = val;
            return ret;
        }
        #endregion
    }

    public class TData<T> : TData
    {
        public TData()
        {

        }
        public TData(T result)
        {
            this.Result = result;
        }

        /// <summary>
        /// 列表的记录数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public  new T Result
        {
            get
            {
                return (T)base.Result;
            }
            set
            {
                base.Result = value;
            }
        }
    }
}
