using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Enum.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Util;
using YiSha.Model;
using Koo.Utilities.Helpers;
using YiSha.Web.Code;
using Koo.Utilities.Exceptions;
using YiSha.Entity.OrganizationManage;
using YiSha.Enum.OrganizationManage;
using YiSha.Enum;

namespace YiSha.Service.SystemManage
{
    public class RoleService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<RoleModel>> GetList(RoleListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList().MapListTo<RoleModel>();
        }

        public async Task<List<RoleModel>> GetPageList(RoleListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList().MapListTo<RoleModel>();
        }

        public async Task<RoleEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<RoleEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(RoleSort) FROM sysrole");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

        private bool ExistRoleName(RoleEntity entity)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.RoleName == entity.RoleName);
            }
            else
            {
                expression = expression.And(t => t.RoleName == entity.RoleName && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(RoleEntity entity)
        {
            this.VerifyHasSystemRole();

            var user = this.GetCurrentUser();

            #region 验证
            if (string.IsNullOrWhiteSpace(entity.RoleName))
            {
                throw new ArgumentIsEmptyException("名称不能为空");
            }
            entity.RoleName = entity.RoleName.Trim();

            if (this.ExistRoleName(entity))
            {
                throw new DuplicationDataExection("角色名称已经存在");
            }
          
            #endregion

            RoleEntity dbEntity = entity.Id.IsNullOrZero() ? new RoleEntity() : await this.GetEntity(entity.Id.Value);

            if (dbEntity == null)
            {
                throw new DataNotExistedException();
            }
            if (System.Enum.TryParse<RoleTypeEnumType>(entity.RoleType?.ToString(), out RoleTypeEnumType _roleType))
            {
                dbEntity.RoleType = entity.RoleType;
            }
            else
            {
                throw new ArgumentErrorException("角色类型错误");
            }

            var db = await this.BaseRepository().BeginTrans();
            try
            {
               
                //中只能使用这两种角色类型：用户、管理员
                //创建人
                dbEntity.RoleType = entity.RoleType;

                dbEntity.IsSystem = 0;
             
                dbEntity.RoleName = entity.RoleName;
                dbEntity.RoleSort = entity.RoleSort;
                dbEntity.RoleStatus = entity.RoleStatus;
                dbEntity.Remark = entity.Remark;
                
                if (dbEntity.Id.IsNullOrZero())
                {
                    dbEntity.Create();
                    this.VerifyIsMyDataOnCreate<RoleEntity>(dbEntity);
                    await db.Insert(dbEntity);
                }
                else
                {
                    dbEntity.Modify();
                    this.VerifyIsMyDataOnModify<RoleEntity>(dbEntity);
                    await db.Update(dbEntity);

                    await db.Delete<MenuAuthorizeEntity>(t => t.AuthorizeId == dbEntity.Id && t.AuthorizeType == AuthorizeTypeEnum.Role.ParseToInt());
                }

                // 角色对应的菜单、页面和按钮权限
                if (!string.IsNullOrEmpty(entity.MenuIds))
                {
                    foreach (long menuId in TextHelper.SplitToArray<long>(entity.MenuIds, ','))
                    {
                        MenuAuthorizeEntity menuAuthorizeEntity = new MenuAuthorizeEntity();
                        menuAuthorizeEntity.AuthorizeId = entity.Id;
                        menuAuthorizeEntity.MenuId = menuId;
                        menuAuthorizeEntity.AuthorizeType = AuthorizeTypeEnum.Role.ParseToInt();
                        menuAuthorizeEntity.Create();
                        await db.Insert(menuAuthorizeEntity);
                    }
                }
                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
        }
        #endregion

        public async Task DeleteForm(string ids)
        {
            this.VerifyHasSystemRole();
           
            var currentUser = this.GetCurrentUser();
          
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            //if (idArr.Any(x => RoleHelper.IsSystemRole(x)))
            //{
            //    throw new ForbidDeleteExection("系统角色不允许删除");
            //}
            this.VerifyIsMyDataOnDelete<RoleEntity>(ids);
            var trans = await this.BaseRepository().BeginTrans();
            try
            {
                await trans.Delete<RoleEntity>(idArr);
                await trans.Delete<UserBelongEntity>(t => idArr.ToList().Contains(t.BelongId.Value) && t.BelongType == UserBelongTypeEnum.Role.ParseToInt());
                await trans.Delete<MenuAuthorizeEntity>(t => idArr.ToList().Contains(t.AuthorizeId.Value) && t.AuthorizeType == AuthorizeTypeEnum.Role.ParseToInt());
                await trans.CommitTrans();
            }
            catch
            {
                await trans.RollbackTrans();
            }
            
        }
      

        #region 私有方法
        private Expression<Func<RoleEntity, bool>> ListFilter(RoleListParam param)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.RoleName))
                {
                    expression = expression.And(t => t.RoleName.Contains(param.RoleName));
                }
                if (!string.IsNullOrEmpty(param.RoleIds))
                {
                    long[] roleIdArr = TextHelper.SplitToArray<long>(param.RoleIds, ',');
                    expression = expression.And(t => roleIdArr.Contains(t.Id.Value));
                }
                if (!string.IsNullOrEmpty(param.RoleName))
                {
                    expression = expression.And(t => t.RoleName.Contains(param.RoleName));
                }
                if (param.RoleStatus > -1)
                {
                    expression = expression.And(t => t.RoleStatus == param.RoleStatus);
                }
                if (!string.IsNullOrEmpty(param.StartTime.ParseToString()))
                {
                    expression = expression.And(t => t.BaseModifyTime >= param.StartTime);
                }
                if (!string.IsNullOrEmpty(param.EndTime.ParseToString()))
                {
                    param.EndTime = param.EndTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                    expression = expression.And(t => t.BaseModifyTime <= param.EndTime);
                }
            }
            return expression;
        }
        #endregion
    }
}
