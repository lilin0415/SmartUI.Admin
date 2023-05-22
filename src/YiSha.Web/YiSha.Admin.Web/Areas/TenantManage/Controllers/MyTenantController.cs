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
using YiSha.Entity.TenantManage;
using YiSha.Business.TenantManage;
using YiSha.Model.Param.TenantManage;
using YiSha.Service.TenantManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.TenantManage;
using YiSha.Business.ProjectManager;
using YiSha.Entity.OrganizationManage;

namespace YiSha.Admin.Web.Areas.TenantManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-26 22:01
    /// 描 述：控制器类
    /// </summary>
    [Area("TenantManage")]
    public class MyTenantController :  BaseController
    {
        private MyTenantBLL myTenantBLL = new MyTenantBLL();

        #region 视图功能


        #endregion

        #region 获取数据
        //[HttpGet]
        //[AuthorizeFilter("tenant:mytenant:search")]
        //public async Task<ActionResult> GetMyTenant()
        //{
        //    var user = this.GetCurrentInfo();

        //    var service = new MyTenantService();
        //    var items = service.GetMyTenantInfoList(user.UserId.Value);

            
        //    var my = user.MyTenantList.First
        //    TData<List<MyTenantEntity>> obj = await myTenantBLL.GetList(param);
        //    return Json(obj);
        //}

        [HttpGet]
        [AuthorizeFilter("tenant:mytenant:search")]
        public async Task<ActionResult> GetListJson(MyTenantListParam param)
        {
            TData<List<MyTenantEntity>> obj = await myTenantBLL.GetList(param);
            return Json(obj);
        }

      
     
        [HttpGet]
        [AuthorizeFilter("tenant:mytenant:search")]
        public async Task<ActionResult> GetPageListJson(MyTenantListParam param, Pagination pagination)
        {
            TData<List<MyTenantEntity>> obj = await myTenantBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        //GetMyTenantUserFormJson
        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<MyTenantEntity> obj = await myTenantBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion


        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("tenant:mytenant:add,tenant:mytenant:edit")]
        public async Task<ActionResult> SaveFormJson(MyTenantEntity entity)
        {
            TData<string> obj = await myTenantBLL.SaveForm(entity);
            return Json(obj);
        }
        #endregion


        #region 查询当 用户列表
        [AuthorizeFilter("tenant:mytenant:view")]
        public ActionResult MyTenantIndex()
        {
            return View();
        }
        [AuthorizeFilter("tenant:mytenant:search")]
        public async Task<ActionResult> GetMyTenantUserPageListJson(MyTenantListParam param, Pagination pagination)
        {
            var myTenantService = new MyTenantService();

            TData<List<MyTenantUserModel>> obj = new TData<List<MyTenantUserModel>>();
            obj.Result = await myTenantService.GetMyTenantUserPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;

            return Json(obj);
        }

        #endregion

        #region 查看用户详情、编辑用户

        [AuthorizeFilter("tenant:mytenant:detail")]
        public ActionResult MyTenantForm()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeFilter("tenant:mytenant:detail")]
        public async Task<ActionResult> GetMyTenantUserFormJson(long id)
        {
            var myTenantService = new MyTenantService();

            TData<MyTenantUserModel> obj = new TData<MyTenantUserModel>();
            obj.Result = await myTenantService.GetMyTenantUserEntity(id);

            obj.Status = true;

            return Json(obj);

        }
        [HttpPost]
        [AuthorizeFilter("tenant:mytenant:add,tenant:mytenant:edit")]
        public async Task<ActionResult> SaveMyTenantUserFormJson(MyTenantUserModel entity)
        {
            var myTenantService = new MyTenantService();
            var id = await myTenantService.SaveForm(entity);
            var obj = TData.CreateSuccessdValue(id);
            return Json(obj);
        }
        #endregion

        #region 删除
        [HttpPost]
        [AuthorizeFilter("tenant:mytenant:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await myTenantBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion

        #region 邀请用户

        [AuthorizeFilter("tenant:mytenant:detail")]
        public ActionResult InviteUserForm()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeFilter("tenant:mytenant:add,tenant:mytenant:edit")]
        public async Task<ActionResult> SaveInviteUserFormJson(UserEntity entity)
        {
            var myTenantService = new MyTenantService();
            try
            {
                var msg = await myTenantService.InviteToTenant(entity.Id.GetValueOrDefault(), entity.UserName);
                var obj = TData.CreateSuccessdMsg(msg);
                return Json(obj);
            }
            catch (Exception ex)
            {
                var obj = TData.CreateFailedMsg(ex);
                return Json(obj);
            }
        }
        #endregion
    }
}
