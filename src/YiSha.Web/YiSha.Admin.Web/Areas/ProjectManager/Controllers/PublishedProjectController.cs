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
using YiSha.Entity.ProjectManager;
using YiSha.Business.ProjectManager;
using YiSha.Model.Param.ProjectManager;
using YiSha.Service.TestCaseManager;
using YiSha.Model.Publishes;
using Newtonsoft.Json.Linq;
using Koo.Utilities.Data;
using Koo.Utilities.Helpers;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using YiSha.Business.ProductCategoryManager;
using YiSha.Model.Param.ProductCategoryManager;
using YiSha.Model.Result;

namespace YiSha.Admin.Web.Areas.ProjectManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-26 22:37
    /// 描 述：控制器类
    /// </summary>
    [Area("ProjectManager")]
    public class PublishedProjectController :  BaseController
    {
        private PublishedProjectBLL publishedProjectBLL = new PublishedProjectBLL();

        #region 视图功能
        [AuthorizeFilter("projectr:publishedproject:view")]
        public ActionResult PublishedProjectIndex()
        {
            return View();
        }

        public ActionResult PublishedProjectForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("projectr:publishedproject:search")]
        public async Task<ActionResult> GetListJson(PublishedProjectListParam param)
        {
            TData<List<PublishedProjectEntity>> obj = await publishedProjectBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("projectr:publishedproject:search")]
        public async Task<ActionResult> GetPageListJson(PublishedProjectListParam param, Pagination pagination)
        {
            TData<List<PublishedProjectEntity>> obj = await publishedProjectBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("projectr:publishedproject:search")]
        public async Task<ActionResult> GetTree()
        {
            var  obj = await publishedProjectBLL.GetPublishedProjectTree();
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<PublishedProjectEntity> obj = await publishedProjectBLL.GetEntity(id);
            return Json(obj);
        }
        public async Task<IActionResult> GetVersionList(string guid)
        {
          var ret =  await publishedProjectBLL.GetVersionList(guid);

            return Json(ret);
        }
        #endregion



        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("projectr:publishedproject:add,projectr:publishedproject:edit")]
        public async Task<ActionResult> SaveFormJson(PublishedProjectEntity entity)
        {
            TData<string> obj = await publishedProjectBLL.SaveForm(entity);
            return Json(obj);
        }


        #endregion

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("projectr:publishedproject:edit")]
        public async Task<ActionResult> DisableFormJson(string ids,int status)
        {
            TData obj = await publishedProjectBLL.DisableForm(ids,status);
            return Json(obj);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("projectr:publishedproject:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await publishedProjectBLL.DeleteForm(ids);
            return Json(obj);
        }

        #region 变量设置


        [HttpGet]
        public async Task<ActionResult> ProjectVarConfigForm(long projectId)
        {
            var varJsonService = new VarJsonService();
            var obj = await varJsonService.GetProjectVarJson(projectId);

            ViewBag.Info = obj.Result;

            return View();
        }


        [HttpGet]
        [AuthorizeFilter("projectr:publishedproject:search")]
        public async Task<ActionResult> GetVarFileTree(long projectId)
        {
            var service = new VarJsonService();

            var obj = await service.GetPublishedDocumentTree(projectId);
            var data = TData.CreateSuccessdValue(obj);

            return Json(data);
        }


        [HttpPost]
        [AuthorizeFilter("projectr:publishedproject:add,projectr:publishedproject:edit")]
        public async Task<ActionResult> SaveProjectVarForm([FromQuery] long projectId, List<SavingVarItem> savingItems)
        {
            var request = this.Request;

            //var savingItems = JsonHelper.ToObject<List<SavingVarItem>>(kv.Value, out string msg);

            var varJsonService = new VarJsonService();
            var obj = await varJsonService.SaveProjectVarJson(projectId, savingItems);

            return Json(obj);
        }

        #endregion

        #region 弹框选择用例模板
        //ChooseProductAndCate
        public ActionResult ChooseProject()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeFilter("projectr:publishedproject:search")]
        public async Task<ActionResult> GetChooseProjectListJson(PublishedProjectListParam param)
        {
            TData<List<PublishedProjectEntity>> obj = await publishedProjectBLL.GetList(param);
            return Json(obj);
        }
        #endregion
    }
}
