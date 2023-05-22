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
using YiSha.Entity.SystemManage;
using Koo.Utilities.Exceptions;
using Koo.Utilities.Helpers;
using YiSha.Entity.TestTaskManager;

namespace YiSha.Service.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:55
    /// 描 述：服务类
    /// </summary>
    public class ExecEnvironmentService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<ExecEnvironmentEntity>> GetList(ExecEnvironmentListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<ExecEnvironmentEntity>> GetPageList(ExecEnvironmentListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<ExecEnvironmentEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<ExecEnvironmentEntity>(id);
        }
        #endregion

        #region 保存数据
        private bool ExistName(ExecEnvironmentEntity entity)
        {
            var expression = CreateFilter<ExecEnvironmentEntity>();
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
        private bool ExistCode(ExecEnvironmentEntity entity)
        {
            var expression = CreateFilter<ExecEnvironmentEntity>();
            //expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.Code == entity.Code);
            }
            else
            {
                expression = expression.And(t => t.Code == entity.Code && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        public async Task SaveForm(ExecEnvironmentEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Code))
            {
                throw new ArgumentIsEmptyException("编码不能为空");
            }
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new ArgumentIsEmptyException("名称不能为空");
            }
            entity.Code = entity.Code.Trim();
            entity.Name= entity.Name.Trim();
            //if (this.ExistCode(entity))
            //{
            //    throw new DuplicationDataExection("已经存在相同的编码");
            //}
            //if (this.ExistName(entity))
            //{
            //    throw new DuplicationDataExection("已经存在相同的名称");
            //}
		
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

        #endregion

        #region 删除
        private bool HasBeenUsed(long[] envIds)
        {
            //查询任务、用例中是否有使用此
            var expression = LinqExtensions.True<TestCaseEntity>();
            //expression = expression.And(t => t.BaseIsDelete == 0);
            expression = expression.And(t => envIds.Contains(t.EnvId.GetValueOrDefault()));
            var ret = this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
            if (ret)
            {
                return true;
            }
            else
            {
                var expression2 = LinqExtensions.True<TestCaseEntity>();
                //expression = expression.And(t => t.BaseIsDelete == 0);
                expression2 = expression2.And(x => envIds.Contains(x.EnvId.GetValueOrDefault()));
                var ret2 = this.BaseRepository().IQueryable(expression2).Count() > 0 ? true : false;
                return ret2;
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            if (HasBeenUsed(idArr))
            {
                throw new ForbidDeleteExection("数据已被使用，禁止删除");
            }

            this.VerifyIsMyDataOnDelete<ExecEnvironmentEntity>(ids);
            await this.BaseRepository().Delete<ExecEnvironmentEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<ExecEnvironmentEntity, bool>> ListFilter(ExecEnvironmentListParam param)
        {
            var expression = CreateFilter<ExecEnvironmentEntity>();
            if (param != null)
            {
                if (SecurityHelper.IsSafeSqlParam(param.Name))
                {
                    expression = expression.And(t => t.Name.Contains(param.Name));
                }
            }
            return expression;
        }
        #endregion
    }
}
