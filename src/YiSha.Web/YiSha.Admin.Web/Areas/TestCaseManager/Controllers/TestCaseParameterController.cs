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
    /// 日 期：2022-10-01 20:14
    /// 描 述：控制器类
    /// </summary>
    [Area("TestCaseManager")]
    public class TestCaseParameterController :  BaseController
    {
        private TestCaseParameterBLL testCaseParameterBLL = new TestCaseParameterBLL();

        #region 视图功能
        [AuthorizeFilter("testcaser:testcaseparameter:view")]
        public ActionResult TestCaseParameterIndex()
        {
            return View();
        }

        public ActionResult TestCaseParameterForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("testcaser:testcaseparameter:search")]
        public async Task<ActionResult> GetListJson(TestCaseListParameterListParam param)
        {
            TData<List<TestCaseParameterEntity>> obj = await testCaseParameterBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("testcaser:testcaseparameter:search")]
        public async Task<ActionResult> GetPageListJson(TestCaseListParameterListParam param, Pagination pagination)
        {
            TData<List<TestCaseParameterEntity>> obj = await testCaseParameterBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<TestCaseParameterEntity> obj = await testCaseParameterBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("testcaser:testcaseparameter:add,testcaser:testcaseparameter:edit")]
        public async Task<ActionResult> SaveFormJson(TestCaseParameterEntity entity)
        {
            TData<string> obj = await testCaseParameterBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("testcaser:testcaseparameter:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await testCaseParameterBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
