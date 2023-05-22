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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestTaskController : BaseApiController
    {
        private ModuleCategoryBLL moduleCategoryBLL = new ModuleCategoryBLL();


        #region 获取任务及任务明细数据
        //[HttpGet]
        ////[AuthorizeFilter("productcategoryr:modulecategory:search")]
        //public async Task<TData<List<TestTaskModel>>> GetList([FromQuery] ModuleCategoryListParam param)
        //{
        //    var testTaskService = new TestTaskService();
        //    var list = await testTaskService.GetListByClient(new Model.Param.TestTaskManager.TestTaskListParam());

        //    return list;
        //}

        //[HttpGet]
        ////[AuthorizeFilter("productcategoryr:modulecategory:search")]
        //public async Task<TData<TestTaskItemDetailModel>> GetTaskItemDetailByTaskId([FromQuery] long taskId)
        //{
        //    var testTaskService = new TestTaskService();
        //    var list = await testTaskService.GetTaskItemDetailByTaskId(taskId);

        //    return TData.CreateSuccessd(list);
        //}

        #endregion

        /// <summary>
        /// 获取测试用例配置的变量
        /// </summary>
        /// <param name="testCaseId"></param>
        /// <returns></returns>
        [HttpGet]
        //[AuthorizeFilter("productcategoryr:modulecategory:search")]
        public async Task<TData<PublishedInfo>> GetPublishedVarByTestCaseId([FromQuery] long testCaseId)
        {
            var varJsonService = new VarJsonService();
            var obj = await varJsonService.GetTestCaseVarJsonByTestCaseId(testCaseId, false);

            return obj;

        }

        #region 拉取作业、作业中包含的用例
        /// <summary>
        /// 拉取待消费的作业
        /// </summary>
        
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter]
        //[AuthorizeFilter("productcategoryr:modulecategory:search")]
        public async Task<TData<List<TaskExecRecordEntity>>> PullTasks(int size)
        {
            var service = new TaskExecRecordService();
            try
            {
                var ret = await service.PullTasks(this.CurrentUser, this.AppToken, this.AppVersion, size);


                return TData.CreateSuccessd(ret);
            }
            catch (Exception ex)
            {
                return TData.CreateFailed<List<TaskExecRecordEntity>>(null, ex.Message);
            }
        }
        /// <summary>
        /// 拉取作业中包含的用例
        /// </summary>
        /// <param name="taskExecId"></param>
        /// <returns></returns>
        [HttpGet]
        //[AuthorizeFilter("productcategoryr:modulecategory:search")]
        public async Task<TData<List<CaseExecRecordModel>>> PullCaseExecListByTaskExecId([FromQuery] long taskExecId)
        {
            var service = new CaseExecRecordService();
            var list = await service.GetListByTaskExecId(taskExecId);
         
            return TData.CreateSuccessd(list);
        }
        #endregion

        #region 更改任务、用例 执行状态，结束状态
        /// <summary>
        /// 更改任务状态
        /// </summary>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        [HttpPost]
        //[AuthorizeFilter("productcategoryr:modulecategory:search")]
        public async Task<TData<bool>> UpdateTaskStatus(UpdateTaskExecStatusBody taskStatus)
        {
            var service = new TaskExecRecordService();
            var ret = await service.UpdateTaskStatus(taskStatus);

            return TData.CreateSuccessd(ret);
        }

        /// <summary>
        /// 更改用例状态
        /// </summary>
        /// <param name="caseStatus"></param>
        /// <returns></returns>
        [HttpPost]
        //[AuthorizeFilter("productcategoryr:modulecategory:search")]
        public async Task<TData<bool>> UpdateCaseStatus(UpdateCaseExecStatusBody caseStatus)
        {
            var service = new CaseExecRecordService();
            var ret = await service.UpdateCaseExecStatus(caseStatus);

            return TData.CreateSuccessd(ret);
        }
        #endregion
    }
}
