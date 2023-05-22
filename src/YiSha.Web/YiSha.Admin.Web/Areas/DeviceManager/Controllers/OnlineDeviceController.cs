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
using YiSha.Model.Param.TestTaskManager;
using YiSha.Model.DeviceManager;
using YiSha.Business.DeviceManager;
using YiSha.Entity.DeviceManager;

namespace YiSha.Admin.Web.Areas.DeviceManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-08 22:14
    /// 描 述：控制器类
    /// </summary>
    [Area("DeviceManager")]
    public class OnlineDeviceController :  BaseController
    {
        private DeviceBLL tenantDeviceBLL = new DeviceBLL();

        #region 首页列表
        [AuthorizeFilter("devicer:onlinedevice:view")]
        public ActionResult OnlineDeviceIndex()
        {
            return View();
        }
        [HttpGet]
        [AuthorizeFilter("devicer:onlinedevice:search")]
        public async Task<ActionResult> GetPageListJson(DeviceListParam param, Pagination pagination)
        {
            var obj = await tenantDeviceBLL.GetOnlinePageList(param, pagination);
            return Json(obj);
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("devicer:onlinedevice:search")]
        public async Task<ActionResult> GetListJson(DeviceListParam param)
        {
            TData<List<DeviceModel>> obj = await tenantDeviceBLL.GetList(param);
            return Json(obj);
        }

    

        #endregion

        #region 保存数据

        public ActionResult OnlineDeviceForm()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<DeviceEntity> obj = await tenantDeviceBLL.GetEntity(id);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("devicer:onlinedevice:add,devicer:onlinedevice:edit")]
        public async Task<ActionResult> SaveFormJson(DeviceEntity entity)
        {
            TData<string> obj = await tenantDeviceBLL.SaveForm(entity);
            return Json(obj);
        }
        #endregion

        #region 删除
        [HttpPost]
        [AuthorizeFilter("devicer:onlinedevice:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await tenantDeviceBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion

        
               [HttpGet]
        [AuthorizeFilter("devicer:onlinedevice:search")]
        public async Task<ActionResult> GetConsumerListJson(DeviceListParam param)
        {
            var obj = await tenantDeviceBLL.GetConsumerList(param);
            return Json(obj);
        }
    }
}
