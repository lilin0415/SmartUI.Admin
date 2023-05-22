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
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.Result;
using YiSha.Service.OrganizationManage;
using YiSha.Web.Code;
using YiSha.Entity;
using YiSha.Service;

namespace YiSha.Business.ProductCategoryManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-24 14:57
    /// 描 述：业务类
    /// </summary>
    public class ModuleCategoryBLL
    {
        private ModuleCategoryService moduleCategoryService = new ModuleCategoryService();

        #region 获取数据
        public async Task<TData<List<ModuleCategoryEntity>>> GetList(ModuleCategoryListParam param)
        {
            TData<List<ModuleCategoryEntity>> obj = new TData<List<ModuleCategoryEntity>>();
            obj.Result = await moduleCategoryService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<ModuleCategoryEntity>>> GetPageList(ModuleCategoryListParam param, Pagination pagination)
        {
            TData<List<ModuleCategoryEntity>> obj = new TData<List<ModuleCategoryEntity>>();
            obj.Result = await moduleCategoryService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<ModuleCategoryEntity>> GetEntity(long id)
        {
            TData<ModuleCategoryEntity> obj = new TData<ModuleCategoryEntity>();
            obj.Result = await moduleCategoryService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        /// <summary>
        /// 返回模板树
        /// </summary>
        /// <param name="param"></param>
        /// <param name="currentEntityId"></param>
        /// <returns></returns>
        public async Task<TData<List<ZtreeInfo>>> GetZtreeList(ModuleCategoryListParam param,long? currentEntityId)
        {
            param.Id = null;

            List<ModuleCategoryEntity> departmentList = await moduleCategoryService.GetList(param);

            var ret = ZtreeHelper.GetZtreeList(departmentList, -1, currentEntityId, true);
            ret.ForEach(x => x.nodeType = ZtreeInfoNodeType.module.ToString());

            return new TData<List<ZtreeInfo>>(ret);
        }
        /// <summary>
        /// 获取产品和模块分类树
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TData<List<ZtreeInfo>>> GetProductAndCateTree(ModuleCategoryListParam param)
        {
            param.Id = null;

            var ret = await moduleCategoryService.GetAllCateTree(true);
           
            return new TData<List<ZtreeInfo>>(ret);
        }

        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(ModuleCategoryEntity entity)
        {
            TData<string> obj = new TData<string>();
            await moduleCategoryService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await moduleCategoryService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
