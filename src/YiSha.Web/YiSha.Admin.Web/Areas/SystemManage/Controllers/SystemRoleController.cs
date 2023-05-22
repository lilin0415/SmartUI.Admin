using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Model;
using YiSha.Model.Param.SystemManage;
using YiSha.Model.Result;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class SystemRoleController : BaseController
    {
        private RoleBLL roleBLL = new RoleBLL();

        /// <summary>
        /// 在用户编辑的时候，需要显示相应的角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:systemrole:search,organization:user:search,organization:user:view,organization:user:edit,tenant:mytenant:detail")]
        public async Task<IActionResult> GetListJson(RoleListParam param)
        {
            TData<List<RoleEntity>> obj = await roleBLL.GetList(param, true);
            return Json(obj);
        }


        #region 获取数据


        [HttpGet]
        [AuthorizeFilter("system:systemrole:view")]
        public async Task<IActionResult> GetRoleName(RoleListParam param)
        {
            TData<string> obj = new TData<string>();
            var list = await roleBLL.GetList(param,true);
            if (list.Status)
            {
                obj.Result = string.Join(",", list.Result.Select(p => p.RoleName));
                obj.Status = true;
            }
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            TData<int> obj = await roleBLL.GetMaxSort();
            return Json(obj);
        }
        #endregion


        #region 查询系统角色列表
        [AuthorizeFilter("system:systemrole:view")]
        public IActionResult RoleIndex()
        {
            return View();
        }
        [HttpGet]
        [AuthorizeFilter("system:systemrole:search,organization:user:search")]
        public async Task<IActionResult> GetPageListJson(RoleListParam param, Pagination pagination)
        {
            TData<List<RoleModel>> obj = await roleBLL.GetPageList(param, pagination, true);
            return Json(obj);
        }

        #endregion


        #region 查询、修改系统角色数据
        [AuthorizeFilter("system:systemrole:view")]
        public IActionResult RoleForm()
        {
            return View();
        }
        [HttpGet]
        [AuthorizeFilter("system:systemrole:view")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<RoleEntity> obj = await roleBLL.GetEntity(id);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("system:systemrole:add,system:systemrole:edit")]
        public async Task<IActionResult> SaveFormJson(RoleEntity entity)
        {
            TData<string> obj = await roleBLL.SaveForm(entity, true);
            return Json(obj);
        }
        #endregion

        #region 删除角色

        [HttpPost]
        [AuthorizeFilter("system:systemrole:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await roleBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion

        /// <summary>
        /// 菜单树，用于角色编辑页面显示
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:systemrole:search")]
        public async Task<IActionResult> GetMenuTreeListJson(MenuListParam param)
        {
            var menuBLL = new MenuBLL();
            TData<List<ZtreeInfo>> obj = await menuBLL.GetRoleMenuTreeList(param);
            return Json(obj);
        }

    }
}