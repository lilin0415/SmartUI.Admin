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
using YiSha.Entity.SystemManage;
using YiSha.Business.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.Cache;
using YiSha.Model.WebApis;
using YiSha.Service.SystemManage;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-02-16 21:00
    /// 描 述：控制器类
    /// </summary>
    [Area("SystemManage")]
    public class ConfigController :  BaseController
    {
        private ConfigBLL configBLL = new ConfigBLL();

        #region 视图功能
        [AuthorizeFilter("system:config:view")]
        public ActionResult ConfigIndex()
        {
            return View();
        }

        public ActionResult ConfigForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:config:search")]
        public async Task<ActionResult> GetListJson(ConfigListParam param)
        {
            TData<List<ConfigEntity>> obj = await configBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:config:search")]
        public async Task<ActionResult> GetPageListJson(ConfigListParam param, Pagination pagination)
        {
            TData<List<ConfigEntity>> obj = await configBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<ConfigEntity> obj = await configBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        //#region 提交数据
        //[HttpPost]
        //[AuthorizeFilter("system:config:add,system:config:edit")]
        //public async Task<ActionResult> SaveFormJson(ConfigEntity entity)
        //{
        //    TData<string> obj = await configBLL.SaveForm(entity);
        //    return Json(obj);
        //}

        //[HttpPost]
        //[AuthorizeFilter("system:config:delete")]
        //public async Task<ActionResult> DeleteFormJson(string ids)
        //{
        //    TData obj = await configBLL.DeleteForm(ids);
        //    return Json(obj);
        //}
        //#endregion

        [HttpGet]
        public async Task<ActionResult> GetIndexFormJson(long id)
        {
            ConfigCache configCache = new ConfigCache();
            var m = await configCache.GetConfigModel();
            //前台不显示密码
            m.MailPassword = string.Empty;

            TData<SystemConfigModel> obj = new TData<SystemConfigModel>();
            obj.Status = true;
            obj.Result = m;

            return Json(obj);
        }

        #region 修改基本信息
        public ActionResult BasicForm()
        {
            return View();
        }
        [HttpPost]
        [AuthorizeFilter("system:config:edit_basic")]
        public async Task<ActionResult> SaveBasicFormJson(SystemConfigModel entity)
        {
            ConfigService configService = new ConfigService();
            await configService.SaveForm(entity, SystemConfigCategoryAttribute.Basic);
            TData<string> obj = new TData<string>();
            obj.Status = true;
            return Json(obj);
        }
        #endregion

        #region 修改基本信息
        public ActionResult RsaForm()
        {
            return View();
        }
        [HttpPost]
        [AuthorizeFilter("system:config:edit_rsa")]
        public async Task<ActionResult> SaveRsaFormJson(SystemConfigModel entity)
        {
            ConfigService configService = new ConfigService();
            await configService.SaveForm(entity, SystemConfigCategoryAttribute.Rsa);
            TData<string> obj = new TData<string>();
            obj.Status = true;
            return Json(obj);
        }
        #endregion

        #region 修改基本信息
        public ActionResult MailForm()
        {
            return View();
        }
        [HttpPost]
        [AuthorizeFilter("system:config:edit_mail")]
        public async Task<ActionResult> SaveMailFormJson(SystemConfigModel entity)
        {
            ConfigService configService = new ConfigService();
            await configService.SaveForm(entity, SystemConfigCategoryAttribute.Mail);
            TData<string> obj = new TData<string>();
            obj.Status = true;
            return Json(obj);
        }
        #endregion
    }
}
