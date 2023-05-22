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

using YiSha.Model.Param.TenantManage;

using NPOI.SS.Formula.Functions;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Areas.TenantManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-26 22:00
    /// 描 述：控制器类
    /// </summary>
    [Area("TenantManage")]
    public class TenantController :  BaseController
    {
        private TenantBLL tenantBLL = new TenantBLL();

        #region 租户首页 列表页
        [AuthorizeFilter("tenant:tenant:view")]
        public ActionResult TenantIndex()
        {
            return View();
        }
        [HttpGet]
        [AuthorizeFilter("tenant:tenant:search")]
        public async Task<ActionResult> GetPageListJson(TenantListParam param, Pagination pagination)
        {
            TData<List<TenantModel>> obj = await tenantBLL.GetPageList(param, pagination);
            return Json(obj);
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("tenant:tenant:search")]
        public async Task<ActionResult> GetListJson(TenantListParam param)
        {
            TData<List<TenantEntity>> obj = await tenantBLL.GetList(param);
            return Json(obj);
        }

      


        #endregion

        #region 删除租户


        [HttpPost]
        [AuthorizeFilter("tenant:tenant:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await tenantBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion

        #region 我的租户的显示、修改
        [HttpGet]
        [AuthorizeFilter("tenant:tenant:mytenantinfo")]
        public ActionResult MyTenantInfo()
        {
            return View();
        }
        [HttpGet]
        [AuthorizeFilter("tenant:tenant:mytenantinfo")]
        public async Task<ActionResult> GetMyTenantInfoFormJson()
        {
            var info = this.GetCurrentInfo();
            TData<TenantEntity> obj = await tenantBLL.GetEntity(info.CurrentTenantId.Value);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("tenant:tenant:mytenantinfo")]
        public async Task<ActionResult> SaveMyTenantInfoFormJson(TenantEntity entity)
        {
            var service = new TenantService();
           
            await service.SaveMyTenantInfoForm(entity);
            var obj = TData.CreateSuccessdMsg("修改成功");
            
            return Json(obj);
        }
        #endregion


        #region 创建租户
        [AuthorizeFilter]
        public ActionResult TenantForm()
        {
            return View();
        }
        [HttpGet]
        [AuthorizeFilter]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<TenantEntity> obj = await tenantBLL.GetEntity(id);
            return Json(obj);
        }
        /// <summary>
        /// 所有用户都可创建租户，不需要权限校验
        /// 切换租户之后，如果当前用户是租户的创建都，会自动继承创建创建者的角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        //[AuthorizeFilter("tenant:tenant:add,tenant:tenant:edit")]
        public async Task<ActionResult> SaveFormJson(TenantEntity entity)
        {
            TData<string> obj = await tenantBLL.SaveForm(entity);
            return Json(obj);
        }
        #endregion


        #region 加入租户
        [AuthorizeFilter]
        public ActionResult JoinTenantForm()
        {
            return View();
        }

        /// <summary>
        /// 加入组织
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        //[AuthorizeFilter("tenant:tenant:add,tenant:tenant:edit")]
        public async Task<ActionResult> SaveJoinTenantFormJson(TenantEntity entity)
        {
            var myTenantService = new MyTenantService();
            try
            {
                var msg =await myTenantService.JoinToTenant(entity.Code, entity.Name);
                var obj= TData.CreateSuccessdMsg(msg);
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
