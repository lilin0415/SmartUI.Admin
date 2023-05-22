using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Web.Code;
using NPOI.POIFS.FileSystem;

namespace YiSha.Service.SystemManage
{
    public class AreaService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<AreaEntity>> GetList(AreaListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AreaEntity>> GetPageList(AreaListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AreaEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AreaEntity>(id);
        }

        public async Task<AreaEntity> GetEntityByAreaCode(string areaCode)
        {
            return await this.BaseRepository().FindEntity<AreaEntity>(p => p.AreaCode == areaCode);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(AreaEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                entity.Create();
                TenantHelper.VerifyIsMyDataOnCreate(entity);

                await this.BaseRepository().Insert<AreaEntity>(entity);
            }
            else
            {
                entity.Modify();
                TenantHelper.VerifyIsMyData(entity);
                await this.BaseRepository().Update<AreaEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            TenantHelper.VerifyIsMyData<AreaEntity>(ids);

            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<AreaEntity>(idArr);
        }

        #endregion

        #region 私有方法
        private Expression<Func<AreaEntity, bool>> ListFilter(AreaListParam param)
        {
            var expression = LinqExtensions.True<AreaEntity>();
            if (param != null)
            {
                if (!param.AreaName.IsEmpty())
                {
                    expression = expression.And(t => t.AreaName.Contains(param.AreaName));
                }
            }
            return expression;
        }
        #endregion
    }
}
