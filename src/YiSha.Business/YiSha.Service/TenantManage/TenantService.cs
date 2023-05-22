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
using YiSha.Entity.TenantManage;
using YiSha.Model.Param.TenantManage;
using Koo.Utilities.Exceptions;
using NPOI.POIFS.FileSystem;
using YiSha.Entity.OrganizationManage;
using YiSha.Web.Code;
using Koo.Utilities.Helpers;
using YiSha.Entity.SystemManage;
using Newtonsoft.Json.Linq;
using YiSha.Enum.OrganizationManage;
using YiSha.Model.TenantManage;
using System.Diagnostics;
using YiSha.Enum;
using YiSha.IdGenerator;
using YiSha.Entity.ProductCategoryManager;
using NPOI.SS.Formula.Functions;

namespace YiSha.Service.TenantManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-26 22:00
    /// 描 述：服务类
    /// </summary>
    public class TenantService :  BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<TenantEntity>> GetList(TenantListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<TenantModel>> GetPageList(TenantListParam param, Pagination pagination)
        {
            var expression = ListFilterBySql(param);
            var list= await this.BaseRepository().FindList<TenantModel>(expression, pagination);
            return list.list.ToList();
        }

        public async Task<TenantEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<TenantEntity>(id);
        }
        public async Task<TenantEntity> GetEntity(string code,string name)
        {
            code = code?.Trim();
            name = name?.Trim();

            return await this.BaseRepository().FindEntity<TenantEntity>(x => !string.IsNullOrEmpty(code) && x.Code == code || !string.IsNullOrEmpty(name) && x.Name == name);
        }
        #endregion

        #region 提交数据


        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<TenantEntity>(idArr);
            Operator.Instance.CurrentInfo().RefreshTenant(null);
        }
        #endregion

        #region 私有方法
        private Expression<Func<TenantEntity, bool>> ListFilter(TenantListParam param)
        {
            var expression = LinqExtensions.True<TenantEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        private string ListFilterBySql(TenantListParam param)
        {
            var sql = $"select a.*, b.UserName as CreatorUserName " +
                $"      from {TenantEntity.TblName} a " +
                $"          inner join SysUser b on a.BaseCreatorId = b.Id ";
            return sql;
        }
        #endregion


        #region  修改我的租户
        public async Task SaveMyTenantInfoForm(TenantEntity entity)
        {
            var user = this.GetCurrentUser();
            if (entity.Id.GetValueOrDefault() == 0)
            {
                entity.Id = user.CurrentTenantId;
            }

            entity.Code = entity.Code.Trim();

            if (!SecurityHelper.IsSafeSqlParam(entity.Code))
            {
                throw new BizException("编码包含非法字符");
            }

            await VerifyExistCode(entity);

            if (!user.HasSystemRole)
            {
                if (entity.Id != user.CurrentTenantId)
                {
                    throw new ForbidOperationExection("只能修改自己的数据");
                }
            }
            entity.Modify();

            TenantHelper.VerifyIsMyData(entity,false);
            await this.BaseRepository().Update(entity);

            //重新加载
            Operator.Instance.CurrentInfo().RefreshTenant(entity.Id);
        }
        #endregion

        #region 创建租户
        public async Task SaveForm(TenantEntity entity)
        {
            await CreateTenant(entity);

            //if (entity.Id.IsNullOrZero())
            //{
               
            //}
            //else
            //{
            //    entity.Modify();
            //    await this.BaseRepository().Update(entity);
            //}

            //创建租户之后，自动切换到创建租户
            Operator.Instance.CurrentInfo().RefreshTenant(entity.Id);
        }
        private async Task<int> VerifyExceedMaxCount()
        {
            int maxCount = 1;

            var myTenantService = new MyTenantService();
            var myOwnTenantCount = await myTenantService.GetMyOwnTenantCount();
            //验证用户可创建的租户最大数量
            if (myOwnTenantCount > maxCount)
            {
                throw new BizException($"您最多只能创建{maxCount}个组织");
            }
            return myOwnTenantCount;
        }

        public async Task VerifyExistCode(TenantEntity entity)
        {
            //return false;

            var expression = LinqExtensions.True<TenantEntity>();
            //expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.Code == entity.Code);
            }
            else
            {
                expression = expression.And(t => t.Code == entity.Code && t.Id != entity.Id);
            }
            var ret = this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
            if (ret)
            {
                throw new DuplicationDataExection("已存在相同的编码");
            }
        }

        private async Task CreateTenant(TenantEntity entity)
        {
            entity.Code = entity.Code.Trim();
            var myOwnTenantCount = await VerifyExceedMaxCount();
            

            if (!SecurityHelper.IsSafeSqlParam(entity.Code))
            {
                throw new BizException("编码包含非法字符");
            }

            
            await VerifyExistCode(entity);

            var operatorInfo = Operator.Instance.CurrentInfo();

            var repo = await this.BaseRepository().BeginTrans();
            try
            {
                var tenantId = IdGeneratorHelper.Instance.GetId();
                var deptId = IdGeneratorHelper.Instance.GetId();
                var defaultNormalUserRoleId = IdGeneratorHelper.Instance.GetId();

                #region 1、创建租户(TenantEntity)
                entity.Id = tenantId;
                entity.DefaultDepartmentId = deptId;
                entity.DefaultRoleIds = defaultNormalUserRoleId.ToString();//默认角色
                entity.AllowJoinType = (byte)AllowJoinTypeEnumType.JoinAndAudit;

                entity.Create();
                TenantHelper.VerifyIsMyDataOnCreate(entity, false);
                await repo.Insert(entity);
                #endregion

                

                #region 2、初始化一个部门数据（我的部门）
                var dept = new DepartmentEntity();
                dept.Id = deptId;
                dept.TenantId = tenantId;
                dept.DepartmentName = "我的部门";
                dept.ParentId = 0;
                dept.PrincipalId = operatorInfo.CurrentTenantId;

                dept.VerifyTenantId = false;
                dept.Create();
                TenantHelper.VerifyIsMyDataOnCreate(dept, false);
                await repo.Insert(dept);
                #endregion

                #region 3、添加到我的租户中(MyTenantEntity)

               
                var myTenantService = new MyTenantService();
                await myTenantService.AddUserToTenant(repo, operatorInfo.UserId, entity,1);

                #endregion
                

                #region 4、添加字典明细值
                /*
                 * 数据字典分两种
                 * 1、系统字典，所有租户共用，不可修改
                 * 2、非系统字典，不同租户各不相同，但仅限于字典明细的数据
                 * 
                 */

                #endregion

                #region 5、初始化两个角色（管理员、普通用户）
                /*
                 * 
                 */
                var role = new RoleEntity();
                role.TenantId = tenantId;
                role.RoleName = "管理员";
                role.RoleStatus = 1;
                role.RoleSort = 1;
                role.RoleType = (int)RoleTypeEnumType.Admin;
                role.IsSystem = 0;
                role.IsPublic = 0;

                role.VerifyTenantId = false;
                role.Create();
                TenantHelper.VerifyIsMyDataOnCreate(role, false);
                await repo.Insert(role);

                role = new RoleEntity();
                role.Id = defaultNormalUserRoleId;
                role.TenantId = tenantId;
                role.RoleName = "普通用户";
                role.RoleStatus = 1;
                role.RoleSort = 2;
                role.RoleType = (int)RoleTypeEnumType.NormalUser;
                role.IsSystem = 0;
                role.IsPublic = 0;

                role.VerifyTenantId = false;
                role.Create();
                TenantHelper.VerifyIsMyDataOnCreate(role, false);
                await repo.Insert(role);
                #endregion


                #region 6、指定当前用户为 系统租户创建者角色
                //创建租户的时候不需要指定当前用户为租户创建者角色
                //在查询角色的时候如果租户没有指定角色，使用系统默认的角色

                //throw new Exception("a");

                ////一个用户可能创建多个租户，只需要指定一个系统租户就可以了

                //var existedRoleSql = $"select count(*) from SysUserBelong " +
                //    $"  where TenantId = {TenantHelper.SystemTenantID} and UserId={operatorInfo.UserId} ";
                //var existedRole = await this.BaseRepository().GetValue<int>(existedRoleSql);
                //if (existedRole > 0)
                //{
                //}
                //else
                //{
                //    UserBelongEntity roleBelongEntity = new UserBelongEntity();
                //    roleBelongEntity.TenantId = TenantHelper.SystemTenantID;
                //    roleBelongEntity.UserId = operatorInfo.UserId;
                //    roleBelongEntity.BelongId = RoleHelper.TenantCreator.Id;
                //    roleBelongEntity.BelongType = UserBelongTypeEnum.Role.ParseToInt();

                //    roleBelongEntity.VerifyTenantId = false;
                //    roleBelongEntity.Create();
                //    await repo.Insert(roleBelongEntity);
                //}
             
                #endregion

                await repo.CommitTrans();
            }
            catch (Exception ex)
            {
                await repo.RollbackTrans();
                throw;
            }
        }
        #endregion
    }
}
