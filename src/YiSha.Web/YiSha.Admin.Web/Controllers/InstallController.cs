using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Model.Result;
using YiSha.Service.Cache;
using YiSha.Service.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Controllers
{
    public class InstallController : Controller
    {
        private string installedLockFile = string.Empty;
        public InstallController()
        {
            installedLockFile = GlobalContext.GetAppData("install", "installed.lock");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (System.IO.File.Exists(installedLockFile))
            {
                this.ViewBag.Installed = true;
            }
            else
            {
                this.ViewBag.Installed = false;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MakeInstallSqlJson()
        {
            TData obj = new TData();

            try
            {
                if (GlobalContext.SystemConfig.IsReleased)
                {
					obj.Status = false;
					obj.Message = "已发布";
				}
                else
                {
					string databaseName = GlobalContext.SystemConfig.GetDatabaseName();

					DatabaseTableMySqlService service = new DatabaseTableMySqlService();
					await service.MakeInstallSql(databaseName);

					obj.Status = true;
					obj.Message = "制作成功";
				}
               
            }
            catch (Exception ex)
            {
                System.IO.File.Delete(installedLockFile);
                obj.Status = false;
                obj.Message = ex.Message;
            }

            return Json(obj);
        }
        [HttpPost]
        public async Task<IActionResult> InstallJson()
        {
            TData obj = new TData();

            if (System.IO.File.Exists(installedLockFile))
            {
                obj.Status = false;
                obj.Message = "已安装，不可重复安装";
            }
            else
            {
                
                try
                {
                    //GlobalContext
                    System.IO.File.Create(installedLockFile).Close();

                    string databaseName = GlobalContext.SystemConfig.GetDatabaseName();

                    DatabaseTableMySqlService service = new DatabaseTableMySqlService();
                    await service.InstallDatabase(databaseName);

                    obj.Status = true;
                    obj.Message = "安装成功";
                }
                catch(Exception ex)
                {
                    System.IO.File.Delete(installedLockFile);
                    obj.Status = false;
                    obj.Message = ex.Message;
                } 
            }

            return Json(obj);
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

         

            var resultContext = await next();


            sw.Stop();
            

            if (resultContext.Exception != null)
            {
                if (IsAjaxRequest(context))
                {
                    var d = new TData<Exception>();
                    d.Result = resultContext.Exception;
                    d.Message = resultContext.Exception.ToString();
                    d.Status = false;

                    resultContext.Result = new JsonResult(d);
                    resultContext.ExceptionHandled = true;

                }
            }
        }

        private bool IsAjaxRequest(ActionExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;
            if (headers.ContainsKey("X-Requested-With") && headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return true;
            }
            return false;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
