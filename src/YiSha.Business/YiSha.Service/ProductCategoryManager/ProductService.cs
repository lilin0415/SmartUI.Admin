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
using YiSha.Entity.ProductCategoryManager;
using YiSha.Model.Param.ProductCategoryManager;
using YiSha.Model.Result;
using YiSha.Entity.SystemManage;
using Koo.Utilities.Exceptions;
using YiSha.Model.ProductCategoryManage;

namespace YiSha.Service.ProductCategoryManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-23 23:04
    /// 描 述：服务类
    /// </summary>
    public class ProductService : BaseRepositoryService
    {
        #region 获取数据

        public async Task<List<ZtreeInfo>> GetAllProductTree()
        {
            var expression = CreateFilter<ProductEntity>();
            var list = new List<ZtreeInfo>();

            var obj = await this.BaseRepository().FindList(expression);
            foreach (var product in obj)
            {
                list.Add(new ZtreeInfo
                {
                    id = product.Id.ToString(),
                    pId = product.ParentId.ToString(),
                    name = product.DisplayName,
                    nodeType = ZtreeInfoNodeType.product.ToString(),
                    tag = product,
                    Obj = product,
                });
            }
            return list;
        }


public async Task<List<ProductEntity>> GetList(ProductListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<ProductEntity>> GetPageList(ProductListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<ProductEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<ProductEntity>(id);
        }
        #endregion


        #region 保存产品
        public bool ExistName(ProductEntity entity)
        {
            //return false;

            var expression = CreateFilter<ProductEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
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

        public async Task<long> SaveForm(ProductEntity entity)
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
        #endregion

        #region 删除数据
        public async Task DeleteForm(string ids)
        {
            this.VerifyIsMyDataOnDelete<ProductEntity>(ids);

            var moduleService = new ModuleCategoryService();
            var hasModules = await moduleService.HasModuleByProductId(ids);
            if (hasModules)
            {
                throw new ForbidDeleteExection("请先删除产品中包含的功能模块");
            }

            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<ProductEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<ProductEntity, bool>> ListFilter(ProductListParam param)
        {
            var expression = CreateFilter<ProductEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
