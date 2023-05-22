using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Koo.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.OrganizationManage;
using YiSha.Business.SystemManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.Result;
using YiSha.Model.Result.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Areas.OrganizationManage.Controllers
{
    [Area("OrganizationManage")]
    public class UserController : BaseController
    {
        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        #region 视图功能
        [AuthorizeFilter("organization:user:view")]
        public IActionResult UserIndex()
        {
            return View();
        }

        public IActionResult UserForm()
        {
            return View();
        }

       
      

      

      

        public async Task<IActionResult> UserPortrait()
        {
            ViewBag.OperatorInfo = await Operator.Instance.Current();
            return View();
        }

        public IActionResult UserImport()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:user:search")]
        public async Task<IActionResult> GetListJson(UserListParam param)
        {
            TData<List<UserEntity>> obj = await userBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:user:search")]
        public async Task<IActionResult> GetPageListJson(UserListParam param, Pagination pagination)
        {
            TData<List<UserEntity>> obj = await userBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:user:view")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<UserEntity> obj = await userBLL.GetEntity(id,true);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:user:view")]
        public async Task<IActionResult> GetUserAuthorizeJson()
        {
            TData<UserAuthorizeInfo> obj = new TData<UserAuthorizeInfo>();
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            TData<List<MenuAuthorizeInfo>> objMenuAuthorizeInfo = await new MenuAuthorizeBLL().GetAuthorizeList(operatorInfo);
            obj.Result = new UserAuthorizeInfo();
            obj.Result.IsSystem = operatorInfo.IsSystem;
            if (objMenuAuthorizeInfo.Status)
            {
                obj.Result.MenuAuthorize = objMenuAuthorizeInfo.Result;
            }
            obj.Status = true;
            return Json(obj);
        }
        #endregion

        #region 我的信息、修改我的信息、修改我的密码

        #region 查询我的信息
        [HttpGet]
        [AuthorizeFilter()]
        public IActionResult MyDetail(long id)
        {
            ViewBag.Ip = NetHelper.Ip;
            return View();
        }

      
        [HttpGet]
        [AuthorizeFilter()]
        public async Task<IActionResult> GetMyDetailJson(long id)
        {
            TData<UserEntity> obj = await userBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 修改我的信息
        [HttpGet]
        [AuthorizeFilter()]
        public IActionResult ChangeMyDetail()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeFilter()]
        public async Task<IActionResult> ChangeMyDetailJson(UserEntity entity)
        {
            TData<long> obj = await userBLL.ChangeMyDetail(entity);
            return Json(obj);
        }
        #endregion

        #region 修改我的密码
        [AuthorizeFilter()]
        public async Task<IActionResult> ChangeMyPassword()
        {
            ViewBag.OperatorInfo = await Operator.Instance.Current();
            return View();
        }

        [HttpPost]
        [AuthorizeFilter()]
        public async Task<IActionResult> ChangeMyPasswordJson(ChangePasswordParam entity)
		{
            var user = await Operator.Instance.Current();
			
			TData<long> obj = await userBLL.ChangeMyPassword(entity);
            return Json(obj);
        }
        #endregion


        #endregion

        #region 重置密码功能
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeFilter("organization:user:resetpassword")]
        public async Task<IActionResult> ResetPasswordJson(UserEntity entity)
        {
            TData<long> obj = await userBLL.ResetPassword(entity);
            return Json(obj);
        }

        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:user:add,organization:user:edit")]
        public async Task<IActionResult> SaveFormJson(UserEntity entity)
        {
            TData<string> obj = await userBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:user:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await userBLL.DeleteForm(ids);
            return Json(obj);
        }

   

      

        [HttpPost]
        [AuthorizeFilter("organization:user:edit")]
        public async Task<IActionResult> ImportUserJson(ImportParam param)
        {
            List<UserEntity> list = new ExcelHelper<UserEntity>().ImportFromExcel(param.FilePath);
            TData obj = await userBLL.ImportUser(param, list);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:user:edit")]
        public async Task<IActionResult> ExportUserJson(UserListParam param)
        {
            TData<string> obj = new TData<string>();
            TData<List<UserEntity>> userObj = await userBLL.GetList(param);
            if (userObj.Status)
            {
                string file = new ExcelHelper<UserEntity>().ExportToExcel("用户列表.xls",
                                                                          "用户列表",
                                                                          userObj.Result,
                                                                          new string[] { "UserName", "RealName", "Gender", "Mobile", "Email" });
                obj.Result = file;
                obj.Status = true;
            }
            return Json(obj);
        }
        #endregion
    }
}