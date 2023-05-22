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
using YiSha.Business.ProductCategoryManager;
using YiSha.Entity.ProductCategoryManager;
using YiSha.Model.Param.ProductCategoryManager;
using YiSha.Model.Param.ProjectManager;
using YiSha.Model.Result;
using YiSha.Service.ProductCategoryManager;
using YiSha.Service.ProjectManager;
using YiSha.Business.ProjectManager;
using YiSha.Model.TestCaseManager;

namespace YiSha.Business.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:13
    /// 描 述：业务类
    /// </summary>
    public class TestCaseBLL
    {
        private TestCaseService testCaseService = new TestCaseService();

        #region 获取数据
        public async Task<TData<List<ZtreeInfo>>> GetTree()
        {
            
            var ret = await testCaseService.GetAllListAsTree();
            return new TData<List<ZtreeInfo>>(ret);
        }

        public async Task<TData<List<TestCaseEntity>>> GetList(TestCaseListParam param)
        {
            TData<List<TestCaseEntity>> obj = new TData<List<TestCaseEntity>>();
            obj.Result = await testCaseService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<TestCaseModel>>> GetPageList(TestCaseListParam param, Pagination pagination)
        {
            TData<List<TestCaseModel>> obj = new TData<List<TestCaseModel>>();
            obj.Result = await testCaseService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<TestCaseEntity>> GetEntity(long id)
        {
            TData<TestCaseEntity> obj = new TData<TestCaseEntity>();
            obj.Result = await testCaseService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(TestCaseEntity entity)
        {
            TData<string> obj = new TData<string>();
            await testCaseService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await testCaseService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
