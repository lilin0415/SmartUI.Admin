using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using System.Linq.Expressions;
using YiSha.Web.Code;
using Koo.Utilities.Exceptions;
using NPOI.POIFS.FileSystem;
using System.Text;
using Koo.Utilities.Helpers;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace YiSha.Service.SystemManage
{
    public class DataDictDetailService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<DataDictDetailEntity>> GetList(DataDictDetailListParam param)
        {
            var sql = ListFilter(param);
            var list = await this.BaseRepository().FindList<DataDictDetailEntity>(sql);
            return list.OrderBy(p => p.DictSort).ToList();
        }

        public async Task<List<DataDictDetailEntity>> GetPageList(DataDictDetailListParam param, Pagination pagination)
        {
            var sql = ListFilter(param);
            pagination.TotalCount = await this.BaseRepository().GetCount(sql);
            var list = await this.BaseRepository().FindList<DataDictDetailEntity>(sql, pagination);
            return list.list.ToList();
        }

        public async Task<DataDictDetailEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DataDictDetailEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(DictSort) FROM sysdatadictdetail");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

     
        #endregion

        #region 提交数据
        private async Task VerifyCanAddItem(DataDictDetailEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {

                //添加明细的时候需要验证是否 允许添加
                var dictService = new DataDictService();
                var dictEntity = await dictService.GetEntityByType(entity.DictType);
                if (dictEntity.CanAddItem == 0)
                {
                 
                }
            }

        }
        private async Task VerifyExistDictKeyValue(DataDictDetailEntity entity)
        {
            var user = this.GetCurrentUser();

           
            var expression = LinqExtensions.True<DataDictDetailEntity>();
         
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.DictType == entity.DictType && (t.DictKey == entity.DictKey || t.DictValue == entity.DictValue));
            }
            else
            {
                expression = expression.And(t => t.DictType == entity.DictType && (t.DictKey == entity.DictKey || t.DictValue == entity.DictValue) && t.Id != entity.Id);
            }
            var ret = this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
            if (ret)
            {
                throw new DuplicationDataExection("字典键或值已经存在");
            }


            {
             
                expression = LinqExtensions.True<DataDictDetailEntity>();
                expression = expression.And(t => t.BaseIsDelete == 0);
                if (entity.Id.IsNullOrZero())
                {
                    expression = expression.And(t => t.DictType == entity.DictType && (t.DictKey == entity.DictKey || t.DictValue == entity.DictValue));
                }
                else
                {
                    expression = expression.And(t => t.DictType == entity.DictType && (t.DictKey == entity.DictKey || t.DictValue == entity.DictValue) && t.Id != entity.Id);
                }
                ret = this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
                if (ret)
                {
                    throw new DuplicationDataExection("字典键或值已经存在");
                }
            }
        }
        public async Task SaveForm(DataDictDetailEntity entity)
        {
            if (entity.DictKey <= 0)
            {
                throw new BizException("字典键必须大于0");
            }
            if (string.IsNullOrWhiteSpace(entity.DictValue))
            {
                throw new BizException("字典值不能为空");
            }

            await this.VerifyCanAddItem(entity);

            await this.VerifyExistDictKeyValue(entity);

            if (entity.Id.IsNullOrZero())
            {
             
                entity.Create();
                await this.BaseRepository().Insert<DataDictDetailEntity>(entity);
            }
            else
            {
                entity.Modify();
                await this.BaseRepository().Update<DataDictDetailEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            if (!idArr.Any())
            {
                return;
            }

            var sql = $"select IsSystem from {DataDictDetailEntity._TblName} where Id in ({string.Join(",", idArr)}) ";
            var isSystemList = await this.BaseRepository().GetList<int>(sql);
            if (isSystemList.Any(x => x == 1))
            {
                throw new ForbidDeleteExection("系统数据禁止删除");
            }

            await this.BaseRepository().Delete<DataDictDetailEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private string ListFilter(DataDictDetailListParam param)
        {
            var info = this.GetCurrentUser();

            var sb = new StringBuilder();
            sb.Append($"select * from {DataDictDetailEntity._TblName} where 1=1 ");
         
            if (param != null)
            {
                if (param.DictKey.ParseToInt() > 0)
                {
                    sb.Append($" and DictKey = {param.DictKey} ");
                }

                if (SecurityHelper.IsSafeSqlParam(param.DictValue))
                {
                    sb.Append($" and DictKey like '%{param.DictValue}%' ");
                }

                if (SecurityHelper.IsSafeSqlParam(param.DictType))
                {
                    sb.Append($" and DictType like '%{param.DictType}%' ");
                }

            }
            return sb.ToString();

        }
        #endregion
    }
}
