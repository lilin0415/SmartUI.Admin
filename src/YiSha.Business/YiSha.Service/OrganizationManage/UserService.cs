using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;
using System.Data.Common;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Enum.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Util.Extension;
using YiSha.Enum;
using YiSha.Entity;
using YiSha.Data.EF;
using YiSha.Service.SystemManage;
using YiSha.Web.Code;
using Koo.Utilities.Exceptions;
using Koo.Utilities.Helpers;

using NPOI.POIFS.FileSystem;

namespace YiSha.Service.OrganizationManage
{
    public class UserService : BaseRepositoryService
    {
        #region 获取数据
        public async Task<List<UserEntity>> GetList(UserListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<UserEntity>> GetPageList(UserListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<UserEntity> GetEntity(long id,bool fillPositionAndRole=false)
        {
            var user = await this.BaseRepository().FindEntity<UserEntity>(id);

            if (fillPositionAndRole)
            {
                UserBelongService userBelongService = new UserBelongService();
                await userBelongService.FillPositionAndRoleData(user);

            }

            return user;
        }

        //public async Task<UserEntity> GetEntity(string userName)
        //{
        //    return await this.BaseRepository().FindEntity<UserEntity>(p => p.UserName == userName);
        //}

        //public async Task<UserEntity> GetEntity(long id,string userName)
        //{
        //    return await this.BaseRepository().FindEntity<UserEntity>(x => id > 0 && x.Id == id || !string.IsNullOrEmpty(userName) && x.UserName == userName);
        //}

        public async Task<UserEntity> CheckLogin(string userName,string password, int platform, AppTypeEnumType appType)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new DataInvalidException("用户名不能为空");
            }
            userName = userName.Trim();

            if (string.IsNullOrEmpty(password))
            {
                throw new DataInvalidException("密码不能为空");
            }

            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.UserName == userName);
            expression = expression.Or(t => t.Mobile == userName);
            expression = expression.Or(t => t.Email == userName);
            var user = await this.BaseRepository().FindEntity(expression);

            if (user == null)
            {
                throw new DataNotExistedException("账号不存在，请重新输入");
            }
            if (user.UserStatus == (int)StatusEnum.No)
            {
                throw new DataInvalidException("账号被禁用，请联系管理员");
            }
            if (user.Password != EncryptUserPassword(password, user.Salt))
            {
                throw new DataInvalidException("密码不正确，请重新输入");
            }
            user.LoginCount++;
            user.IsOnline = 1;

            #region 设置日期
            if (DateTimeHelper.IsEmpty(user.FirstVisit))
            {
                user.FirstVisit = DateTime.Now;
            }
            
            if (DateTimeHelper.IsEmpty(user.PreviousVisit))
            {
                user.PreviousVisit = DateTime.Now;
            }
            if (user.LastVisit != GlobalConstant.DefaultTime)
            {
                user.PreviousVisit = user.LastVisit;
            }
            user.LastVisit = DateTime.Now;
            #endregion

            //不同的电脑登录之后，选择不同的我租户，因此内存中应该有两份数据
            var token = SecurityHelper.GetGuid(true) + $"_{(int)appType}";
            user.UserToken = token;

            
            await this.UpdateUser(user);

            return user;
        }
        public async Task<UserEntity> Logoff(long? userId)
        {
            var user = new UserEntity { Id = userId, IsOnline = 0 };
         
            await this.UpdateUser(user);

            return user;
        }
        #endregion

        #region 修改、更新用户信息
        private async Task UpdateUser(UserEntity entity)
        {
            await this.BaseRepository().Update(entity);
        }

      
        /// <summary>
        /// 管理员添加、修改用户信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentIsEmptyException"></exception>
        public async Task SaveForm(UserEntity entity)
        {
            this.VerifyHasManagerPower();

            var user = this.GetCurrentUser();

            if (string.IsNullOrWhiteSpace(entity.UserName))
            {
                throw new CanNotBeEmptyException("登录名称不能为空");
            }
            entity.UserName = entity.UserName.Trim();

            if (entity.Id.IsNullOrZero())
            {
                await RegisterForm(entity);
            }
            else
            {
                var dbEntity = await SetupDbEntity(entity);
                dbEntity.DepartmentId = entity.DepartmentId;

                var db = await this.BaseRepository().BeginTrans();
                try
                {
                    await db.Update(dbEntity);

                    await db.Delete<UserBelongEntity>(t => t.UserId == entity.Id);

                    // 职位
                    foreach (long positionId in TextHelper.SplitToArray<long>(entity.PositionIds, ','))
                    {
                        UserBelongEntity positionBelongEntity = new UserBelongEntity();
                        positionBelongEntity.UserId = entity.Id;
                        positionBelongEntity.BelongId = positionId;
                        positionBelongEntity.BelongType = UserBelongTypeEnum.Position.ParseToInt();
                        positionBelongEntity.Create();
                        await db.Insert(positionBelongEntity);
                    }

                    //角色
                    foreach (long roleId in TextHelper.SplitToArray<long>(entity.RoleIds, ','))
                    {
                        UserBelongEntity departmentBelongEntity = new UserBelongEntity();
                        departmentBelongEntity.UserId = entity.Id;
                        departmentBelongEntity.BelongId = roleId;
                        departmentBelongEntity.BelongType = UserBelongTypeEnum.Role.ParseToInt();
                        departmentBelongEntity.Create();
                        await db.Insert(departmentBelongEntity);
                    }

                    await db.CommitTrans();
                }
                catch
                {
                    await db.RollbackTrans();
                    throw;
                }

            }

        }

        #endregion

        /// <summary>
        /// 查询系统用户
        /// </summary>
        /// <returns></returns>
        private async Task<List<long>> GetSystemUserIds()
        {
            var sql = "select Id from sysuser where IsSystem=1";
            var ret = await base.BaseRepository().GetList<long>(sql);
            return ret;
        }

        #region 删除用户
        public async Task DeleteForm(string ids)
        {
            this.VerifyHasManagerPower();

            var oper = this.GetCurrentUser();

            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            if (idArr.Length == 0)
            {
                return;
            }

            var systemIds = await GetSystemUserIds();

            if (idArr.Any(x => systemIds.Contains(x)))
            {
                throw new ForbidDeleteExection("不可删除系统管理员");
            }

            var db = await this.BaseRepository().BeginTrans();
            try
            {
                await db.Delete<UserEntity>(idArr);
                await db.Delete<UserBelongEntity>(t => idArr.Contains(t.UserId.Value));
                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
        }
        #endregion

        #region 注册用户

        private bool ExistUserName(UserEntity entity)
        {
            var expression = LinqExtensions.True<UserEntity>();
            //expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.UserName == entity.UserName);
            }
            else
            {
                expression = expression.And(t => t.UserName == entity.UserName && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="DuplicationDataExection"></exception>
        public async Task RegisterForm(UserEntity entity)
        {
            if (this.ExistUserName(entity))
            {
                throw new DuplicationDataExection("用户名已经存在");
            }

            var db = await this.BaseRepository().BeginTrans();
            try
            {
                if (!entity.Birthday.IsEmpty())
                {
                    entity.Birthday = entity.Birthday.ParseToDateTime().ToString("yyyy-MM-dd");
                }
              
                if (string.IsNullOrEmpty(entity.RealName))
                {
                    entity.RealName = entity.UserName;
                }

                entity.Salt = GetPasswordSalt();
                entity.Password = EncryptUserPassword(entity.Password, entity.Salt);

                entity.UserStatus = 1;

                entity.SetupCreatorInfo();
                entity.BaseModifierId = entity.Id;
                entity.SetupModifyInfo();

                await db.Insert(entity);

                // 角色
                foreach (long roleId in TextHelper.SplitToArray<long>(entity.RoleIds, ','))
                {
                    UserBelongEntity departmentBelongEntity = new UserBelongEntity();
                    departmentBelongEntity.UserId = entity.Id;
                    departmentBelongEntity.BelongId = roleId;
                    departmentBelongEntity.BelongType = UserBelongTypeEnum.Role.ParseToInt();
                    departmentBelongEntity.Create();
                    await db.Insert(departmentBelongEntity);
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


        private async Task<UserEntity> SetupDbEntity(UserEntity entity)
        {
            if (entity.Id.GetValueOrDefault() == 0)
            {
                throw new ArgumentIsEmptyException("用户id为空");
            }
            if (string.IsNullOrWhiteSpace(entity.RealName))
            {
                entity.RealName = entity.UserName;
            }

            //if (string.IsNullOrWhiteSpace(entity.RealName))
            //{
            //    throw new DataInvalidException("姓名不能为空");
            //}
            //if (string.IsNullOrWhiteSpace(entity.Email))
            //{
            //    throw new DataInvalidException("邮箱不能为空");
            //}
            if (!entity.Birthday.IsEmpty())
            {
                entity.Birthday = entity.Birthday.ParseToDateTime().ToString("yyyy-MM-dd");
            }

            var dbEntity = await GetEntity(entity.Id.Value);
           
            dbEntity.RealName = entity.RealName;
            dbEntity.Gender = entity.Gender;
            dbEntity.Birthday = entity.Birthday;
            dbEntity.Portrait = entity.Portrait;
            dbEntity.Email = entity.Email;
            dbEntity.Mobile = entity.Mobile;
            dbEntity.QQ = entity.QQ;
            dbEntity.Wechat = entity.Wechat;
            dbEntity.Remark = entity.Remark;
            dbEntity.UserStatus = entity.UserStatus;

            dbEntity.Password = null;
            dbEntity.Modify();
            return dbEntity;
        }

        #region 修改个人信息、个人密码
        public async Task ChangeMyDetail(UserEntity entity)
        {
            var user = this.GetCurrentUser();
            entity.Id = user.UserId;

            var dbEntity = await SetupDbEntity(entity);

            var db = await this.BaseRepository().BeginTrans();
            try
            {
                await db.Update(dbEntity);

                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
          
        }

        public async Task<long> ChangeMyPassword(ChangePasswordParam param)
        {
			var user = this.GetCurrentUser();
			param.Id = user.UserId;

			if (GlobalContext.SystemConfig.Demo)
			{
				if (!user.HasManagerPower && user.UserName == "demo")
				{
					throw new ForbidOperationExection("演示模式，不允许操作");
				}
			}

			if (string.IsNullOrEmpty(param.Password) || string.IsNullOrEmpty(param.NewPassword))
            {
                throw new ArgumentIsEmptyException("新密码不能为空");
            }
            
			UserEntity dbUserEntity = await this.GetEntity(param.Id.Value);
         
            //验证旧密码
            if (dbUserEntity.Password != EncryptUserPassword(param.Password, dbUserEntity.Salt))
            {
                throw new DataInvalidException("旧密码不正确");
            }

            //生成新盐+新秘钥
            dbUserEntity.Salt = GetPasswordSalt();
            dbUserEntity.Password = EncryptUserPassword(param.NewPassword, dbUserEntity.Salt);

            dbUserEntity.Modify();

            await this.BaseRepository().Update(dbUserEntity);

            return dbUserEntity.Id.Value;
        }
        #endregion

        #region 重置密码


        public async Task ResetPassword(long? userId,string newPassword)
        {
            this.VerifyHasManagerPower();

            var user = this.GetCurrentUser();
          
            UserEntity dbUserEntity = await this.GetEntity(userId.Value);
            if (dbUserEntity == null)
            {
                throw new DataNotExistedException();
            }

            //生成新盐+新秘钥
            dbUserEntity.Salt = GetPasswordSalt();
            dbUserEntity.Password = EncryptUserPassword(newPassword, dbUserEntity.Salt);

            dbUserEntity.Modify();

            await this.BaseRepository().Update(dbUserEntity);
        }

        /// <summary>
        /// 密码MD5处理
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private string EncryptUserPassword(string password, string salt)
        {
            string md5Password = SecurityHelper.MD5ToHex(password);
            string encryptPassword = SecurityHelper.MD5ToHex(md5Password.ToLower() + salt).ToLower();
            return encryptPassword;
        }

        /// <summary>
        /// 密码盐
        /// </summary>
        /// <returns></returns>
        private string GetPasswordSalt()
        {
            return new Random().Next(1, 100000).ToString();
        }
        #endregion

        #region 私有方法
        private Expression<Func<UserEntity, bool>> ListFilter(UserListParam param)
        {
            var expression = LinqExtensions.True<UserEntity>();

            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.UserName))
                {
                    expression = expression.And(t => t.UserName.Contains(param.UserName));
                }
                if (!string.IsNullOrEmpty(param.UserIds))
                {
                    long[] userIdList = TextHelper.SplitToArray<long>(param.UserIds, ',');
                    expression = expression.And(t => userIdList.Contains(t.Id.Value));
                }
                if (!string.IsNullOrEmpty(param.Mobile))
                {
                    expression = expression.And(t => t.Mobile.Contains(param.Mobile));
                }
                if (param.UserStatus > -1)
                {
                    expression = expression.And(t => t.UserStatus == param.UserStatus);
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
