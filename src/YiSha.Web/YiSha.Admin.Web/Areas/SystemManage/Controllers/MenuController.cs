using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Koo.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Enum.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Model.Result;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class MenuController : BaseController
    {
        private MenuBLL menuBLL = new MenuBLL();

        private void SetMenuTypeJsonData()
        {
            OperatorInfo operatorInfo = this.GetCurrentInfo();
            var menuType = typeof(MenuTypeEnum);

            List<KeyValuePair<int, string>> dictionaryList = menuType.EnumToDictionary().ToList();
            var sJson = string.Empty;
            if (operatorInfo.HasManagerPower)
            {
                sJson = JsonConvert.SerializeObject(dictionaryList);
            }
            else
            {
                
                sJson = JsonConvert.SerializeObject(dictionaryList.Where(x => x.Key != (int)MenuTypeEnum.Button).ToList());
            }

            ViewBag.MenuTypeJsonData = sJson;
        }
        #region 视图功能
        [AuthorizeFilter("system:menu:view")]
        public async Task<IActionResult> MenuIndex()
        {
            SetMenuTypeJsonData();
            return View();
        }

        public async Task<IActionResult> MenuForm()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            ViewBag.OperatorInfo = operatorInfo;
            SetMenuTypeJsonData();

            return View();
        }
       
        public IActionResult MenuIcon()
        {
            return View();
        }
        #endregion


        /// <summary>
        /// 菜单编辑主页面的树数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:menu:search,system:role:search")]
        public async Task<IActionResult> GetListJson(MenuListParam param)
        {
            TData<List<MenuEntity>> obj = await menuBLL.GetMenuListForEdit();
            return Json(obj);
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
        [AuthorizeFilter("system:menu:edit")]
        public async Task<IActionResult> GetMenuChooseTreeListJson(MenuListParam param)
        {
            TData<List<ZtreeInfo>> obj = await menuBLL.GetMenuChooseTreeListJson(param);
            return Json(obj);
        }
        #endregion



        #region 编辑菜单
        [HttpGet]
        [AuthorizeFilter("system:menu:view")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<MenuEntity> obj = await menuBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson(long parentId = 0)
        {
            TData<int> obj = await menuBLL.GetMaxSort(parentId);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("system:menu:add,system:menu:edit")]
        public async Task<IActionResult> SaveFormJson(MenuEntity entity)
        {
            TData<string> obj = await menuBLL.SaveForm(entity);
            return Json(obj);
        }

        #endregion

        #region 删除菜单

        [HttpPost]
        [AuthorizeFilter("system:menu:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            
            TData obj = await menuBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}