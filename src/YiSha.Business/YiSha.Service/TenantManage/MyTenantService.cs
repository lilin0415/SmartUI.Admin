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
using YiSha.Web.Code;
using Koo.Utilities.Helpers;
using YiSha.Entity.OrganizationManage;
using YiSha.Enum.OrganizationManage;
using YiSha.Service.OrganizationManage;
using YiSha.Model.TenantManage;
using YiSha.Model.Param.OrganizationManage;
using Koo.Utilities.Exceptions;
using NPOI.POIFS.FileSystem;
using YiSha.Entity.SystemManage;
using YiSha.Entity.ProjectManager;
using YiSha.Enum;

namespace YiSha.Service.TenantManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-10-26 22:01
    /// 描 述：服务类
    /// </summary>
    public class MyTenantService : BaseRepositoryService
    {
        #region 查询当前租户下面所有的用户
        public async Task<List<MyTenantUserModel>> GetMyTenantUserList(MyTenantListParam param)
        {

            var deptService = new DepartmentService();

            TData<List<UserEntity>> obj = new TData<List<UserEntity>>();
            if (param?.DepartmentId != null)
            {
                param.ChildrenDepartmentIdList = await deptService.GetChildrenDepartmentIdList(null, param.DepartmentId.Value);
            }
            else
            {
                //OperatorInfo user = await Operator.Instance.Current();
                //param.ChildrenDepartmentIdList = await deptService.GetChildrenDepartmentIdList(null, user.DepartmentId.Value);
            }



            var sql = GetQueryMyTenantUserSql(param);
            var list = await this.BaseRepository().FindList<MyTenantUserModel>(sql);
            return list.ToList();
        }

        public async Task<List<MyTenantUserModel>> GetMyTenantUserPageList(MyTenantListParam param, Pagination pagination)
        {
            var deptService = new DepartmentService();

            TData<List<UserEntity>> obj = new TData<List<UserEntity>>();
            if (param?.DepartmentId != null)
            {
                param.ChildrenDepartmentIdList = await deptService.GetChildrenDepartmentIdList(null, param.DepartmentId.Value);
            }
            else
            {
                //OperatorInfo user = await Operator.Instance.Current();
                //param.ChildrenDepartmentIdList = await deptService.GetChildrenDepartmentIdList(null, user.DepartmentId.Value);
            }

            var sql = GetQueryMyTenantUserSql(param);
            var list = await this.BaseRepository().FindList<MyTenantUserModel>(sql, pagination);
            return list.list.ToList();
        }
        public async Task<MyTenantUserModel> GetMyTenantUserEntity(long id)
        {
            var list = await GetMyTenantUserList(new MyTenantListParam() { MyTenantIds = id.ToString() });
            var ret = list.FirstOrDefault();
            if (ret != null)
            {
                await GetUserBelong(ret);
            }
            return ret;
        }
        private string GetQueryMyTenantUserSql(MyTenantListParam param)
        {
            var operatorInfo = Operator.Instance.CurrentInfo();

            var sb = new StringBuilder();

            //var hasDept = param != null && param.ChildrenDepartmentIdList != null && param.ChildrenDepartmentIdList.Count > 0;

            //用户列表肯定是要和当前租户想关联
            sb.Append("select b.*" +
                    $" ,a.UserName ,a.RealName ,a.Gender ,a.Birthday ,a.Portrait " +
                    $" ,a.Email ,a.Mobile ,a.QQ ,a.Wechat, a.UserStatus " +
                    $" ,c.DepartmentName  " +
                    $" from SysUser a " +
                    $" inner join  MyTenant b " +
                    $"      on a.Id = b.UserId and b.TenantId = {operatorInfo.CurrentTenantId.GetValueOrDefault()} and b.IsLeft=0 ");

            sb.Append($" left join SysDepartment c " +
                  $"      on b.TenantId = c.TenantId and b.DepartmentId = c.Id ");


            sb.Append(" where 1=1 ");

            if (param != null)
            {
                var myTenantIdList = TextHelper.SplitToArray<long>(param.MyTenantIds, ',');
                if (myTenantIdList.Any())
                {
                    sb.Append($" and b.Id  in ({string.Join(",", myTenantIdList)})");
                }

                if (SecurityHelper.IsSafeSqlParam(param.UserName))
                {
                    sb.Append($" and a.UserName like '%{param.UserName}%'");
                }

                long[] userIdList = TextHelper.SplitToArray<long>(param.UserIds, ',');
                if (userIdList.Any())
                {
                    sb.Append($" and a.Id  in ({string.Join(",", userIdList)})");
                }

                if (SecurityHelper.IsSafeSqlParam(param.Mobile))
                {
                    sb.Append($" and a.Mobile like '%{param.Mobile}%'");
                }
                if (param.UserStatus > -1)
                {
                    sb.Append($" and a.UserStatus ={param.UserStatus}");
                }

                if (!string.IsNullOrEmpty(param.StartTime.ParseToString()))
                {
                    sb.Append($" and b.JoinTime >= '{param.StartTime}'");
                }

                if (!string.IsNullOrEmpty(param.EndTime.ParseToString()))
                {
                    param.EndTime = param.EndTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                    sb.Append($" and b.JoinTime <= '{param.EndTime}'");
                }

                if (param.ChildrenDepartmentIdList != null && param.ChildrenDepartmentIdList.Count > 0)
                {
                    sb.Append($" and b.DepartmentId  in ({string.Join(",", param.ChildrenDepartmentIdList)})");
                }
            }
            return sb.ToString();
        }
        #endregion

        public async Task<int> GetMyOwnTenantCount()
        {
            var userId = this.GetCurrentUserId();
            var sql = $"select count(*) from {MyTenantEntity.TblName} " +
                $"  where UserId={userId} and RoleType =1 ";
            var table = await this.BaseRepository().GetValue<int>(sql);
            return table;

        }

        #region 获取数据
        public async Task<List<MyTenantInfo>> GetMyTenantInfoList(long userId)
        {
            var sql = $"select " +
                $"  a.Code as  TenantCode" +
                $", a.Name as TenantName  " +
                $", a.Type as TenantType  " +
                $", a.IsEnable as TenantIsEnable  " +
                $", b.* " +

                $" from {TenantEntity.TblName} a " +
                $" inner join {MyTenantEntity.TblName} b " +
                $"      on b.TenantId = a.Id and b.UserId={userId}";

            var list = await this.BaseRepository().FindList<MyTenantInfo>(sql);
            return list.ToList();
        }

        public async Task<List<MyTenantEntity>> GetList(MyTenantListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<MyTenantEntity>> GetPageList(MyTenantListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }
        //MyTenantUserModel
        public async Task<MyTenantEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<MyTenantEntity>(id);
        }

        #endregion

        #region 提交数据
      
   
        public async Task<long> SaveForm(MyTenantEntity entity)
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

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            var safeIds = SecurityHelper.ToSafeSqlIds(ids);
            if (!string.IsNullOrEmpty(safeIds))
            {
                var sql = $"select RoleType from {MyTenantEntity.TblName} where Id in ({safeIds})";
                var roleTypeList = await this.BaseRepository().GetList<int>(sql);
                if (roleTypeList.Any(x => x == 1))
                {
                    throw new ForbidDeleteExection("不可删除租户创建人");
                }
                this.VerifyIsMyDataOnDelete<MyTenantEntity>(ids);
                var trans = await this.BaseRepository().BeginTrans();
                try
                {
                    var currentTenantId = this.GetCurrentTenantId();
                    //删除租户中的用户
                    await trans.Delete<MyTenantEntity>(idArr);

                    ////查询所有的角色
                    //var roles = await trans.FindList<UserBelongEntity>(x => x.TenantId == currentTenantId && idArr.Contains(x.BelongId.Value) && x.BelongType == (int)UserBelongTypeEnum.Role);

                    ////
                    //await trans.Delete<MenuAuthorizeEntity>(x => x.TenantId == this.GetCurrentTenantId() && idArr.Contains(x.AuthorizeId.Value));


                    //删除在当前租户中给此用户设置的角色、岗位
                    await trans.Delete<UserBelongEntity>(x => x.TenantId == this.GetCurrentTenantId() && idArr.Contains(x.BelongId.Value));

                    await trans.CommitTrans();

                }
                catch
                {
                    await trans.RollbackTrans();
                    throw;
                }
                
            }
            
           
        }
        #endregion

        #region 私有方法
        private Expression<Func<MyTenantEntity, bool>> ListFilter(MyTenantListParam param)
        {
            var expression = LinqExtensions.True<MyTenantEntity>();
            if (param != null)
            {
            }
            return expression;
        }

        #endregion

        /// <summary>
        /// 获取用户的职位和角色
        /// </summary>
        /// <param name="user"></param>
        public async Task GetUserBelong(MyTenantUserModel user)
        {
            var userBelongService = new UserBelongService();

            List<UserBelongEntity> userBelongList = await userBelongService.GetList(new UserBelongEntity { TenantId = user.TenantId, UserId = user.UserId });

            List<UserBelongEntity> roleBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Role.ParseToInt()).ToList();
            if (roleBelongList.Count > 0)
            {
                user.RoleIds = string.Join(",", roleBelongList.Select(p => p.BelongId).ToList());
            }

            List<UserBelongEntity> positionBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Position.ParseToInt()).ToList();
            if (positionBelongList.Count > 0)
            {
                user.PositionIds = string.Join(",", positionBelongList.Select(p => p.BelongId).ToList());
            }
        }

        #region 用户加入组织
        public async Task<string> JoinToTenant(string code, string name)
        {
            //if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
            //{
            //    throw new ArgumentIsEmptyException("组织编码和名称不能为空");
            //}
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentIsEmptyException("组织编码不能为空");
            }
            var tenantService = new TenantService();
            var tenant = await tenantService.GetEntity(code, name);
            if (tenant == null)
            {
                throw new DataNotExistedException("找不到此组织");
            }

            var user = this.GetCurrentUser();

            var processed = await CheckAndProcessIsLeftedUser(user.UserId, tenant, $"您已经是该组织[{tenant.Name}]的成员，不需要重复加入");
            if (processed)
            {
                return $"您已成功加入组织[{tenant.Name}]";
            }


            if (tenant.AllowJoinType == (int)AllowJoinTypeEnumType.Forbid)
            {
                throw new ForbidOperationExection($"此组织[{tenant.Name}]设置为禁用加入");
            }
            else if (tenant.AllowJoinType == (int)AllowJoinTypeEnumType.JoinWithoutAudit)
            {
                var db = await this.BaseRepository().BeginTrans();
                try
                {
                    await AddUserToTenant(db, user.UserId, tenant, 0);
                    await db.CommitTrans();

                    return $"您已成功加入组织【{tenant.Name}】";
                }
                catch (Exception ex)
                {
                    await db.RollbackTrans();
                    throw;
                }
            }
            else
            {
                var msg = new UserMsgEntity();
                msg.FromId = user.UserId;
                msg.ToId = tenant.Id;
                msg.TenantId = tenant.Id;
                msg.MsgType = (int)UserMsgTypeEnumType.JoinTenant;
                msg.Title = $"用户[{user.RealName}]申请加入组织[{tenant.Name}]";
                msg.Content = msg.Title;

                var msgService = new UserMsgService();
                await msgService.SaveForm(msg);

                return $"已向组织【{tenant.Name}】发出申请，等待组织审核";
            }

        }

        #endregion

        #region 邀请用户
        /// <summary>
        /// 邀请用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="ForbidOperationExection"></exception>
        public async Task<string> InviteToTenant(long?userId,string userName)
        {
            var tenantService = new TenantService();
            var currentTenant = await tenantService.GetEntity(this.GetCurrentTenantId().GetValueOrDefault());

            var processed = await CheckAndProcessIsLeftedUser(userId, currentTenant, "用户已经是该组织的成员，不需要重复添加");
            if (processed)
            {
                return $"用户已成功加入当前组织";
            }

            var currentUser = this.GetCurrentUser();

            var userSerivce = new UserService();
            var toUser = await userSerivce.GetEntity(userId.GetValueOrDefault(), userName);
            if (toUser.AllowJoinType == (int)AllowJoinTypeEnumType.Forbid)
            {
                throw new ForbidOperationExection("此用户设置为禁用添加");
            }
            else if (toUser.AllowJoinType == (int)AllowJoinTypeEnumType.JoinWithoutAudit)
            {
                var trans = await this.BaseRepository().BeginTrans();
                try
                {
                    await AddUserToTenant(trans, userId, currentTenant, 0);
                    await trans.CommitTrans();

                    return $"用户【{toUser.RealName}】已成功加入组织";
                }
                catch
                {
                    await trans.RollbackTrans();
                    throw;
                }
            }
            else
            {
                var msg = new UserMsgEntity();
                msg.FromId = currentUser.UserId;
                msg.ToId = toUser.Id;
                msg.TenantId = currentTenant.Id;
                msg.MsgType = (int)UserMsgTypeEnumType.InviteToTenant;
                msg.Title = $"用户[{currentUser.RealName}]邀请您加入组织【{currentTenant.Name}】";
                msg.Content = msg.Title;

                msg.Create();
                var msgService = new UserMsgService();
                await msgService.SaveForm(msg);

                return $"已向用户【{toUser.RealName}】发出申请，等待用户审核";
            }
        }
        #endregion

        #region 在加入、邀请用户的时候检查是否有相同的已经离开的用户，如果有的话重新还原
        private async Task<bool> CheckAndProcessIsLeftedUser(long? userId, TenantEntity currentTenant, string duplicateMsg)
        {
            var db = await this.BaseRepository().BeginTrans();
            try
            {
                var existed = await db.FindEntity<MyTenantEntity>(x => x.TenantId == currentTenant.Id && x.UserId == userId);

                if (existed == null)
                {
                    return false;
                }

                if (existed.IsLeft == 0)
                {
                    //已加入此租户
                    throw new DuplicationDataExection(duplicateMsg);
                }

                await RestoreUser(db, existed, currentTenant);
                await db.CommitTrans();

                return true;
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
        }
        private async Task RestoreUser(Repository db, MyTenantEntity myTenantInDb, TenantEntity tenant)
        {
            myTenantInDb.TenantId = tenant.Id;
            myTenantInDb.DepartmentId = tenant.DefaultDepartmentId;
            myTenantInDb.IsEnable = 1;
            myTenantInDb.RoleType = 0;

            //如果当前用户已离开，重新加入
            //重新加入租户
            myTenantInDb.IsLeft = 0;
            myTenantInDb.LeftTime = DateTimeHelper.Empty1970;

            myTenantInDb.JoinTime = DateTimeHelper.Now;
            myTenantInDb.LastActiveTime = DateTimeHelper.Now;

            myTenantInDb.Modify();

            await db.Update(myTenantInDb);

            //设置用户在当前租户下面的角色、岗位
            await SetTenantUserRole(db, tenant.Id, myTenantInDb.UserId, tenant.DefaultRoleIds, tenant.DefaultPositionIds);
        }
        #endregion

        #region 修改已有用户

        public async Task<string> SaveForm(MyTenantUserModel model)
        {
            if (model.UserId.IsNullOrZero())
            {
                throw new BizException("用户不能为空");
            }

            var info = Operator.Instance.CurrentInfo();

         
            var db = await this.BaseRepository().BeginTrans();
            try
            {
                var myTenantInDb = await db.FindEntity<MyTenantEntity>(x => x.TenantId == info.CurrentTenantId && x.UserId == model.UserId);

                if (myTenantInDb == null)
                {
                    throw new DataNotExistedException("当前组织下未找到此用户，不能修改");
                }

                //创始人
                if (myTenantInDb.RoleType == 1 && !info.IsOwnerOfCurrentTenant)
                {
                    throw new ForbidUpdateExection();
                }

                if (myTenantInDb.UserId == info.UserId &&model.IsEnable==0)
                {
                    throw new ForbidUpdateExection("不能禁用自己");
                }

                //修改的都是可见的用户，不需要判断IsLeft
               
                myTenantInDb.DepartmentId = model.DepartmentId;
                myTenantInDb.Remark = model.Remark;
                myTenantInDb.IsEnable = model.IsEnable;

                myTenantInDb.Modify();
                await db.Update(myTenantInDb);

                //设置用户在当前租户下面的角色、岗位
                await SetTenantUserRole(db, info.CurrentTenantId, model.UserId, model.RoleIds, model.PositionIds);

                await db.CommitTrans();
                return myTenantInDb.Id.ToString();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
        }
        #endregion

        #region 添加用户到租户 设置用户的权限

        /// <summary>
        /// 添加 用户加入租户
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId"></param>
        /// <param name="defaultTenant"></param>
        /// <returns></returns>
        public async Task<MyTenantEntity> AddUserToTenant(Repository db, long? userId, TenantEntity tenantEntity, int roleType)
        {
            var sql = $"select * from {MyTenantEntity.TblName} where TenantId = {tenantEntity.Id} and UserId={userId} ";
            var myTenantInDb = (await this.BaseRepository().FindList<MyTenantEntity>(sql)).FirstOrDefault();

            if (myTenantInDb != null)
            {
                throw new DuplicationDataExection("已存在此用户");
            }

            var myTenant = new MyTenantEntity();
            myTenant.TenantId = tenantEntity.Id;
            myTenant.UserId = userId;
            
            myTenant.DepartmentId = tenantEntity.DefaultDepartmentId;

            myTenant.RoleType = (byte)roleType;
           
            myTenant.IsEnable = 1;
            //myTenant.Remark = model.Remark;

            myTenant.JoinTime = DateTimeHelper.Now;
            myTenant.LastActiveTime = DateTimeHelper.Now;
            myTenant.LeftTime = DateTimeHelper.Empty1970;

            myTenant.VerifyTenantId = false;
            myTenant.Create();
            TenantHelper.VerifyIsMyDataOnCreate(myTenant, false);
            await db.Insert(myTenant);

            //设置用户在当前租户下面的默认角色、岗位
            await SetTenantUserRole(db, tenantEntity.Id, userId, tenantEntity.DefaultRoleIds, tenantEntity.DefaultPositionIds);

            return myTenant;
        }

        /// <summary>
        /// 设置用户角色、岗位
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId"></param>
        /// <param name="defaultTenant"></param>
        /// <returns></returns>
        private async Task SetTenantUserRole(Repository db, long? tenantId, long? userId, string roleIds,string postionIds)
        {
            //删除旧的职位
            await db.Delete<UserBelongEntity>(t => t.TenantId == tenantId && t.UserId == userId && t.BelongType == (int)UserBelongTypeEnum.Position);

            //删除旧的自定义角色

        
            //由于可以通过管理员，直接设置用户在某个租户下面的角色，
            //因此只能删除用户下面的自定义角色，不能删除系统角色
            //var sql = $"delete from SysUserBelong where TenantId = {tenantId} and UserId = {userId} " +
            //    $" and BelongId not in (select Id from SysRole where TenantId = {TenantHelper.SystemTenantID} and IsSystem = 1 ) ";
            //await db.ExecuteBySql(sql);

            var systemRoles = await db.FindList<RoleEntity>(x => x.TenantId == TenantHelper.SystemTenantID && x.IsSystem == 1);
            var systemRoleIds = systemRoles.Select(x => x.Id).ToList();

            await db.Delete<UserBelongEntity>(t => t.TenantId == tenantId && t.UserId == userId && t.BelongType == (int)UserBelongTypeEnum.Role && !systemRoleIds.Contains(t.BelongId));


            foreach (var defaultRoleId in SecurityHelper.ToSafeSqlIdArray(roleIds))
            {
                //设置默认租户的默认角色
                UserBelongEntity defaultRole = new UserBelongEntity();
                defaultRole.TenantId = tenantId;
                defaultRole.UserId = userId;
                defaultRole.BelongId = defaultRoleId;
                defaultRole.BelongType = UserBelongTypeEnum.Role.ParseToInt();

                defaultRole.VerifyTenantId = false;
                defaultRole.Create();
                TenantHelper.VerifyIsMyDataOnCreate(defaultRole, false);
                await db.Insert(defaultRole);
            }

            foreach (var defaultPositionId in SecurityHelper.ToSafeSqlIdArray(postionIds))
            {
                //设置默认租户的默认岗位
                UserBelongEntity defaultRole = new UserBelongEntity();
                defaultRole.TenantId = tenantId;
                defaultRole.UserId = userId;
                defaultRole.BelongId = defaultPositionId;
                defaultRole.BelongType = UserBelongTypeEnum.Position.ParseToInt();

                defaultRole.VerifyTenantId = false;
                defaultRole.Create();
                TenantHelper.VerifyIsMyDataOnCreate(defaultRole, false);
                await db.Insert(defaultRole);
            }
        }
        #endregion

     
    }
}
