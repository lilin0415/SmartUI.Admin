using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.TestCaseManager;
using YiSha.Model.Param.TestCaseManager;
using YiSha.Service.TestCaseManager;
using YiSha.Service.ProjectManager;
using YiSha.Model.Publishes;

namespace YiSha.Business.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:14
    /// 描 述：业务类
    /// </summary>
    public class TestCaseVarJsonBLL
    {
        private TestCaseParameterService testCaseParameterService = new TestCaseParameterService();

        #region 获取数据
      

        public async Task<TData<List<TestCaseParameterEntity>>> GetList(TestCaseListParameterListParam param)
        {
            TData<List<TestCaseParameterEntity>> obj = new TData<List<TestCaseParameterEntity>>();
            obj.Result = await testCaseParameterService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<TestCaseParameterEntity>>> GetPageList(TestCaseListParameterListParam param, Pagination pagination)
        {
            TData<List<TestCaseParameterEntity>> obj = new TData<List<TestCaseParameterEntity>>();
            obj.Result = await testCaseParameterService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<TestCaseParameterEntity>> GetEntity(long id)
        {
            TData<TestCaseParameterEntity> obj = new TData<TestCaseParameterEntity>();
            obj.Result = await testCaseParameterService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(TestCaseParameterEntity entity)
        {
            TData<string> obj = new TData<string>();
            await testCaseParameterService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await testCaseParameterService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
