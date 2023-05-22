using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.TestTaskManager;
using YiSha.Model.Param.TestTaskManager;
using YiSha.Service.TestTaskManager;

namespace YiSha.Business.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-03-25 19:49
    /// 描 述：业务类
    /// </summary>
    public class CaseExecLogBLL
    {
        private CaseExecLogService caseExecLogService = new CaseExecLogService();

        #region 获取数据
        public async Task<TData<List<CaseExecLogEntity>>> GetList(CaseExecLogListParam param)
        {
            TData<List<CaseExecLogEntity>> obj = new TData<List<CaseExecLogEntity>>();
            obj.Result = await caseExecLogService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<CaseExecLogEntity>>> GetPageList(CaseExecLogListParam param, Pagination pagination)
        {
            TData<List<CaseExecLogEntity>> obj = new TData<List<CaseExecLogEntity>>();
            obj.Result = await caseExecLogService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<CaseExecLogEntity>> GetEntity(long id)
        {
            TData<CaseExecLogEntity> obj = new TData<CaseExecLogEntity>();
            obj.Result = await caseExecLogService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(CaseExecLogEntity entity)
        {
            TData<string> obj = new TData<string>();
            await caseExecLogService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await caseExecLogService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
