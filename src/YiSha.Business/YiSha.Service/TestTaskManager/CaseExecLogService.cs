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
using YiSha.Entity.TestTaskManager;
using YiSha.Model.Param.TestTaskManager;
using YiSha.Entity.OrganizationManage;

namespace YiSha.Service.TestTaskManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-03-25 19:49
    /// 描 述：服务类
    /// </summary>
    public class CaseExecLogService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<CaseExecLogEntity>> GetList(CaseExecLogListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<CaseExecLogEntity>> GetPageList(CaseExecLogListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<CaseExecLogEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<CaseExecLogEntity>(id);
        }
        public async Task<CaseExecLogEntity> GetStartEntity(long caseExecId,string pathmd5)
        {
            var expression = LinqExtensions.True<CaseExecLogEntity>();
            expression = expression.And(t => t.CaseExecId == caseExecId);
            expression = expression.And(t => t.ExecutionPathMd5 == pathmd5);
            expression = expression.And(t => t.TransStep == (int)TransStep.Start);
            return await this.BaseRepository().FindEntity<CaseExecLogEntity>(expression);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(CaseExecLogEntity entity)
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
            await this.BaseRepository().Delete<CaseExecLogEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<CaseExecLogEntity, bool>> ListFilter(CaseExecLogListParam param)
        {
            var expression = LinqExtensions.True<CaseExecLogEntity>();
            if (param != null)
            {
                if (param.CaseExecId.HasValue)
                {
                    expression = expression.And(x => x.CaseExecId == param.CaseExecId.Value);
                }
            }
            return expression;
        }
        #endregion
    }
}
