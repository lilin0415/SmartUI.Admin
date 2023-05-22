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
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Result;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Koo.Utilities.Exceptions;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using Koo.Utilities.Helpers;
using YiSha.Service.ProjectManager;
using YiSha.Entity;

namespace YiSha.Service.ProductCategoryManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-24 14:57
    /// 描 述：服务类
    /// </summary>
    public class ModuleCategoryService : BaseRepositoryService
    {
        #region 获取数据
        /// <summary>
        /// 获取包含当前分类的 树的路径
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="appendProductTree"></param>
        /// <returns></returns>
        public async Task<List<BaseEntity>> GetPath(long currentId, bool appendProductTree)
        {
            var ret = new List<BaseEntity>();

            if (currentId == 0)
            {
                return ret;
            }

            //获取所有分类，并组装分类所对应的产品
            var expression = CreateFilter<ModuleCategoryEntity>();

            var list = await this.BaseRepository().FindList(expression);

            var currentEntity = list.FirstOrDefault(x => x.Id == currentId);
            while (currentEntity != null)
            {
                ret.Insert(0,currentEntity);

                //查找父节点
                currentEntity = list.FirstOrDefault(x => x.Id == currentEntity.ParentId);
            }

            if(appendProductTree && list.Any())
            {
                var productService = new ProductService();
                var product = await productService.GetEntity(list.First().ProductId.Value);

                ret.Insert(0, product);
            }

            return ret;
        }
        /// <summary>
        /// 获取模块树
        /// </summary>
        /// <returns></returns>
        public async Task<List<ZtreeInfo>> GetAllCateTree(bool appendProductTree=true)
        {
            var list = new List<ZtreeInfo>();

            //获取所有分类，并组装分类所对应的产品
            var expression = CreateFilter<ModuleCategoryEntity>();

            var departmentList = await this.BaseRepository().FindList(expression);
            var moduleTree = ZtreeHelper.GetZtreeList(departmentList, -1, 0, false);
            foreach (var item in moduleTree)
            {
                item.nodeType = ZtreeInfoNodeType.module.ToString();
                //如果当前是功能模块根分类，
                //指定父级为相应的产品
                if (item.pId == "0")
                {
                    var entity = item.Obj as ModuleCategoryEntity;
                    item.pId = entity.ProductId.ToString();
                }
                list.Add(item);
            }
            if (appendProductTree)
            {
                var productService = new ProductService();
                var productList = await productService.GetAllProductTree();
                list.AddRange(productList);
            }
            return list;
        }


        public async Task<List<ModuleCategoryEntity>> GetAllList()
        {
            var expression = CreateFilter<ModuleCategoryEntity>();
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<ModuleCategoryEntity>> GetList(ModuleCategoryListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }
        public async Task<bool> HasModuleByProductId(string productIds)
        {
            var expression = CreateFilter<ModuleCategoryEntity>();
            if (!string.IsNullOrEmpty(productIds))
            {
                var idArray = SecurityHelper.ToSafeSqlIdArray(productIds);

                expression = expression.And(t => idArray.Contains(t.ProductId.Value));
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;

        }
        public async Task<List<ModuleCategoryEntity>> GetPageList(ModuleCategoryListParam param, Pagination pagination)
        {

            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<ModuleCategoryEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<ModuleCategoryEntity>(id);
        }
        #endregion

        #region 获取所有子节点数据
        public async Task<List<ModuleCategoryEntity>> GetAllChildren(long? id,long? productId=null)
        {
            var expression = CreateFilter<ModuleCategoryEntity>();
            if (productId.HasValue)
            {
                expression = expression.And(t => t.ProductId == productId.Value);
            }

            var list = await this.BaseRepository().FindList<ModuleCategoryEntity>(expression);
            var items = list.ToList();

            var ret = new List<ModuleCategoryEntity>();

            GetAllChildrenRecursive(items, id, ret);

            return ret;
        }
        private void GetAllChildrenRecursive(List<ModuleCategoryEntity> items, long? id, List<ModuleCategoryEntity> ret)
        {
            var subItems = items.Where(x => x.ParentId == id);
            if (subItems.Any())
            {
                ret.AddRange(subItems);

                foreach (var item in subItems)
                {
                    GetAllChildrenRecursive(items, item.Id, ret);
                }
            }
        }
        #endregion

        private async Task VerifyParentId(ModuleCategoryEntity entity)
        {
            if (entity.Id.GetValueOrDefault()!=0 && entity.ParentId.GetValueOrDefault() != 0)
            {
                if (entity.ParentId == entity.Id)
                {

                    throw new DataInvalidException("不能选择自己作为上级菜单");
                }

                var parentEntity = await this.GetEntity(entity.ParentId.Value);
                //if (parentEntity.MenuType != (int)MenuTypeEnum.Directory)
                //{
                //    throw new BizException("上级菜单只能选择目录");
                //}
                if (entity.Id > 0)
                {
                    //修改节点的时候，需要查询当前节点下面的子节点
                    var children = await GetAllChildren(entity.Id);
                    if (children.Any(x => x.Id == entity.ParentId))
                    {
                        throw new DataInvalidException("不能选择下级数据作为上级");
                    }
                }
                else
                {
                    //新增的时候不需要
                }
            }
        }
        public async Task<long> SaveForm(ModuleCategoryEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new ArgumentIsEmptyException("名称不能为空");
            }
            entity.Name= entity.Name.Trim();

            await this.VerifyParentId(entity);

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

        #region 删除
        public async Task DeleteForm(string ids)
        {
            this.VerifyIsMyDataOnDelete<ModuleCategoryEntity>(ids);

            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');

            var projects = await (new PublishedProjectService()).GetListByCategoryId(idArr.ToList());
            if (projects.Any())
            {
                throw new ForbidDeleteExection($"当前模块下面存在{GlobalContext.SystemConfig.CaseName}模板，禁止删除");
            }

            foreach (var id in idArr)
            {
                var children = await GetAllChildren(id);
                if (children.Any())
                {
                    throw new ForbidDeleteExection("请先删除子节点");
                }
            }
           
            await this.BaseRepository().Delete<ModuleCategoryEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<ModuleCategoryEntity, bool>> ListFilter(ModuleCategoryListParam param)
        {

            var expression = CreateFilter<ModuleCategoryEntity>();
            if (param != null)
            {
                if (param.ProductId.HasValue)
                {
                    expression = expression.And(t => t.ProductId == param.ProductId.Value);
                }
                else
                {
                    expression = expression.And(t => false);
                }
            }
            return expression;
        }


        #endregion
    }
}
