using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.ProjectManager;
using YiSha.Entity.TestCaseManager;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.StatsModels;
using YiSha.Service;
using YiSha.Service.Cache;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Controllers
{
    public class StatsController: BaseController
    {
        private StatsCache statsService = new StatsCache();

        [HttpGet]
        
        public async Task<IActionResult> GetProjectStatsJson()
        {
            var data = await statsService.GetStatsModel<ProjectStatsModel>();
            return Json(TData.CreateSuccessdValue(data));
        }

        [HttpGet]
        public async Task<IActionResult> GetTestCaseStatsJson()
        {
            var data = await statsService.GetStatsModel<TestCaseStatsModel>();
            return Json(TData.CreateSuccessdValue(data));
        }


        [HttpGet]
        public async Task<IActionResult> GetTestTaskStatsJson()
        {
            var data = await statsService.GetStatsModel<TestTaskStatsModel>();
            return Json(TData.CreateSuccessdValue(data));
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskExecStatsJson()
        {
            var data = await statsService.GetStatsModel<TaskExecStatsModel>();
            return Json(TData.CreateSuccessdValue(data));
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseExecStatsJson()
        {
            var data = await statsService.GetStatsModel<CaseExecStatsModel>();
            return Json(TData.CreateSuccessdValue(data));
        }

    }
}
