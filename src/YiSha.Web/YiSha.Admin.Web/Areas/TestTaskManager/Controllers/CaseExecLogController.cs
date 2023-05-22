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

namespace YiSha.Admin.Web.Areas.TestTaskManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-03-25 19:49
    /// 描 述：控制器类
    /// </summary>
    [Area("TestTaskManager")]
    public class CaseExecLogController :  BaseController
    {
        private CaseExecLogBLL caseExecLogBLL = new CaseExecLogBLL();

        #region 视图功能
        [AuthorizeFilter("testtaskr:caseexeclog:view")]
        public ActionResult CaseExecLogIndex()
        {
            return View();
        }

        public ActionResult CaseExecLogForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("testtaskr:caseexeclog:search")]
        public async Task<ActionResult> GetListJson(CaseExecLogListParam param)
        {
            TData<List<CaseExecLogEntity>> obj = await caseExecLogBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("testtaskr:caseexeclog:search")]
        public async Task<ActionResult> GetPageListJson(CaseExecLogListParam param, Pagination pagination)
        {
            TData<List<CaseExecLogEntity>> obj = await caseExecLogBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<CaseExecLogEntity> obj = await caseExecLogBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("testtaskr:caseexeclog:add,testtaskr:caseexeclog:edit")]
        public async Task<ActionResult> SaveFormJson(CaseExecLogEntity entity)
        {
            TData<string> obj = await caseExecLogBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("testtaskr:caseexeclog:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await caseExecLogBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
