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
    /// 日 期：2022-09-23 23:04
    /// 描 述：控制器类
    /// </summary>
    [Area("ProductCategoryManager")]
    public class ProductController :  BaseController
    {
        private ProductBLL productBLL = new ProductBLL();

        #region 视图功能
        [AuthorizeFilter("productcategoryr:product:view")]
        public ActionResult ProductIndex()
        {
            return View();
        }

        public ActionResult ProductForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("productcategoryr:product:search")]
        public async Task<ActionResult> GetListJson(ProductListParam param)
        {
            TData<List<ProductEntity>> obj = await productBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("productcategoryr:product:search")]
        public async Task<ActionResult> GetPageListJson(ProductListParam param, Pagination pagination)
        {
            TData<List<ProductEntity>> obj = await productBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<ProductEntity> obj = await productBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("productcategoryr:product:view,productcategoryr:product:search")]
        public async Task<IActionResult> GetProductTreeListJson(ProductListParam param)
        {
            TData<List<ZtreeInfo>> obj = await productBLL.GetZtreeList(param);
            return Json(obj);
        }

        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("productcategoryr:product:add,productcategoryr:product:edit")]
        public async Task<ActionResult> SaveFormJson(ProductEntity entity)
        {
            TData<string> obj = await productBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("productcategoryr:product:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await productBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
