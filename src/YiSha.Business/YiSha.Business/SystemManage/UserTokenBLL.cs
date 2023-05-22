using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;

namespace YiSha.Business.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-13 14:13
    /// 描 述：业务类
    /// </summary>
    public class UserTokenBLL
    {
        private UserTokenService userTokenService = new UserTokenService();

        #region 获取数据
        public async Task<TData<List<UserTokenEntity>>> GetList(UserTokenListParam param)
        {
            TData<List<UserTokenEntity>> obj = new TData<List<UserTokenEntity>>();
            obj.Result = await userTokenService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<UserTokenEntity>>> GetPageList(UserTokenListParam param, Pagination pagination)
        {
            TData<List<UserTokenEntity>> obj = new TData<List<UserTokenEntity>>();
            obj.Result = await userTokenService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<UserTokenEntity>> GetEntity(long id)
        {
            TData<UserTokenEntity> obj = new TData<UserTokenEntity>();
            obj.Result = await userTokenService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(UserTokenEntity entity)
        {
            TData<string> obj = new TData<string>();
            await userTokenService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await userTokenService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
