using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Service.OrganizationManage;
using YiSha.Model.OrganizationManage;

namespace YiSha.Business.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2022-11-24 10:41
    /// 描 述：业务类
    /// </summary>
    public class UserMsgBLL
    {
        private UserMsgService userMsgService = new UserMsgService();

        #region 获取数据
        public async Task<TData<List<UserMsgEntity>>> GetList(UserMsgListParam param)
        {
            TData<List<UserMsgEntity>> obj = new TData<List<UserMsgEntity>>();
            obj.Result = await userMsgService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<UserMsgModel>>> GetPageList(UserMsgListParam param, Pagination pagination)
        {
            TData<List<UserMsgModel>> obj = new TData<List<UserMsgModel>>();
            obj.Result = await userMsgService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<UserMsgEntity>> GetEntity(long id)
        {
            TData<UserMsgEntity> obj = new TData<UserMsgEntity>();
            obj.Result = await userMsgService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        public async Task<TData<string>> AckForm(UserMsgEntity entity)
        {
            TData<string> obj = new TData<string>();
            await userMsgService.AckForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        #region 提交数据
        public async Task<TData<string>> SaveForm(UserMsgEntity entity)
        {
            TData<string> obj = new TData<string>();
            await userMsgService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await userMsgService.DeleteForm(ids);
            obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
