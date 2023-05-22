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
using YiSha.Entity.ProjectManager;
using YiSha.Model.Param.ProjectManager;
using YiSha.Model.Param.TestCaseManager;
using YiSha.Model.Result;
using YiSha.Service.TestCaseManager;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using YiSha.Service.ProductCategoryManager;
using YiSha.Entity.ProductCategoryManager;
using YiSha.Model.Param.ProductCategoryManager;
using NPOI.POIFS.FileSystem;
using Koo.Utilities.Exceptions;
using Koo.Utilities.Helpers;
using Koo.Utilities.Data;
using Org.BouncyCastle.Crypto;

namespace YiSha.Service.ProjectManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-09-26 22:37
    /// 描 述：服务类
    /// </summary>
    public class PublishedProjectService : BaseRepositoryService
    {
        #region 获取数据

        /// <summary>
        /// 获取用例模板树
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ZtreeInfo>> GetAllProjectTree(bool appendModuleCategory=true)
        {
            List<ZtreeInfo> list = new List<ZtreeInfo>();

            var expression = CreateFilter<PublishedProjectEntity>();

            //用例模板（项目）
            var projectList = await this.BaseRepository().FindList(expression);
            foreach (var item in projectList.GroupBy(x => x.ProjectGuid))
            {
                var firstProject = item.First();
                list.Add(new ZtreeInfo
                {
                    id = firstProject.ProjectGuid,
                    pId = firstProject.CateId.ToString(),
                    name = "[模板] " + firstProject.Name,
                    nodeType = ZtreeInfoNodeType.project.ToString(),
                    tag = firstProject,
                    Obj = item,
                });

            }
            if (appendModuleCategory)
            {
                var moduleService = new ModuleCategoryService();
                var moduleList = await moduleService.GetAllCateTree();
                list.AddRange(moduleList);
            }
            return list;

        }

        public async Task<List<PublishedProjectEntity>> GetList(PublishedProjectListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }
        public async Task<List<PublishedProjectEntity>> GetListByCategoryId(List<long> categoryIds)
        {
            if (!categoryIds.Any())
            {
                return new List<PublishedProjectEntity>();
            }

            var param = new PublishedProjectListParam();
           
            var expression = ListFilter(param);
            expression = expression.And(x => categoryIds.Contains(x.CateId.Value));

            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }
        public async Task<List<PublishedProjectEntity>> GetPageList(PublishedProjectListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<PublishedProjectEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<PublishedProjectEntity>(id);
        }
        public async Task<PublishedProjectEntity> GetEntityByGuidAndVersion(string guid,string version)
        {
            var expression = CreateFilter<PublishedProjectEntity>();
            expression = expression.And(x => x.ProjectGuid == guid && x.Version == version);

            var projectList = await this.BaseRepository().FindEntity(expression);

            return projectList;
        }
        //public async Task<PublishedProjectEntity> GetDownloadUrl(string guid, string version)
        //{
        //    var expression = LinqExtensions.True<PublishedProjectEntity>();
        //    expression = expression.And(x => x.ProjectGuid == guid && x.Version == version);

        //    var projectList = await this.BaseRepository().FindList(expression);

        //    return projectList.ToList().FirstOrDefault();
        //}
        public async Task<List<string>> GetVersionList(string guid)
        {
            var expression = CreateFilter<PublishedProjectEntity>();
            expression = expression.And(x => x.ProjectGuid == guid);

            var list = await this.BaseRepository().FindList(expression);
            var ret = list.Select(x => x.Version).Distinct().OrderByDescending(x=>x).ToList();

            return ret;
        }

        public async Task<List<NameValuePair>> GetNameListByGuid(params string[] guids)
        {
            if (guids == null || !guids.Any())
            {
                return new List<NameValuePair>();
            }
            var str = SqlStringHelper.QuoteString(guids);


            var sql = $"select a.Name,a.ProjectGuid Value from publishedproject a " +
                
                  $" where a.ProjectGuid in({str})";


            var list = await this.BaseRepository().GetList<NameValuePair>(sql);
           
            return list;
        }

        #endregion

        #region 提交数据
        public async Task<long> SaveForm(PublishedProjectEntity entity)
        {
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
        public async Task DisableForm(string ids,int status)
        {
            ids = SecurityHelper.ToSafeSqlIds(ids);
            if (!string.IsNullOrEmpty(ids))
            {
                this.VerifyIsMyDataOnDelete<PublishedProjectEntity>(ids);

                var toStatus = status == 1 ? 0 : 1;

                if (toStatus == 1)
                {
                    var sql = $"update publishedproject set IsEnable = 1 where Id in ({ids})";
                    await this.BaseRepository().ExecuteBySql(sql);
                }
                else
                {
                    var sql = $"select a.Name from publishedproject a " +
                   $"          inner join  testcase b " +
                   $"              on a.ProjectGuid= b.ProjectGuid and a.Version= b.SpecialVersion " +
                   $" where a.Id in ({ids}) and a.IsEnable = 1 and b.IsEnable = 1 ";

                    //可以禁用，可以认为以前的可以继续使用，后续不可以再新增了
                    //var names = await this.BaseRepository().GetList<string>(sql);
                    //if (names.Any())
                    //{
                    //    throw new ForbidUpdateExection($"模板:{string.Join(",", names)} 已在用例中使用，不可禁用。请先禁用相应用例");
                    //}

                    sql = $"update publishedproject set IsEnable = 0 where Id in ({ids})";
                    await this.BaseRepository().ExecuteBySql(sql);
                }
               
            }
        }

        public async Task DeleteForm(string ids)
        {
            ids = SecurityHelper.ToSafeSqlIds(ids);
            if (!string.IsNullOrEmpty(ids))
            {
                this.VerifyIsMyDataOnDelete<PublishedProjectEntity>(ids);

                var sql = $"select a.Name from publishedproject a " +
                    $"          inner join  testcase b " +
                    $"              on a.ProjectGuid= b.ProjectGuid and a.Version= b.SpecialVersion " +
                    $" where a.Id in({ids})";

                var names = await this.BaseRepository().GetList<string>(sql);
                if (names.Any())
                {
                    throw new ForbidDeleteExection($"模板:{string.Join(",", names)} 已在{GlobalContext.SystemConfig.CaseName}中使用，不可删除");
                }

                long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
                await this.BaseRepository().Delete<PublishedProjectEntity>(idArr);
            }
           
        }
        #endregion

        #region 私有方法
        private Expression<Func<PublishedProjectEntity, bool>> ListFilter(PublishedProjectListParam param)
        {
            var expression = CreateFilter<PublishedProjectEntity>();
            if (param != null)
            {
                if (param.ProductId.HasValue && param.ProductId.Value>0)
                {
                    expression = expression.And(t => t.ProductId == param.ProductId.Value);
                }
                if (param.CateId.HasValue && param.CateId.Value>0)
                {
                    expression = expression.And(t => t.CateId == param.CateId.Value);
                }
                if (SecurityHelper.IsSafeSqlParam(param.ProjectGuid))
                {
                    expression = expression.And(t => t.ProjectGuid.Contains(param.ProjectGuid));
                }

                if (SecurityHelper.IsSafeSqlParam(param.Name))
                {
                    expression = expression.And(t => t.Name.Contains(param.Name));
                }
                if (param.IsEnable.HasValue)
                {
                    expression = expression.And(t => t.IsEnable == (param.IsEnable.Value ? 1 : 0));
                }
            }
            return expression;
        }
        #endregion


        public async Task<string> Verify(PublishedProjectEntity entity)
        {
            var error = string.Empty;
            string guid = entity.ProjectGuid;
            string alignedVersion = entity.AlignedVersion;

            var expression = LinqExtensions.True<PublishedProjectEntity>();
            expression = expression.And(x => x.ProjectGuid == guid /*&& x.AlignedVersion== alignedVersion*/);

            var sql = $"select Version from {PublishedProjectEntity.TblName}";
            sql += $" where ProjectGuid='{guid}' order by BaseCreateTime desc";
            sql+=" limit 1";

            var strVersion = await this.BaseRepository().GetValue<string>(sql);
            if (string.IsNullOrWhiteSpace(strVersion))
            {
                return string.Empty;
            }
            else
            {
                if (entity.Version.Trim() == strVersion)
                {
                    error = "已存在相同的版本号";
                   
                }
                else
                {
                    if (Version.Parse(entity.Version) < Version.Parse(strVersion))
                    {
                        error = $"不能小于已有的版本号，当前最新版本号:{strVersion}";
                    }
                }
                return error;
            }
        }
    }
}
