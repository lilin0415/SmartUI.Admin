using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.ProjectManager;
using YiSha.Model.Param.ProjectManager;
using YiSha.Service.ProjectManager;
using YiSha.Business.ProductCategoryManager;
using YiSha.Entity.ProductCategoryManager;
using YiSha.Model.Param.ProductCategoryManager;
using YiSha.Model.Result;
using YiSha.Service.ProductCategoryManager;
using NPOI.SS.Formula.Functions;
using YiSha.Entity;

namespace YiSha.Business.ProjectManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-26 22:37
    /// 描 述：业务类
    /// </summary>
    public class PublishedProjectBLL
    {
        private PublishedProjectService publishedProjectService = new PublishedProjectService();

        #region 获取数据
        public async Task<TData<List<PublishedProjectEntity>>> GetList(PublishedProjectListParam param)
        {
            TData<List<PublishedProjectEntity>> obj = new TData<List<PublishedProjectEntity>>();
            obj.Result = await publishedProjectService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<PublishedProjectEntity>>> GetPageList(PublishedProjectListParam param, Pagination pagination)
        {
            TData<List<PublishedProjectEntity>> obj = new TData<List<PublishedProjectEntity>>();
            obj.Result = await publishedProjectService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<PublishedProjectEntity>> GetEntity(long id)
        {
            TData<PublishedProjectEntity> obj = new TData<PublishedProjectEntity>();
            obj.Result = await publishedProjectService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }

        /// <summary>
        /// 获取用例模板树
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TData<List<ZtreeInfo>>> GetPublishedProjectTree()
        {
           
            //用例模板（项目）
            var projectList = await publishedProjectService.GetAllProjectTree();


            return new TData<List<ZtreeInfo>>(projectList);
        }
        public async Task<TData<List<KeyValueEntity>>> GetVersionList(string guid)
        {
            var projectList = await publishedProjectService.GetVersionList(guid);

            var ret = new List<KeyValueEntity>();
            projectList.ForEach(x => {
                ret.Add(new KeyValueEntity() { Key = x, Value = x, });
            });

            return new TData<List<KeyValueEntity>>(ret);
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(PublishedProjectEntity entity)
        {
            TData<string> obj = new TData<string>();
            await publishedProjectService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }
        public async Task<TData> DisableForm(string ids,int status)
        {
            TData obj = new TData();
            await publishedProjectService.DisableForm(ids, status);
            obj.Status = true;
            return obj;
        }
        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await publishedProjectService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion

      
    }
}
