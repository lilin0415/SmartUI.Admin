using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.TestCaseManager;
using YiSha.Model.Param.TestCaseManager;

namespace YiSha.Service.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:14
    /// 描 述：服务类
    /// </summary>
    public class TestCaseParameterService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<TestCaseParameterEntity>> GetList(TestCaseListParameterListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<TestCaseParameterEntity>> GetPageList(TestCaseListParameterListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<TestCaseParameterEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<TestCaseParameterEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(TestCaseParameterEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify();
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<TestCaseParameterEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<TestCaseParameterEntity, bool>> ListFilter(TestCaseListParameterListParam param)
        {
            var expression = LinqExtensions.True<TestCaseParameterEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
