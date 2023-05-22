using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Entity.ProductCategoryManager;
using YiSha.Business.ProductCategoryManager;
using YiSha.Model.Param.ProductCategoryManager;
using YiSha.Business.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.Result;
using YiSha.Admin.Web.WebApi.Controllers;
using YiSha.Service.TestTaskManager;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.TestTaskManager;
using YiSha.Service.TestCaseManager;
using YiSha.Model.Publishes;
using YiSha.Model.WebApis;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-24 14:57
    /// 描 述：控制器类
    /// </summary>
 
    public class TestTaskController : AuthorizedBaseApiController
    {
        private ModuleCategoryBLL moduleCategoryBLL = new ModuleCategoryBLL();



        /// <summary>
        /// 获取测试用例配置的变量
        /// </summary>
        /// <param name="testCaseId"></param>
        /// <returns></returns>
        [HttpGet]
  
        public async Task<TData<PublishedInfo>> GetPublishedVarByTestCaseId([FromQuery] long testCaseId)
        {
            var varJsonService = new VarJsonService();
            var obj = await varJsonService.GetTestCaseVarJsonByTestCaseId(testCaseId, false);

            return obj;

        }


        #region 更改任务、用例 执行状态，结束状态
        /// <summary>
        /// 更改任务状态
        /// </summary>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData<bool>> UpdateTaskStatus(UpdateTaskExecStatusBody taskStatus)
        {
            try
            {
                throw new NotImplementedException();
                //var service = new TaskExecRecordService();
                //var ret = await service.UpdateTaskStatus(taskStatus);

                //return TData.CreateSuccessdValue(ret);
            }
            catch (Exception ex)
            {
                return TData.CreateFailedValue(false, ex.Message);
            }
        }

        /// <summary>
        /// 更改用例状态
        /// </summary>
        /// <param name="caseStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData<bool>> UpdateCaseStatus(UpdateCaseExecStatusBody caseStatus)
        {
            var service = new CaseExecRecordService();
            var ret = await service.UpdateCaseExecStatus(caseStatus);

            return TData.CreateSuccessdValue(ret);
        }
        #endregion
    }
}
