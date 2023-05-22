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
using YiSha.Entity.DeviceManager;
using YiSha.Model.Param.DeviceManager;
using Koo.Utilities.Exceptions;
using YiSha.Entity.ProductCategoryManager;
using YiSha.Service.ProductCategoryManager;
using YiSha.Enum;
using Koo.Utilities.Helpers;

namespace YiSha.Service.DeviceManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-04-22 09:41
    /// 描 述：服务类
    /// </summary>
    public class DeviceGroupService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<DeviceGroupEntity>> GetList(DeviceGroupListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<DeviceGroupEntity>> GetPageList(DeviceGroupListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<DeviceGroupEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DeviceGroupEntity>(id);
        }
        #endregion

        public bool ExistName(DeviceGroupEntity entity)
        {
            //return false;

            var expression = CreateFilter<DeviceGroupEntity>();
            //expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.Name == entity.Name);
            }
            else
            {
                expression = expression.And(t => t.Name == entity.Name && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
      
        public async Task<long> SaveForm(DeviceGroupEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new DataInvalidException("名称不能为空");
            }
            if (ExistName(entity))
            {
                throw new DuplicationDataExection();
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
        #region 提交数据
      
        public async Task DeleteForm(string ids)
        {
            this.VerifyIsMyDataOnDelete<DeviceGroupEntity>(ids);

            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            if (idArr.Any())
            {
                var hasBeenUsed = await CheckHasBeenUsed(ids);

                if (hasBeenUsed)
                {
                    throw new ForbidDeleteExection("此客户端组已被使用不可删除");
                }

                await this.BaseRepository().Delete<DeviceGroupEntity>(idArr);
                await this.BaseRepository().Delete<DeviceGroupDetailEntity>(x => idArr.Contains(x.GroupId.Value));
            }
            else
            {
               
            }

        }
        #endregion

        public async Task<bool> CheckHasBeenUsed(string ids)
        {
            var sql = $"select count(1) from testtask where ConsumeMode={(int)TaskConsumeModeEnumType.ClientGroup} and ConsumerId in ({ids})";
            var hasBeenUsed = await BaseRepository().GetCountValue(sql);

            if (hasBeenUsed > 0)
            {
                return true;
            }

            sql = $"select count(1) from taskexecrecord where ConsumeMode={(int)TaskConsumeModeEnumType.ClientGroup} and ConsumerId in ({ids})";
            hasBeenUsed = await BaseRepository().GetCountValue(sql);

            if (hasBeenUsed > 0)
            {
                return true;
            }

            return false;
        }

        #region 私有方法
        private Expression<Func<DeviceGroupEntity, bool>> ListFilter(DeviceGroupListParam param)
        {
            var expression = LinqExtensions.True<DeviceGroupEntity>();
            if (param != null)
            {
                if (SqlStringHelper.IsSafeSqWhere(param.Name))
                {
                    expression = expression.And(x => x.Name == param.Name);
                }
            }
            return expression;
        }
        #endregion
    }
}
