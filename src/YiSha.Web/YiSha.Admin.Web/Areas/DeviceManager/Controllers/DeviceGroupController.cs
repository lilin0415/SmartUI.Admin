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
using YiSha.Entity.DeviceManager;
using YiSha.Business.DeviceManager;
using YiSha.Model.Param.DeviceManager;
using Koo.Utilities.Exceptions;
using YiSha.Entity.ProductCategoryManager;
using YiSha.Service.ProductCategoryManager;

namespace YiSha.Admin.Web.Areas.DeviceManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-04-22 09:41
    /// 描 述：控制器类
    /// </summary>
    [Area("DeviceManager")]
    public class DeviceGroupController :  BaseController
    {
        private DeviceGroupBLL deviceGroupBLL = new DeviceGroupBLL();

        #region 视图功能
        [AuthorizeFilter("devicer:devicegroup:view")]
        public ActionResult DeviceGroupIndex()
        {
            return View();
        }

        public ActionResult DeviceGroupForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("devicer:devicegroup:search")]
        public async Task<ActionResult> GetListJson(DeviceGroupListParam param)
        {
            TData<List<DeviceGroupEntity>> obj = await deviceGroupBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("devicer:devicegroup:search")]
        public async Task<ActionResult> GetPageListJson(DeviceGroupListParam param, Pagination pagination)
        {
            TData<List<DeviceGroupEntity>> obj = await deviceGroupBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<DeviceGroupEntity> obj = await deviceGroupBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("devicer:devicegroup:add,devicer:devicegroup:edit")]
        public async Task<ActionResult> SaveFormJson(DeviceGroupEntity entity)
        {
            TData<string> obj = await deviceGroupBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("devicer:devicegroup:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
          
            TData obj = await deviceGroupBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
