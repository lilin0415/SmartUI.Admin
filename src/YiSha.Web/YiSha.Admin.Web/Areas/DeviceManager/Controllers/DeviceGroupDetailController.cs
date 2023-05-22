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
using YiSha.Model.TestCaseManager;
using YiSha.Service.TestTaskManager;
using YiSha.Model.TestTaskManager;
using YiSha.Entity.TestTaskManager;
using YiSha.Service.DeviceManager;

namespace YiSha.Admin.Web.Areas.DeviceManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-04-22 09:42
    /// 描 述：控制器类
    /// </summary>
    [Area("DeviceManager")]
    public class DeviceGroupDetailController :  BaseController
    {
        private DeviceGroupDetailBLL deviceGroupDetailBLL = new DeviceGroupDetailBLL();

        #region 视图功能
        [AuthorizeFilter("devicer:devicegroupdetail:view")]
        public ActionResult DeviceGroupDetailIndex()
        {
            return View();
        }

        public ActionResult DeviceGroupDetailForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("devicer:devicegroupdetail:search")]
        public async Task<ActionResult> GetListJson(DeviceGroupDetailListParam param)
        {
            var obj = await deviceGroupDetailBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("devicer:devicegroupdetail:search")]
        public async Task<ActionResult> GetPageListJson(DeviceGroupDetailListParam param, Pagination pagination)
        {
            var obj = await deviceGroupDetailBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<DeviceGroupDetailEntity> obj = await deviceGroupDetailBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("devicer:devicegroupdetail:add,devicer:devicegroupdetail:edit")]
        public async Task<ActionResult> SaveFormJson(DeviceGroupDetailEntity entity)
        {
            TData<string> obj = await deviceGroupDetailBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("devicer:devicegroupdetail:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await deviceGroupDetailBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion

        #region 选择客户端
        public ActionResult SelectDeviceIndex()
        {
            return View();
        }
        #endregion

        
        [HttpPost]
        public async Task<ActionResult> SaveDeviceListForm([FromQuery] long groupId, DeviceEntity[] caseList)
        {
            var service = new DeviceGroupDetailService();
            await service.AddList(groupId, caseList.ToList());

            return Json(TData.CreateSuccessdValue(1));

        }
    }
}
