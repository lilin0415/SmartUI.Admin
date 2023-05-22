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
    /// 日 期：2022-10-12 21:21
    /// 描 述：控制器类
    /// </summary>
    [Area("TestTaskManager")]
    public class DeployTaskController :  BaseController
    {
        private DeployTaskBLL deployTaskBLL = new DeployTaskBLL();

        #region 视图功能
        [AuthorizeFilter("testtaskr:deploytask:view")]
        public ActionResult DeployTaskIndex()
        {
            return View();
        }

        public ActionResult DeployTaskForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("testtaskr:deploytask:search")]
        public async Task<ActionResult> GetListJson(DeployTaskListParam param)
        {
            TData<List<DeployTaskEntity>> obj = await deployTaskBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("testtaskr:deploytask:search")]
        public async Task<ActionResult> GetPageListJson(DeployTaskListParam param, Pagination pagination)
        {
            TData<List<DeployTaskEntity>> obj = await deployTaskBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<DeployTaskEntity> obj = await deployTaskBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("testtaskr:deploytask:add,testtaskr:deploytask:edit")]
        public async Task<ActionResult> SaveFormJson(DeployTaskEntity entity)
        {
            TData<string> obj = await deployTaskBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("testtaskr:deploytask:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await deployTaskBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
