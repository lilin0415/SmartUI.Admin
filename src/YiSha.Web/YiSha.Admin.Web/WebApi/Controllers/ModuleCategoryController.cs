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
using YiSha.Entity.ProductCategoryManager;
using YiSha.Business.ProductCategoryManager;
using YiSha.Model.Param.ProductCategoryManager;
using YiSha.Business.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.Result;
using YiSha.Admin.Web.WebApi.Controllers;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-24 14:57
    /// 描 述：控制器类
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ModuleCategoryController : BaseApiController
    {
        private ModuleCategoryBLL moduleCategoryBLL = new ModuleCategoryBLL();


        #region 获取数据
        [HttpGet]
        [AuthorizeApiFilter("productcategoryr:modulecategory:search")]
        public async Task<TData<List<ModuleCategoryEntity>>> GetList([FromQuery] ModuleCategoryListParam param)
        {
            TData<List<ModuleCategoryEntity>> obj = await moduleCategoryBLL.GetList(param);
            return obj;
        }

        [HttpGet]
        [AuthorizeApiFilter("productcategoryr:modulecategory:search")]
        public async Task<TData<List<ModuleCategoryEntity>>> GetListJson([FromQuery] ModuleCategoryListParam param)
        {
            TData<List<ModuleCategoryEntity>> obj = await moduleCategoryBLL.GetList(param);
            return obj;
        }

        [HttpGet]
        [AuthorizeApiFilter("productcategoryr:modulecategory:search")]
        public async Task<TData<List<ModuleCategoryEntity>>> GetPageListJson([FromQuery] ModuleCategoryListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<ModuleCategoryEntity>> obj = await moduleCategoryBLL.GetPageList(param, pagination);
            return obj;
        }
      
        [HttpGet]
        public async Task<TData<ModuleCategoryEntity>> GetFormJson(long id)
        {
            TData<ModuleCategoryEntity> obj = await moduleCategoryBLL.GetEntity(id);
            return obj;
        }

        [HttpGet]
        [AuthorizeApiFilter("productcategoryr:modulecategory:search")]
        public async Task<TData<List<ZtreeInfo>>> GetModuleCategoryTreeListJson(long? id,[FromQuery] ModuleCategoryListParam param)
        {
            
            TData<List<ZtreeInfo>> obj = await moduleCategoryBLL.GetZtreeList(param,id);
            return obj;
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeApiFilter("productcategoryr:modulecategory:add,productcategoryr:modulecategory:edit")]
        public async Task<TData<string>> SaveFormJson(ModuleCategoryEntity entity)
        {
            TData<string> obj = await moduleCategoryBLL.SaveForm(entity);
            return obj;
        }

        [HttpPost]
        [AuthorizeApiFilter("productcategoryr:modulecategory:delete")]
        public async Task<TData> DeleteFormJson(string ids)
        {
            TData obj = await moduleCategoryBLL.DeleteForm(ids);
            return obj;
        }
        #endregion
    }
}
