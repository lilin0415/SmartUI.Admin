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
using YiSha.Model.TestTaskManager;

namespace YiSha.Business.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-16 18:04
    /// 描 述：业务类
    /// </summary>
    public class CaseExecRecordBLL
    {
        private CaseExecRecordService caseExecRecordService = new CaseExecRecordService();

        #region 获取数据
        public async Task<TData<List<CaseExecRecordEntity>>> GetList(CaseExecRecordListParam param)
        {
            TData<List<CaseExecRecordEntity>> obj = new TData<List<CaseExecRecordEntity>>();
            obj.Result = await caseExecRecordService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }
        
              public async Task<TData<List<CaseExecRecordModel>>> GetUnfinishedPageListJson(CaseExecRecordListParam param, Pagination pagination)
        {
            TData<List<CaseExecRecordModel>> obj = new TData<List<CaseExecRecordModel>>();
            obj.Result = await caseExecRecordService.GetUnfinishedPageListJson(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }
        public async Task<TData<List<CaseExecRecordModel>>> GetPageList(CaseExecRecordListParam param, Pagination pagination)
        {
            TData<List<CaseExecRecordModel>> obj = new TData<List<CaseExecRecordModel>>();
            obj.Result = await caseExecRecordService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<CaseExecRecordEntity>> GetEntity(long id)
        {
            TData<CaseExecRecordEntity> obj = new TData<CaseExecRecordEntity>();
            obj.Result = await caseExecRecordService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(CaseExecRecordEntity entity)
        {
            TData<string> obj = new TData<string>();
            await caseExecRecordService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await caseExecRecordService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
