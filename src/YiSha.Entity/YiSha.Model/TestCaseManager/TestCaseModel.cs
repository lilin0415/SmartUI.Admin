using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;
using YiSha.Entity.TestCaseManager;

namespace YiSha.Model.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:13
    /// 描 述：实体类
    /// </summary>
    public partial class TestCaseModel : TestCaseEntity
    {
        public TestCaseModel()
         {

         }

        public string EnvDisplayName
        {
            get; set;
        }
        public string UsingVersionDisplayName
        {
            get; set;
        }
        //public string ProjectName
        //{
        //    get; set;
        //}
    }
}
