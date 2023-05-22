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
using Koo.Utilities.Exceptions;
using YiSha.Business.DeviceManager;
using YiSha.Entity.DeviceManager;

namespace YiSha.Admin.Web.Areas.DeviceManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-12 07:17
    /// 描 述：控制器类
    /// </summary>
    [Area("DeviceManager")]
    [AuthorizeFilter]
    public class DeviceController :  BaseController
    {
        private DeviceBLL deviceBLL = new DeviceBLL();

        #region 首页
        [AuthorizeFilter()]
        public ActionResult DeviceIndex()
        {
            return View();
        }

        /// <summary>
        /// 查找我的设备
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        //[AuthorizeFilter("devicer:device:search")]
        public async Task<ActionResult> GetMyPageListJson(DeviceListParam param, Pagination pagination)
        {
            TData<List<DeviceEntity>> obj = await deviceBLL.GetMyPageList(param, pagination);
            return Json(obj);
        }

        /// <summary>
        /// 所有设备
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        //[AuthorizeFilter("devicer:device:search")]
        public async Task<ActionResult> GetPageListJson(DeviceListParam param, Pagination pagination)
        {
            var obj = await deviceBLL.GetOnlinePageList(param, pagination);
            return Json(obj);
        }

        #endregion

        #region 获取数据
        [HttpGet]
        //[AuthorizeFilter("devicer:device:search")]
        public async Task<ActionResult> GetListJson(DeviceListParam param)
        {
            var obj = await deviceBLL.GetList(param);
            return Json(obj);
        }

        #endregion

        #region 添加、修改设备
        public ActionResult DeviceForm()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<DeviceEntity> obj = await deviceBLL.GetEntity(id);
            return Json(obj);
        }
        [HttpPost]
        //[AuthorizeFilter("devicer:device:add,testtaskr:device:edit")]
        public async Task<ActionResult> SaveFormJson(DeviceEntity entity)
        {
            throw new ForbidOperationExection();
            TData<string> obj = await deviceBLL.SaveForm(entity);
            return Json(obj);
        }


        #endregion

        #region 删除设备
        [HttpPost]
        //[AuthorizeFilter("devicer:device:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await deviceBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
