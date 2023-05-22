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
using YiSha.Entity.TestTaskManager;
using YiSha.Business.TestTaskManager;
using YiSha.Model.Param.TestTaskManager;
using YiSha.Model.TestTaskManager;
using YiSha.Service.TestTaskManager;
using YiSha.Service.DeviceManager;

namespace YiSha.Admin.Web.Areas.TestTaskManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-16 18:04
    /// 描 述：控制器类
    /// </summary>
    [Area("TestTaskManager")]
    public class CaseExecRecordController :  BaseController
    {
        private CaseExecRecordBLL caseExecRecordBLL = new CaseExecRecordBLL();

        #region 视图功能
        [AuthorizeFilter("testtaskr:caseexecrecord:view")]
        public ActionResult CaseExecRecordIndex()
        {
            return View();
        }

        public ActionResult CaseExecRecordForm()
        {
            return View();
        }
        #endregion

        #region 获取数据

        [HttpGet]
        [AuthorizeFilter("testtaskr:caseexecrecord:search")]
        public async Task<ActionResult> GetListJson(CaseExecRecordListParam param)
        {
            TData<List<CaseExecRecordEntity>> obj = await caseExecRecordBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("testtaskr:caseexecrecord:search")]
        public async Task<ActionResult> GetPageListJson(CaseExecRecordListParam param, Pagination pagination)
        {
            TData<List<CaseExecRecordModel>> obj = await caseExecRecordBLL.GetPageList(param, pagination);
            return Json(obj);
        }
        [HttpGet]
        [AuthorizeFilter("testtaskr:caseexecrecord:search")]
        public async Task<ActionResult> GetUnfinishedPageListJson(CaseExecRecordListParam param, Pagination pagination)
        {
            TData<List<CaseExecRecordModel>> obj = await caseExecRecordBLL.GetUnfinishedPageListJson(param, pagination);
            return Json(obj);
        }
        
        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<CaseExecRecordEntity> obj = await caseExecRecordBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 保存数据
        [HttpPost]
        [AuthorizeFilter("testtaskr:caseexecrecord:add,testtaskr:caseexecrecord:edit")]
        public async Task<ActionResult> SaveFormJson(CaseExecRecordEntity entity)
        {
            TData<string> obj = await caseExecRecordBLL.SaveForm(entity);
            return Json(obj);
        }


        #endregion


        [HttpPost]
        [AuthorizeFilter("testtaskr:caseexecrecord:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await caseExecRecordBLL.DeleteForm(ids);
            return Json(obj);
        }

        #region 取消作业的执行
        [HttpPost]
        [AuthorizeFilter("testtaskr:caseexecrecord:add,testtaskr:caseexecrecord:edit")]
        public async Task<ActionResult> CancelJobJson(long id)
        {
            var service = new CaseExecRecordService();
            await service.CancelCaseExec(id);
            TData obj = new TData();
            obj.Status = true;
            return Json(obj);
        }
        #endregion

        #region 选择消费者（指定的客户端、客户端组）
        public ActionResult ViewConsumerIndex(int? consumeMode,long? consumerId)
        {
            this.ViewBag.GroupName = string.Empty;

            if (consumeMode == 2)
            {
                var group = new DeviceGroupService().GetEntity(consumerId.GetValueOrDefault());
                if (group == null)
                {
                }
                else
                {
                    this.ViewBag.GroupName = group.Result.Name;
                }

            }
            return View();
        }
        #endregion
    }
}
