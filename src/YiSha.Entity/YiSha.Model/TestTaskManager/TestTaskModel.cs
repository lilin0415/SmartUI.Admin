using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using YiSha.Entity.TestTaskManager;

namespace YiSha.Model.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-02 16:20
    /// 描 述：实体类
    /// </summary>
    public partial class TestTaskModel : TestTaskEntity
    {
        public TestTaskModel()
        {

        }

        /// <summary>
        /// 执行环境
        /// </summary>
        public string EnvDisplayName{get;set;}
    }
}
