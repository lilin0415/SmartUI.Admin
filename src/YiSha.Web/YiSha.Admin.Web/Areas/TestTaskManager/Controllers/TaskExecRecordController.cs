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

namespace YiSha.Admin.Web.Areas.TestTaskManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-16 18:02
    /// 描 述：控制器类
    /// </summary>
    [Area("TestTaskManager")]
    public class TaskExecRecordController :  BaseController
    {
        private TaskExecRecordBLL taskExecRecordBLL = new TaskExecRecordBLL();

        #region 列表数据功能
        [AuthorizeFilter("testtaskr:taskexecrecord:view")]
        public ActionResult TaskExecRecordIndex()
        {
            return View();
        }
        [HttpGet]
        [AuthorizeFilter("testtaskr:taskexecrecord:search")]
        public async Task<ActionResult> GetListJson(TaskExecRecordListParam param)
        {
            TData<List<TaskExecRecordEntity>> obj = await taskExecRecordBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("testtaskr:taskexecrecord:search")]
        public async Task<ActionResult> GetPageListJson(TaskExecRecordListParam param, Pagination pagination)
        {
            TData<List<TaskExecRecordModel>> obj = await taskExecRecordBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        #endregion

        #region 编辑


        public ActionResult TaskExecRecordForm()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<TaskExecRecordEntity> obj = await taskExecRecordBLL.GetEntity(id);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("testtaskr:taskexecrecord:add,testtaskr:taskexecrecord:edit")]
        public async Task<ActionResult> SaveFormJson(TaskExecRecordEntity entity)
        {
            TData<string> obj = await taskExecRecordBLL.SaveForm(entity);
            return Json(obj);
        }
        #endregion

        #region 删除


        [HttpPost]
        [AuthorizeFilter("testtaskr:taskexecrecord:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await taskExecRecordBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion


        #region 取消作业的执行
        [HttpPost]
        [AuthorizeFilter("testtaskr:taskexecrecord:add,testtaskr:taskexecrecord:edit")]
        public async Task<ActionResult> CancelTaskJson(long id)
        {
            var service = new TaskExecRecordService();
            await service.CancelTaskExec(id);
            TData obj = new TData();
            obj.Status = true;
            return Json(obj);
        }
        #endregion
    }
}
