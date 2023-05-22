using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Model.Result;
using YiSha.Service.Cache;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Controllers
{
    public class HomeController: BaseHomeController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           return this.Redirect("Member/Index");
            //return View();
        }
    }
}
