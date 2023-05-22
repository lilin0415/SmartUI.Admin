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
using YiSha.Admin.Web.Controllers;
using YiSha.Entity.TestTaskManager;
using YiSha.Business.TestTaskManager;
using YiSha.Model.Param.TestTaskManager;
using YiSha.Model.TestTaskManager;
using YiSha.Business.TestCaseManager;
using YiSha.Model.TestCaseManager;
using YiSha.Model.Param.TestCaseManager;
using YiSha.Service.TestCaseManager;
using YiSha.Service.TestTaskManager;
using YiSha.Model.Publishes;

namespace YiSha.Admin.Web.Areas.TestTaskManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-02 16:21
    /// 描 述：控制器类
    /// </summary>
    [Area("TestTaskManager")]
    public class TestTaskItemController :  BaseController
    {
        private TestTaskItemBLL testTaskItemBLL = new TestTaskItemBLL();


        #region 计划任务中显示的用例明细页面
        [AuthorizeFilter("testtaskr:testtaskitem:view")]
        public ActionResult TestTaskItemIndex()
        {
            return View();
        }
        [HttpGet]
        [AuthorizeFilter("testtaskr:testtaskitem:search")]
        public async Task<IActionResult> GetPageListJson(long? taskId, TestTaskItemListParam param, Pagination pagination)
        {
            var testCaseSceneBLL = new TestTaskService();

            var param1 = new TestCaseListParam();
            var obj = await testCaseSceneBLL.GetPageListByTaskId((taskId.HasValue ? taskId.Value : (long)0));

            var ret = TData.CreateSuccessdValue(obj);
            return Json(ret);

            //TData<List<TestTaskItemModel>> obj = await testTaskItemBLL.GetPageList(param, pagination);
            //return Json(obj);
        }
        #endregion

        #region 视图功能
        public ActionResult TestTaskItemForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("testtaskr:testtaskitem:search")]
        public async Task<ActionResult> GetListJson(TestTaskItemListParam param)
        {
            TData<List<TestTaskItemEntity>> obj = await testTaskItemBLL.GetList(param);
            return Json(obj);
        }



        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<TestTaskItemEntity> obj = await testTaskItemBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("testtaskr:testtaskitem:add,testtaskr:testtaskitem:edit")]
        public async Task<ActionResult> SaveFormJson(TestTaskItemEntity entity)
        {
            TData<string> obj = await testTaskItemBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("testtaskr:testtaskitem:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await testTaskItemBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion


        #region 选择用例
        [AuthorizeFilter("testcaser:testcase:view")]
        public ActionResult SelectTestCaseIndex()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveCaseListForm([FromQuery] long taskId, TestCaseModel[] caseList)
        {
            var service = new TestTaskItemService();
            await service.AddCaseList(taskId,caseList);

            return Json(TData.CreateSuccessdValue(1));

        }

        #endregion


        [HttpPost]

        public async Task<ActionResult> UpdateSortNums(List<SavingTestTaskItem> savingItems)
        {
            var request = this.Request;

            //var savingItems = JsonHelper.ToObject<List<SavingVarItem>>(kv.Value, out string msg);

            var service = new TestTaskItemService();
            await service.UpdateSortNums(savingItems);

            return Json(TData.CreateSuccessdMsg(""));
        }
    }
}
