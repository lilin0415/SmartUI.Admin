using Koo.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Service.Cache;
using YiSha.Business.SystemManage;
using YiSha.Cache.Factory;
using YiSha.Entity;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Enum.OrganizationManage;
using YiSha.Model;
using YiSha.Model.Param;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Service.OrganizationManage;
using YiSha.Service.SystemManage;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Business.OrganizationManage
{
    public class UserBLL
    {
        private UserService userService = new UserService();
     
        #region 获取数据
        public async Task<TData<List<UserEntity>>> GetList(UserListParam param)
        {
            TData<List<UserEntity>> obj = new TData<List<UserEntity>>();
            obj.Result = await userService.GetList(param);
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<UserEntity>>> GetPageList(UserListParam param, Pagination pagination)
        {
            TData<List<UserEntity>> obj = new TData<List<UserEntity>>();
          
            obj.Result = await userService.GetPageList(param, pagination);
          
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<UserEntity>> GetEntity(long id, bool fillPositionAndRole = false)
        {
            TData<UserEntity> obj = new TData<UserEntity>();
            obj.Result = await userService.GetEntity(id, fillPositionAndRole);

            obj.Status = true;
            return obj;
        }

        public async Task<TData<UserEntity>> CheckLogin(string userName, string password, int platform, AppTypeEnumType appType)
        {
            TData<UserEntity> obj = new TData<UserEntity>();
            try
            {
                UserEntity user = await userService.CheckLogin(userName, password, platform,appType);

                obj.Result = user;
                obj.Message = "登录成功";
                obj.Status = true;
                return obj;
            }
            catch (Exception ex)
            {
                return TData.CreateFailedMsg<UserEntity>(ex);
            }
          
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(UserEntity entity)
        {
            TData<string> obj = new TData<string>();
          
            await userService.SaveForm(entity);

            await RemoveCacheById(entity.Id.Value);

            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            if (string.IsNullOrEmpty(ids))
            {
                obj.Message = "参数不能为空";
                return obj;
            }
            await userService.DeleteForm(ids);

            await RemoveCacheById(ids);

            obj.Status = true;
            return obj;
        }

        #region 重置密码
        public async Task<TData<long>> ResetPassword(UserEntity entity)
        {
            TData<long> obj = new TData<long>();
            if (entity.Id > 0)
            {
                await userService.ResetPassword(entity.Id, entity.Password);

                await RemoveCacheById(entity.Id.Value);

                obj.Result = entity.Id.Value;
                obj.Status = true;
            }
            return obj;
        }

        #endregion

        #region 修改个人信息、个人密码
        /// <summary>
        /// 用户自己修改自己的信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TData<long>> ChangeMyDetail(UserEntity entity)
        {
            TData<long> obj = new TData<long>();
            
            await userService.ChangeMyDetail(entity);

            await RemoveCacheById(entity.Id.Value);

            obj.Result = entity.Id.Value;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<long>> ChangeMyPassword(ChangePasswordParam param)
        {
            TData<long> obj = new TData<long>();
            var id = await userService.ChangeMyPassword(param);

            await RemoveCacheById(param.Id.Value);

            obj.Result = id;
            obj.Status = true;
            return obj;
        }
        #endregion

     

        public async Task<TData> ImportUser(ImportParam param, List<UserEntity> list)
        {
            TData obj = new TData();
            //if (list.Any())
            //{
            //    foreach (UserEntity entity in list)
            //    {
            //        UserEntity dbEntity = await userService.GetEntity(entity.UserName);
            //        if (dbEntity != null)
            //        {
            //            entity.Id = dbEntity.Id;
            //            if (param.IsOverride == 1)
            //            {
            //                await userService.SaveForm(entity);
            //                await RemoveCacheById(entity.Id.Value);
            //            }
            //        }
            //        else
            //        {
            //            await userService.SaveForm(entity);
            //            await RemoveCacheById(entity.Id.Value);
            //        }
            //    }
            //    obj.Status = true;
            //}
            //else
            //{
            //    obj.Message = " 未找到导入的数据";
            //}
            return obj;
        }

        #endregion

        #region 私有方法
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

        /// <summary>
        /// 移除缓存里面的token
        /// </summary>
        /// <param name="id"></param>
        private async Task RemoveCacheById(string ids)
        {
            foreach (long id in ids.Split(',').Select(p => long.Parse(p)))
            {
                await RemoveCacheById(id);
            }
        }

        private async Task RemoveCacheById(long id)
        {
            var userTokenService = new UserTokenService();
           var tokenList = await userTokenService.GetListByUserId(id);
            foreach (var userToken in tokenList)
            {
                //清理用户登录的所有设备
                CacheFactory.Cache.RemoveCache(userToken.Token);
            }

        }


        #endregion

        public async Task<TData<string>> Register(UserEntity entity)
        {
            TData<string> obj = new TData<string>();
         
            await userService.RegisterForm(entity);

            await RemoveCacheById(entity.Id.Value);

            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }
    }
}
