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
using YiSha.Entity.OrganizationManage;
using YiSha.Business.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.OrganizationManage;

namespace YiSha.Admin.Web.Areas.OrganizationManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-24 10:41
    /// 描 述：控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class UserMsgController :  BaseController
    {
        private UserMsgBLL userMsgBLL = new UserMsgBLL();

        #region 首页列表
        [AuthorizeFilter()]
        public ActionResult UserMsgIndex()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeFilter()]
        public async Task<ActionResult> GetPageListJson(UserMsgListParam param, Pagination pagination)
        {
            TData<List<UserMsgModel>> obj = await userMsgBLL.GetPageList(param, pagination);
            return Json(obj);
        }
      
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter()]
        public async Task<ActionResult> GetListJson(UserMsgListParam param)
        {
            TData<List<UserMsgEntity>> obj = await userMsgBLL.GetList(param);
            return Json(obj);
        }

        #endregion

        #region 查看消息
        public ActionResult ViewUserMsgForm()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetViewFormJson(long id)
        {
            TData<UserMsgEntity> obj = await userMsgBLL.GetEntity(id);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter(/*"organization:usermsg:add,organization:usermsg:edit"*/)]
        public async Task<ActionResult> SaveViewFormJson(UserMsgEntity entity)
        {
            TData<string> obj = await userMsgBLL.AckForm(entity);
            return Json(obj);
        }
        #endregion

        #region 保存消息
        public ActionResult UserMsgForm()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<UserMsgEntity> obj = await userMsgBLL.GetEntity(id);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter(/*"organization:usermsg:add,organization:usermsg:edit"*/)]
        public async Task<ActionResult> SaveFormJson(UserMsgEntity entity)
        {
            TData<string> obj = await userMsgBLL.SaveForm(entity);
            return Json(obj);
        }
        #endregion

        #region 删除数据

        [HttpPost]
        [AuthorizeFilter(/*"organization:usermsg:delete"*/)]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await userMsgBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
