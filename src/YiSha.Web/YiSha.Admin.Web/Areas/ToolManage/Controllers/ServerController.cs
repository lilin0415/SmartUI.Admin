using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.Web.Controllers;
using YiSha.Model.Result.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.ToolManage.Controllers
{
    [Area("ToolManage")]
    public class ServerController : BaseController
    {
        #region 视图功能
        [AuthorizeFilter("tool:server:view")]
        public IActionResult ServerIndex()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("tool:server:view")]
        public IActionResult GetServerJson()
        {
            TData<ComputerInfo> obj = new TData<ComputerInfo>();
            ComputerInfo computerInfo = null;
            try
            {
                computerInfo = ComputerHelper.GetComputerInfo();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                obj.Message = ex.Message;
            }
            obj.Result = computerInfo;
            obj.Status = true;
            return Json(obj);
        }

        public IActionResult GetServerIpJson()
        {
            TData<string> obj = new TData<string>();
            string ip = NetHelper.GetWanIp();
            string ipLocation = IpLocationHelper.GetIpLocation(ip);
            obj.Result = string.Format("{0} ({1})", ip, ipLocation);
            obj.Status = true;
            return Json(obj);
        }
        #endregion    　
    }
}