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

namespace YiSha.Admin.Web.Areas.TestCaseManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-04 16:17
    /// 描 述：控制器类
    /// </summary>
    [Area("TestCaseManager")]
    public class TestCaseGroupController :  BaseController
    {
        private TestCaseGroupBLL testCaseGroupBLL = new TestCaseGroupBLL();

        #region 视图功能
        [AuthorizeFilter("testcaser:testcasegroup:view")]
        public ActionResult TestCaseGroupIndex()
        {
            return View();
        }

        public ActionResult TestCaseGroupForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("testcaser:testcasegroup:search")]
        public async Task<ActionResult> GetListJson(TestCaseGroupListParam param)
        {
            TData<List<TestCaseGroupEntity>> obj = await testCaseGroupBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("testcaser:testcasegroup:search")]
        public async Task<ActionResult> GetPageListJson(TestCaseGroupListParam param, Pagination pagination)
        {
            TData<List<TestCaseGroupEntity>> obj = await testCaseGroupBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<TestCaseGroupEntity> obj = await testCaseGroupBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("testcaser:testcasegroup:add,testcaser:testcasegroup:edit")]
        public async Task<ActionResult> SaveFormJson(TestCaseGroupEntity entity)
        {
            TData<string> obj = await testCaseGroupBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("testcaser:testcasegroup:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await testCaseGroupBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
