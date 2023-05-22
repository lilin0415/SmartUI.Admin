using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Enum.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Model.Result;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class SystemMenuController : BaseController
    {
        private SystemMenuBLL menuBLL = new SystemMenuBLL();

        private void SetMenuTypeJsonData()
        {
            OperatorInfo operatorInfo = this.GetCurrentInfo();
            var menuType = typeof(MenuTypeEnum);

            List<KeyValuePair<int, string>> dictionaryList = menuType.EnumToDictionary().ToList();
            var sJson = JsonConvert.SerializeObject(dictionaryList);
            ViewBag.MenuTypeJsonData = sJson;
        }

        #region 菜单首页树列表
        [AuthorizeFilter("system:systemmenu:view")]
        public async Task<IActionResult> MenuIndex()
        {
            SetMenuTypeJsonData();
            return View();
        }
        /// <summary>
        /// 菜单编辑主页面的树数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:systemmenu:search,system:role:search")]
        public async Task<IActionResult> GetListJson(MenuListParam param)
        {
            TData<List<MenuEntity>> obj = await menuBLL.GetSystemMenuList();
            return Json(obj);
        }

        #endregion

        public IActionResult MenuIcon()
        {
            return View();
        }
        #region 菜单编辑，选择父结点页面
        public IActionResult MenuChoose()
        {
            return View();
        }
        /// <summary>
        /// 菜单编辑页面，父级节点显示的菜单树
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:systemmenu:edit")]
        public async Task<IActionResult> GetMenuTreeListJson(MenuListParam param)
        {
            TData<List<ZtreeInfo>> obj = await menuBLL.GetMenuTreeList(param);
            return Json(obj);
        }
        #endregion



        #region 菜单编辑、保存

        public async Task<IActionResult> MenuForm()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            ViewBag.OperatorInfo = operatorInfo;
            SetMenuTypeJsonData();

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson(long parentId = 0)
        {
            var obj = await menuBLL.GetMaxSort(parentId);
            return Json(TData.CreateSuccessdValue(obj));
        }
        [HttpGet]
        [AuthorizeFilter("system:systemmenu:view")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<MenuEntity> obj = await menuBLL.GetEntity(id);

            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("system:systemmenu:add,system:systemmenu:edit")]
        public async Task<IActionResult> SaveFormJson(MenuEntity entity)
        {
            TData<string> obj = await menuBLL.SaveForm(entity);
            return Json(obj);
        }
        #endregion

        #region 删除
        [HttpPost]
        [AuthorizeFilter("system:systemmenu:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await menuBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}