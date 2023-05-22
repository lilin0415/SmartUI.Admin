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
using YiSha.Model.Result;
using YiSha.Entity.ProjectManager;
using YiSha.Service.ProductCategoryManager;
using YiSha.Service.ProjectManager;
using Koo.Utilities.Helpers;
using YiSha.Enum;
using YiSha.Model.TestTaskManager;
using YiSha.Model.TestCaseManager;
using Koo.Utilities.Exceptions;
using Koo.Utilities.Data;
using NPOI.POIFS.FileSystem;
using YiSha.Entity.TestTaskManager;
using YiSha.Entity.ProductCategoryManager;

namespace YiSha.Service.TestCaseManager
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-01 20:13
    /// 描 述：服务类
    /// </summary>
    public class TestCaseService : BaseRepositoryService
    {
        #region 获取数据

        /// <summary>
        /// 获取用例模板树
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ZtreeInfo>> GetAllListAsTree(bool appendProject = true,bool appendModule = true)
        {
            List<ZtreeInfo> list = new List<ZtreeInfo>();

            var expression = CreateFilter<TestCaseEntity>();
            //用例
            var projectList = await this.BaseRepository().FindList(expression);
            foreach (var item in projectList)
            {
                var firstProject = item;
                list.Add(new ZtreeInfo
                {
                    id = firstProject.Id.ToString(),
                    pId = firstProject.ProjectGuid.ToString(),//用例所属的模板
                    name = $"[{GlobalContext.SystemConfig.CaseName}] " + firstProject.Name,
                    nodeType = ZtreeInfoNodeType.@case.ToString(),
                    tag = item,
                    Obj = item,
                    
                });
            }

            if (appendProject)
            {
                var moduleService = new PublishedProjectService();
                var moduleList = await moduleService.GetAllProjectTree(appendModule);
                list.AddRange(moduleList);
            }
            return list;

        }
        public async Task<List<ZtreeInfo>> GetAllListAsTree(long?envId,bool appendProject = true, bool appendModule = true)
        {
            List<ZtreeInfo> list = new List<ZtreeInfo>();

            var expression = CreateFilter<TestCaseEntity>();
            if (envId.HasValue)
            {
                expression = expression.And(x => x.EnvId == envId);
            }
            //用例
            var projectList = await this.BaseRepository().FindList(expression);
            foreach (var item in projectList)
            {
                var firstProject = item;
                list.Add(new ZtreeInfo
                {
                    id = firstProject.Id.ToString(),
                    pId = firstProject.ProjectGuid.ToString(),//用例所属的模板
                    name = $"[{GlobalContext.SystemConfig.CaseName}] " + firstProject.Name,
                    nodeType = ZtreeInfoNodeType.@case.ToString(),
                    tag = item,
                    Obj = item,

                });
            }

            if (appendProject)
            {
                var moduleService = new PublishedProjectService();
                var moduleList = await moduleService.GetAllProjectTree(appendModule);
                list.AddRange(moduleList);
            }
            return list;

        }
        public async Task<List<TestCaseEntity>> GetList(TestCaseListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<TestCaseModel>> GetPageList(TestCaseListParam param, Pagination pagination)
        {
            var expression = ListFilterSql(param);
            pagination.TotalCount = await this.BaseRepository().GetCount(expression);
            var list= await this.BaseRepository().FindList<TestCaseModel>(expression, pagination);

            var modelList = list.list.ToList();

           
            var envSerice = new ExecEnvironmentService();
            var envList = await envSerice.GetList(new ExecEnvironmentListParam());
            foreach (var model in modelList)
            {
                model.EnvDisplayName = envList.FirstOrDefault(x => x.Id == model.EnvId)?.Name;
                var usingVersionEnum = DataConverter.ToEnum<UsingVersionEnumType>(model.UsingVersion);
                model.UsingVersionDisplayName = DescriptionHelper.GetDescription(usingVersionEnum);
            }
            return modelList;
        }

        public async Task<TestCaseEntity> GetEntityForEdit(long? id,long? productId,long? cateId)
        {
            TestCaseEntity ret = null;

            if (id.HasValue && id.Value > 0)
            {
                //修改
                ret = await this.BaseRepository().FindEntity<TestCaseEntity>(id.Value);
                if (ret == null)
                {
                    throw new DataNotExistedException($"此{GlobalContext.SystemConfig.CaseName}不存在");
                }

                ret.HasBeenUsed = CheckHasBeenUsed(id.Value);

                var projectService = new PublishedProjectService();
                var projectEntity = await projectService.GetEntityByGuidAndVersion(ret.ProjectGuid, ret.SpecialVersion);
                ret.ProjectName = projectEntity.Name;

                productId = ret.ProductId;
                cateId = ret.CateId;
            }
            else
            {
                //新建的时候，默认使用当前选择的产品及分类
                ret = new TestCaseEntity();
                ret.ProductId = productId;
                ret.CateId = cateId;
                ret.IsEnable = 1;
            }

            var pathNames = new List<string>();
            var moduleService = new ModuleCategoryService();
            var pathArray = await moduleService.GetPath(cateId.GetValueOrDefault(), true);
            foreach (var pathItem in pathArray)
            {
                if (pathItem is ProductEntity p)
                {
                    pathNames.Add(p.Name);
                }
                else if (pathItem is ModuleCategoryEntity m)
                {
                    pathNames.Add(m.Name);
                }
            }
            ret.ProductCateFullName = string.Join("->", pathNames);

            return ret;
        }

        public async Task<TestCaseEntity> GetEntity(long id)
        {
            var ret = await this.BaseRepository().FindEntity<TestCaseEntity>(id);
            
            return ret;
        }
        #endregion

        private bool CheckHasBeenUsed(long id)
        {
            var expression = CreateFilter<TestTaskItemEntity>().And(t => t.CaseId == id);
            var ret= this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
            if (!ret)
            {
                var e2 = CreateFilter<CaseExecRecordEntity>().And(x => x.CaseId == id);
                ret = this.BaseRepository().IQueryable(e2).Count() > 0 ? true : false;
            }

            return ret;
        }

        #region 保存用例
        private bool ExistCode(TestCaseEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Code))
            {
                throw new ArgumentIsEmptyException("编码不能为空");
                return false;
            }
            entity.Code = entity.Code.Trim();

            var expression = CreateFilter<TestCaseEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
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

        public async Task SaveForm(TestCaseEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new ArgumentIsEmptyException("名称不能为空");
            }
            entity.Name= entity.Name.Trim();

            if (this.ExistCode(entity))
            {
                throw new DuplicationDataExection("已经存在相同的编码");
            }
            

            //entity.ProjectGuid
            var productGuid = SecurityHelper.ToSafeSqlParam(entity.ProjectGuid);
            var productVersion = SecurityHelper.ToSafeSqlParam(entity.SpecialVersion);

            var sql = $"select IsEnable Value from publishedproject a" +
                $"     where a.ProjectGuid = '{productGuid}' and a.Version='{productVersion}'";

            var items = await this.BaseRepository().GetList<int>(sql);
            if (items.Any())
            {
                if (entity.IsEnable == 1)
                {
                    if (items.First() == 0)
                    {
                        throw new DataInvalidException("当前模板的版本号已禁用");
                    }
                }
                else
                {
                    //如果禁用当前用例，任务计划需要禁用吗
                    //应该是不需要禁用，在执行计划任务的时候，如果发现机器人已禁用，则直接取消此机器人执行
                    //对于未执行的
                }
            }
            else
            {
                throw new DataNotExistedException("不存在此模板或者相应的版本号");
            }

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
        public async Task DeleteForm(string ids)
        {
            ids = SecurityHelper.ToSafeSqlIds(ids);
            if (!string.IsNullOrEmpty(ids))
            {
                this.VerifyIsMyDataOnDelete<TestCaseEntity>(ids);

                var sql = $"(" +
                    $"      select a.Name from testcase a " +
                    $"          inner join testtaskitem b on a.Id = b.CaseId " +
                    $"      where a.Id in ({ids})" +
                    $")";
                sql += " union (";
                sql += $"" +
                    $"      select a.Name from testcase a " +
                    $"          inner join caseexecrecord b on a.Id = b.CaseId " +
                    $"      where a.Id in ({ids})" +
                    $")";
                var names = await this.BaseRepository().GetList<string>(sql);
                if (names.Any())
                {
                    throw new ForbidDeleteExection($"{GlobalContext.SystemConfig.CaseName}:{string.Join(",", names)} 已在[计划管理]中使用，不可删除");
                }

                long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
                await this.BaseRepository().Delete<TestCaseEntity>(idArr);

            }

        }
        #endregion

        #region 私有方法
        private Expression<Func<TestCaseEntity, bool>> ListFilter(TestCaseListParam param)
        {
            var expression = CreateFilter<TestCaseEntity>();
            if (param != null)
            {
                if (param.ProductId.HasValue)
                {
                    expression = expression.And(t => t.ProductId == param.ProductId.Value);

                    if (param.CateId.HasValue)
                    {
                        expression = expression.And(t => t.CateId == param.CateId.Value);
                    }
                }

               
                if (SecurityHelper.IsSafeSqlParam(param.Code))
                {
                    expression = expression.And(t => t.Name.Contains(param.Code));
                }

                if (SecurityHelper.IsSafeSqlParam(param.Name))
                {
                    expression = expression.And(t => t.Name.Contains(param.Name));
                }
                if (param.EnvId.HasValue)
                {
                    expression = expression.And(t => t.EnvId == param.EnvId.Value);
                }
                //else
                //{
                //    expression = expression.And(t => false);
                //}
                if (SecurityHelper.IsSafeSqlParam(param.ProjectGuid))
                {
                    expression = expression.And(t => t.ProjectGuid == param.ProjectGuid);
                }

            }
            return expression;
        }
        private string  ListFilterSql(TestCaseListParam param)
        {
            var sql = $"select a.*, b.Name ProjectName,b.AssertionCount,b.SupportParallel from testcase a ";
            sql += " inner join publishedproject b on a.ProjectGuid=b.ProjectGuid and a.SpecialVersion=b.Version ";
            sql += " where 1=1 ";

            if (param != null)
            {
                if (param.ProductId.HasValue)
                {
                    sql += $" and a.ProductId ={param.ProductId.Value} ";

                    if (param.CateId.HasValue)
                    {
                        sql += $" and a.CateId ={param.CateId.Value} ";
                    }
                }


                if (SecurityHelper.IsSafeSqlParam(param.Code))
                {
                    sql += $" and a.Code like '%{param.Code}%' ";
                }

                if (SecurityHelper.IsSafeSqlParam(param.Name))
                {
                    sql += $" and a.Name like '%{param.Name}%' ";
                }
                if (param.EnvId.HasValue)
                {
                    if (param.SearchSource == "QueryForTaskItems")
                    {
                        sql += $" and a.EnvId in ({param.EnvId.Value},0) ";
                    }
                    else
                    {
                        sql += $" and a.EnvId ={param.EnvId.Value} ";
                    }
                    
                }
                if (SecurityHelper.IsSafeSqlParam(param.ProjectGuid))
                {
                    sql += $" and a.ProjectGuid ='{param.ProjectGuid}' ";
                }
                if (SecurityHelper.IsSafeSqlParam(param.ProjectName))
                {
                    sql += $" and b.Name like '%{param.ProjectName}%' ";
                }
                if (param.IsEnable.HasValue)
                {
                    sql += $" and a.IsEnable ={param.IsEnable.Value} ";
                }
            }
            return sql;
        }
        #endregion

        #region 升级指定用例的模板版本
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="projectVersion"></param>
        /// <returns></returns>
        public async Task UpgradeVersionJson(string ids, string projectVersion)
        {
            ids = SecurityHelper.ToSafeSqlIds(ids);
            if (!string.IsNullOrEmpty(ids))
            {
                this.VerifyIsMyDataOnDelete<TestCaseEntity>(ids);

                var sql = $"update testcase ";
                sql += $" set SpecialVersion ='{projectVersion}' ";
                sql += $" where Id in ({ids})";

                await this.BaseRepository().ExecuteBySql(sql);
            }
        }

        #endregion
    }
}
