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
using YiSha.Entity.ProductCategoryManager;
using YiSha.Business.ProductCategoryManager;
using YiSha.Model.Param.ProductCategoryManager;
using YiSha.Business.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.Result;

namespace YiSha.Admin.Web.Areas.ProductCategoryManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-24 14:57
    /// 描 述：控制器类
    /// </summary>
    [Area("ProductCategoryManager")]
    public class ModuleCategoryController :  BaseController
    {
        private ModuleCategoryBLL moduleCategoryBLL = new ModuleCategoryBLL();

        #region 视图功能
        [AuthorizeFilter("productcategoryr:modulecategory:view")]
        public ActionResult ModuleCategoryIndex()
        {
            return View();
        }

        public ActionResult ModuleCategoryForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("productcategoryr:modulecategory:search")]
        public async Task<ActionResult> GetListJson(ModuleCategoryListParam param)
        {
            TData<List<ModuleCategoryEntity>> obj = await moduleCategoryBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("productcategoryr:modulecategory:search")]
        public async Task<ActionResult> GetPageListJson(ModuleCategoryListParam param, Pagination pagination)
        {
            TData<List<ModuleCategoryEntity>> obj = await moduleCategoryBLL.GetPageList(param, pagination);
            return Json(obj);
        }
      
        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<ModuleCategoryEntity> obj = await moduleCategoryBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("productcategoryr:modulecategory:search")]
        public async Task<IActionResult> GetModuleCategoryTreeListJson(long? id, long? productId)
        {
            var param = new ModuleCategoryListParam();
            param.ProductId = productId;

            TData<List<ZtreeInfo>> obj = await moduleCategoryBLL.GetZtreeList(param,id);
            return Json(obj);
        }
        [HttpGet]
        [AuthorizeFilter("productcategoryr:modulecategory:search")]
        public async Task<IActionResult> GetAllModuleCategoryTreeListJson( ModuleCategoryListParam param)
        {

            TData<List<ZtreeInfo>> obj = await moduleCategoryBLL.GetProductAndCateTree(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("productcategoryr:modulecategory:search")]
        public async Task<IActionResult> GetPublishedProjectTreeListJson(ModuleCategoryListParam param)
        {

            TData<List<ZtreeInfo>> obj = await moduleCategoryBLL.GetProductAndCateTree(param);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("productcategoryr:modulecategory:add,productcategoryr:modulecategory:edit")]
        public async Task<ActionResult> SaveFormJson(ModuleCategoryEntity entity)
        {
            TData<string> obj = await moduleCategoryBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("productcategoryr:modulecategory:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await moduleCategoryBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion

        #region 获取产品、模块分类树
        //ChooseProductAndCate
        public ActionResult ChooseProductAndCate()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeFilter("productcategoryr:modulecategory:search")]
        public async Task<IActionResult> GetChooseProductAndCateTreeJson(ModuleCategoryListParam param)
        {

            TData<List<ZtreeInfo>> obj = await moduleCategoryBLL.GetProductAndCateTree(param);
            return Json(obj);
        }
        #endregion
    }
}
