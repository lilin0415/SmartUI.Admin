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
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using Koo.Utilities.Exceptions;
using YiSha.Model;
using Koo.Utilities.Helpers;
using System.Diagnostics;
using YiSha.Service.Cache;
using Koo.Utilities.FileFormaKoo.RSSHelper;

namespace YiSha.Service.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-02-16 21:00
    /// 描 述：服务类
    /// </summary>
    public class ConfigService : BaseRepositoryService
    {
        #region 获取数据
      
        public async Task<List<ConfigEntity>> GetList(ConfigListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<ConfigEntity>> GetPageList(ConfigListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<ConfigEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<ConfigEntity>(id);
        }
        #endregion

        #region 提交数据
        private bool ExistCode(ConfigEntity entity)
        {
            var expression = LinqExtensions.True<ConfigEntity>();
            //expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.Category == entity.Category && t.Code==entity.Code);
            }
            else
            {
                expression = expression.And(t => t.Category == entity.Category && t.Code == entity.Code && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }

        public async Task SaveForm(ConfigEntity entity)
        {
            this.VerifyHasManagerPower();

            #region 验证
            entity.Category = "Default";

            if (string.IsNullOrWhiteSpace(entity.Code))
            {
                throw new ArgumentIsEmptyException("编码不能为空");
            }
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new ArgumentIsEmptyException("名称不能为空");
            }

            if (this.ExistCode(entity))
            {
                throw new DuplicationDataExection("编码已经存在");
            }

            #endregion

            if (entity.Id.IsNullOrZero())
            {
                entity.Create();
                this.VerifyIsMyDataOnCreate<ConfigEntity>(entity);
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify();
                this.VerifyIsMyDataOnModify<ConfigEntity>(entity);
                await this.BaseRepository().Update(entity);
            }

            this.ClearCache();
        }
    
        public async Task SaveForm(SystemConfigModel entity,string category)
        {
            this.VerifyHasManagerPower();

            var expression = ListFilter(null);
            var list = await this.BaseRepository().FindList(expression);
            var itemsInDb = list.ToList();

            var itemsToUpdate = new List<ConfigEntity>();
            var itemsToInsert = new List<ConfigEntity>();

            var properties = TypeHelper.GetProperties(typeof(SystemConfigModel));
            
            foreach (var property in properties)
            {
                var categoryAttribute = TypeHelper.GetAttribute<SystemConfigCategoryAttribute>(property);
                if (categoryAttribute != null && category == categoryAttribute.Category)
                {
                    var propertyValue = TypeHelper.GetPropertyValue(entity, property);

                    var existedItem = itemsInDb.FirstOrDefault(x => x.Code == property.Name);
                    if (existedItem != null)
                    {
                        if (existedItem.Name == property.Name)
                        {
                            existedItem.Name = DescriptionHelper.GetDescription(property);
                        }
                        existedItem.Val = propertyValue?.ToString();
                        itemsToUpdate.Add(existedItem);
                    }
                    else
                    {
                        existedItem = new ConfigEntity();
                        existedItem.Code= property.Name;
                        existedItem.Name = DescriptionHelper.GetDescription(property);
                        existedItem.Val = propertyValue?.ToString();
                        itemsToInsert.Add(existedItem);
                    }
                }
            }

            var repo = this.BaseRepository();
            var trans = await repo.BeginTrans();
            try
            {
                foreach (var item in itemsToInsert)
                {
                    item.Create();
                    await trans.Insert(item);
                }

                foreach (var item in itemsToUpdate)
                {
                    item.Modify();
                    await trans.Update(item);
                }
               
                await trans.CommitTrans();

                this.ClearCache();
            }
            catch (Exception ex)
            {
                await trans.RollbackTrans();
                throw ex;
            }
        }
        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<ConfigEntity>(idArr);
           
            this.ClearCache();
        }
        #endregion

        #region 私有方法
        private Expression<Func<ConfigEntity, bool>> ListFilter(ConfigListParam param)
        {
            var expression = LinqExtensions.True<ConfigEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion

        public void ClearCache()
        {
            ConfigCache cache = new ConfigCache();
            cache.Remove();
        }
    }
}
