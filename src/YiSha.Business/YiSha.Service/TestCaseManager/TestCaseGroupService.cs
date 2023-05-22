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
    /// 日 期：2022-10-04 16:17
    /// 描 述：服务类
    /// </summary>
    public class TestCaseGroupService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<TestCaseGroupEntity>> GetList(TestCaseGroupListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<TestCaseGroupEntity>> GetPageList(TestCaseGroupListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<TestCaseGroupEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<TestCaseGroupEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(TestCaseGroupEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                entity.Create();

                await VerifyIsMyDataAndInsert(entity);
            }
            else
            {
                entity.Modify();

                await VerifyIsMyDataAndUpdate(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            this.VerifyIsMyDataOnDelete<TestCaseGroupEntity>(ids);
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<TestCaseGroupEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<TestCaseGroupEntity, bool>> ListFilter(TestCaseGroupListParam param)
        {
            var expression = CreateFilter<TestCaseGroupEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
