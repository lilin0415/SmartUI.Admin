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

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-13 14:13
    /// 描 述：控制器类
    /// </summary>
    [Area("SystemManage")]
    public class UserTokenController :  BaseController
    {
        private UserTokenBLL userTokenBLL = new UserTokenBLL();

        #region 视图功能
        [AuthorizeFilter("system:usertoken:view")]
        public ActionResult UserTokenIndex()
        {
            return View();
        }

        public ActionResult UserTokenForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:usertoken:search")]
        public async Task<ActionResult> GetListJson(UserTokenListParam param)
        {
            TData<List<UserTokenEntity>> obj = await userTokenBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:usertoken:search")]
        public async Task<ActionResult> GetPageListJson(UserTokenListParam param, Pagination pagination)
        {
            TData<List<UserTokenEntity>> obj = await userTokenBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<UserTokenEntity> obj = await userTokenBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("system:usertoken:add,system:usertoken:edit")]
        public async Task<ActionResult> SaveFormJson(UserTokenEntity entity)
        {
            TData<string> obj = await userTokenBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("system:usertoken:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await userTokenBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
