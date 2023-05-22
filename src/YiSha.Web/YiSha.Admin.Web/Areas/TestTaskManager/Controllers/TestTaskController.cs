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
using YiSha.Service.TestTaskManager;
using YiSha.Model.TestTaskManager;
using YiSha.Service.TestCaseManager;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Business.AutoJob;
using YiSha.Model.DeviceManager;
using YiSha.Service.DeviceManager;

namespace YiSha.Admin.Web.Areas.TestTaskManager.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-02 16:20
    /// 描 述：控制器类
    /// </summary>
    [Area("TestTaskManager")]
    public class TestTaskController :  BaseController
    {
        private TestTaskBLL testTaskBLL = new TestTaskBLL();

        #region 列表页面数据
        [AuthorizeFilter("testtaskr:testtask:view")]
        public ActionResult TestTaskIndex()
        {
            return View();
        }

     
        [HttpGet]
        [AuthorizeFilter("testtaskr:testtask:search")]
        public async Task<ActionResult> GetListJson(TestTaskListParam param)
        {
            TData<List<TestTaskEntity>> obj = await testTaskBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("testtaskr:testtask:search")]
        public async Task<ActionResult> GetPageListJson(TestTaskListParam param, Pagination pagination)
        {
            var testTaskService = new TestTaskService();

            TData<List<TestTaskModel>> obj = new TData<List<TestTaskModel>>();
            obj.Result = await testTaskService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return Json(obj);

            
            //TData<List<TestTaskModel>> obj = await testTaskBLL.GetPageList(param, pagination);
            //return Json(obj);
        }

        #endregion


        #region 编辑功能
        public ActionResult TestTaskForm()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<TestTaskEntity> obj = await testTaskBLL.GetEntity(id, true);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("testtaskr:testtask:add,testtaskr:testtask:edit")]
        public async Task<ActionResult> SaveFormJson(TestTaskEntity entity)
        {
            TData<string> obj = await testTaskBLL.SaveForm(entity);
            if (obj.Status)
            {
                var jobCenter = new JobCenter();
                var dbEntity = await testTaskBLL.GetEntity(entity.Id.Value);
                await jobCenter.AddOrUpdate(dbEntity.Result);
            }
            return Json(obj);
        }

        #endregion

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("testtaskr:testtask:add,testtaskr:testtask:edit")]
        public async Task<IActionResult> ChangeStatusJson(TestTaskEntity entity)
        {
            TData<string> obj = await testTaskBLL.ChangeStatusJson(entity);
            if (obj.Status)
            {
                var jobCenter = new JobCenter();
                var dbEntity = await testTaskBLL.GetEntity(entity.Id.Value);
                await jobCenter.AddOrUpdate(dbEntity.Result);
            }
            return Json(obj);
        }

        #region 删除数据

        [HttpPost]
        [AuthorizeFilter("testtaskr:testtask:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            var items = await testTaskBLL.GetListByIds(ids);

            TData obj = await testTaskBLL.DeleteForm(ids);
            if (obj.Status)
            {
                var jobCenter = new JobCenter();

                foreach (var item in items.Result)
                {
                    await jobCenter.Delete(item);
                }
            }
            return Json(obj);
        }
        #endregion


        #region 部署任务到指定客户端
        public ActionResult DeployTask(long taskId)
        {
            return View();
        }

        [HttpGet]
        [AuthorizeFilter("devicer:device:search")]
        public async Task<ActionResult> GetDeviceListJson(long taskId, DeviceListParam param)
        {
           
            var deployTaskService = new DeployTaskService();
            var deployedItems = await deployTaskService.GetListByTaskId(taskId);


            var deviceService = new DeviceService();
            var deviceList = await deviceService.GetOnlinePageList(param,new Pagination());

            foreach (var deviceItem in deviceList)
            {
                foreach (var deployedItem in deployedItems)
                {
                    if (deployedItem == null)
                    {
                        continue;
                    }

                    if (deviceItem.CheckIsDeployed(deployedItem))
                    {
                        break;
                    }
                }
            }

            TData<List<DeviceModel>> obj = new TData<List<DeviceModel>>();
            obj.Result = deviceList;
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return Json(obj);

        }

   
        [HttpPost]
        public async Task<ActionResult> SaveDeployListForm([FromQuery] long taskId, TenantDeviceModel[] deviceList)
        {
            var deployTaskService = new DeployTaskService();
            await deployTaskService.SaveList(taskId, deviceList);

            return Json(TData.CreateSuccessdValue(1));

        }
        #endregion

      
        /// <summary>
        /// 立即执行定时任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpPost]
        //[AuthorizeFilter("testtaskr:testtask:delete")]
        public async Task<ActionResult> ExecTask(long taskId)
        {
            var service = new TaskExecRecordService();
            await service.AddManuallyRecord(taskId);

            return Json(TData.CreateSuccessdValue(true));
        }

        #region 选择消费者（指定的客户端、客户端组）
        public ActionResult SelectConsumerIndex()
        {
            return View();
        }
        #endregion
    }
}
