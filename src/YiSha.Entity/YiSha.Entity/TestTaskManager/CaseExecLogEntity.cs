using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using Koo.Utilities.Helpers;

namespace YiSha.Entity.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-03-25 19:49
    /// 描 述：实体类
    /// </summary>
    public partial class CaseExecLogEntity : BaseCreatableEntity, ICreatableEntity
    {
        public CaseExecLogEntity()
         {

         }
    }

    public enum TransStep
    {
        /// <summary>
        /// 非执行器日志
        /// </summary>
        None,
        /// <summary>
        /// 执行器开始日志
        /// </summary>
        Start,
        /// <summary>
        /// 执行器运行中日志
        /// </summary>
        Normal,
        /// <summary>
        /// 执行器结束日志
        /// </summary>
        End,
    }
}
