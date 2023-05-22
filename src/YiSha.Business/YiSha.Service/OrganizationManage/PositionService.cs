using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Util;
using Koo.Utilities.Exceptions;
using YiSha.Enum.OrganizationManage;

namespace YiSha.Service.OrganizationManage
{
    public class PositionService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<PositionEntity>> GetList(PositionListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<PositionEntity>> GetPageList(PositionListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<PositionEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<PositionEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(PositionSort) FROM sysposition");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

        #endregion

        #region 提交数据

        private bool ExistPositionName(PositionEntity entity)
        {
            var expression = CreateFilter<PositionEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.PositionName == entity.PositionName);
            }
            else
            {
                expression = expression.And(t => t.PositionName == entity.PositionName && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        public async Task<long> SaveForm(PositionEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.PositionName))
            {
                throw new CanNotBeEmptyException("名称不能为空");
            }
            entity.PositionName = entity.PositionName.Trim();

            if (this.ExistPositionName(entity))
            {
                throw new DuplicationDataExection("职位名称已经存在");
            }

            if (entity.Id.IsNullOrZero())
            {
                entity.Create();

                return await VerifyIsMyDataAndInsert(entity);
            }
            else
            {
                entity.Modify();

                return await VerifyIsMyDataAndUpdate(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            this.VerifyIsMyDataOnDelete<PositionEntity>(ids);

            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            var trans = await base.BaseRepository().BeginTrans();
            try
            {
                await trans.Delete<PositionEntity>(idArr);
                await trans.Delete<UserBelongEntity>(t =>  idArr.Contains(t.BelongId.Value) && t.BelongType == UserBelongTypeEnum.Role.ParseToInt());
				await trans.CommitTrans();
            }
            catch
            {
                await trans.RollbackTrans();
            }
          
        }
        #endregion

        #region 私有方法
        private Expression<Func<PositionEntity, bool>> ListFilter(PositionListParam param)
        {
            var expression = CreateFilter<PositionEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.PositionName))
                {
                    expression = expression.And(t => t.PositionName.Contains(param.PositionName));
                }
                if (!string.IsNullOrEmpty(param.PositionIds))
                {
                    long[] positionIdArr = TextHelper.SplitToArray<long>(param.PositionIds, ',');
                    expression = expression.And(t => positionIdArr.Contains(t.Id.Value));
                }
            }
            return expression;
        }
        #endregion
    }
}
