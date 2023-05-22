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
    /// 日 期：2022-10-01 20:55
    /// 描 述：控制器类
    /// </summary>
    [Area("TestCaseManager")]
    public class ExecEnvironmentController :  BaseController
    {
        private ExecEnvironmentBLL execEnvironmentBLL = new ExecEnvironmentBLL();

        #region 视图功能
        [AuthorizeFilter("testcaser:execenvironment:view")]
        public ActionResult ExecEnvironmentIndex()
        {
            return View();
        }


        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("testcaser:execenvironment:search")]
        public async Task<ActionResult> GetListJson(ExecEnvironmentListParam param)
        {
            TData<List<ExecEnvironmentEntity>> obj = await execEnvironmentBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("testcaser:execenvironment:search")]
        public async Task<ActionResult> GetPageListJson(ExecEnvironmentListParam param, Pagination pagination)
        {
            TData<List<ExecEnvironmentEntity>> obj = await execEnvironmentBLL.GetPageList(param, pagination);
            return Json(obj);
        }


        #endregion

        #region 保存数据
        public ActionResult ExecEnvironmentForm()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<ExecEnvironmentEntity> obj = await execEnvironmentBLL.GetEntity(id);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("testcaser:execenvironment:add,testcaser:execenvironment:edit")]
        public async Task<ActionResult> SaveFormJson(ExecEnvironmentEntity entity)
        {
            TData<string> obj = await execEnvironmentBLL.SaveForm(entity);
            return Json(obj);
        }


        #endregion

        #region 删除
        [HttpPost]
        [AuthorizeFilter("testcaser:execenvironment:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await execEnvironmentBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
