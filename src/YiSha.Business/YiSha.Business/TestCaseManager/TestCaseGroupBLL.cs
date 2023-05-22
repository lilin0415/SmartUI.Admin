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

namespace YiSha.Business.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-04 16:17
    /// 描 述：业务类
    /// </summary>
    public class TestCaseGroupBLL
    {
        private TestCaseGroupService testCaseGroupService = new TestCaseGroupService();

        #region 获取数据
        public async Task<TData<List<TestCaseGroupEntity>>> GetList(TestCaseGroupListParam param)
        {
            TData<List<TestCaseGroupEntity>> obj = new TData<List<TestCaseGroupEntity>>();
            obj.Result = await testCaseGroupService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<TestCaseGroupEntity>>> GetPageList(TestCaseGroupListParam param, Pagination pagination)
        {
            TData<List<TestCaseGroupEntity>> obj = new TData<List<TestCaseGroupEntity>>();
            obj.Result = await testCaseGroupService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<TestCaseGroupEntity>> GetEntity(long id)
        {
            TData<TestCaseGroupEntity> obj = new TData<TestCaseGroupEntity>();
            obj.Result = await testCaseGroupService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(TestCaseGroupEntity entity)
        {
            TData<string> obj = new TData<string>();
            await testCaseGroupService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await testCaseGroupService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
