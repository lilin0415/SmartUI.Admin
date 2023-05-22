using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.ProductCategoryManager;
using YiSha.Model.Param.ProductCategoryManager;
using YiSha.Service.ProductCategoryManager;
using Microsoft.AspNetCore.Mvc;
using YiSha.Model.Result;

namespace YiSha.Business.ProductCategoryManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-23 23:04
    /// 描 述：业务类
    /// </summary>
    public class ProductBLL
    {
        private ProductService productService = new ProductService();

        #region 获取数据
        public async Task<TData<List<ProductEntity>>> GetList(ProductListParam param)
        {
            TData<List<ProductEntity>> obj = new TData<List<ProductEntity>>();
            obj.Result = await productService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<ProductEntity>>> GetPageList(ProductListParam param, Pagination pagination)
        {
            TData<List<ProductEntity>> obj = new TData<List<ProductEntity>>();
            obj.Result = await productService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeList(ProductListParam param)
        {

            var departmentList = await productService.GetAllProductTree();

            return new TData<List<ZtreeInfo>>(departmentList);
        }

        public async Task<TData<ProductEntity>> GetEntity(long id)
        {
            TData<ProductEntity> obj = new TData<ProductEntity>();
            obj.Result = await productService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }


        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(ProductEntity entity)
        {
            TData<string> obj = new TData<string>();
            await productService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await productService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
