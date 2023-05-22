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
using YiSha.Entity.TestCaseManager;
using YiSha.Business.TestCaseManager;
using YiSha.Model.Param.TestCaseManager;
using YiSha.Business.ProjectManager;
using YiSha.Service.TestCaseManager;
using Koo.Utilities.Data;
using Koo.Utilities.Helpers;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using YiSha.Model.Publishes;
using YiSha.Enum;
using YiSha.Service.TestTaskManager;

namespace YiSha.Admin.Web.Areas.TestCaseManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:13
    /// 描 述：控制器类
    /// </summary>
    [Area("TestCaseManager")]
    public class TestCaseController :  BaseController
    {
        private TestCaseBLL testCaseBLL = new TestCaseBLL();

        #region 首页列表功能
        [AuthorizeFilter("testcaser:testcase:view")]
        public ActionResult TestCaseIndex()
        {
            return View();
        }
        [HttpGet]
        [AuthorizeFilter("testcaser:testcase:search")]
        public async Task<ActionResult> GetPageListJson(TestCaseListParam param, Pagination pagination)
        {
            var obj = await testCaseBLL.GetPageList(param, pagination);
            return Json(obj);
        }


        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("testcaser:testcase:search")]
        public async Task<ActionResult> GetTree()
        {
            var obj = await testCaseBLL.GetTree();
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("testcaser:testcase:search")]
        public async Task<ActionResult> GetListJson(TestCaseListParam param)
        {
            TData<List<TestCaseEntity>> obj = await testCaseBLL.GetList(param);
            return Json(obj);
        }

     
        /// <summary>
        /// 下拉选择执行用例，用过添加测试任务中的明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("testcaser:testcase:search")]
        public async Task<IActionResult> GetCaseTreeListJson(long? envId)
        {
            var testCaseService = new TestCaseService();
            var obj = await testCaseService.GetAllListAsTree(envId, false, false);
            var ret = TData.CreateSuccessdValue(obj);
            return Json(ret);
        }
        #endregion

        #region 保存用例
        public ActionResult TestCaseForm()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetFormJson(long? id, long? productId, long? cateId)
        {
            var testCaseService = new TestCaseService();
            TData<TestCaseEntity> obj = new TData<TestCaseEntity>();
            obj.Result = await testCaseService.GetEntityForEdit(id, productId, cateId);
            if (obj.Result != null)
            {
                obj.Status = true;
            }

            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("testcaser:testcase:add,testcaser:testcase:edit")]
        public async Task<ActionResult> SaveFormJson(TestCaseEntity entity)
        {
            entity.UsingVersion = (byte)UsingVersionEnumType.Special;
            TData<string> obj = await testCaseBLL.SaveForm(entity);
            return Json(obj);
        }
        #endregion

        #region 删除数据
        [HttpPost]
        [AuthorizeFilter("testcaser:testcase:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await testCaseBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion


        #region 配置变量
        [HttpGet]
        public async Task<ActionResult> TestCaseVarConfigForm(long testCaseId)
        {
            var varJsonService = new VarJsonService();
            var obj = await varJsonService.GetTestCaseVarJsonByTestCaseId(testCaseId, false);

            ViewBag.Info = obj.Result;

            return View();
        }

        [HttpGet]
        [AuthorizeFilter("testcaser:testcase:add,testcaser:testcase:edit")]
        public async Task<ActionResult> GetVarFileTree(long testCaseId)
        {
            var service = new VarJsonService();

            var obj = await service.GetPublishedDcoumentTreeByTestCaseId(testCaseId);
            var data = TData.CreateSuccessdValue(obj);

            return Json(data);
        }

     
        [HttpPost]
        
        public async Task<ActionResult> SaveTestCaseVarForm([FromQuery] long testCaseId, List<SavingVarItem> savingItems)
        {
            var request = this.Request;

            //var savingItems = JsonHelper.ToObject<List<SavingVarItem>>(kv.Value, out string msg);

            var varJsonService = new VarJsonService();
           var obj = await varJsonService.SaveTestCaseVarJsonByTestCaseId(testCaseId, savingItems);

            return Json(obj);
        }
        #endregion

        /// <summary>
        /// 升级用例的版本
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="projectVersion"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("testcaser:testcase:edit")]
        public async Task<ActionResult> UpgradeVersionJson(string ids,string projectVersion)
        {
            var service = new TestCaseService();
            TData obj = new TData();
            await service.UpgradeVersionJson(ids,projectVersion);
            obj.Status = true;
            return Json(obj);
        }
    }
}
