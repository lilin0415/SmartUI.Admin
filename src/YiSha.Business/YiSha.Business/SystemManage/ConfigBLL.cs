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
    /// 日 期：2023-02-16 21:00
    /// 描 述：业务类
    /// </summary>
    public class ConfigBLL
    {
        private ConfigService configService = new ConfigService();

        #region 获取数据
        public async Task<TData<List<ConfigEntity>>> GetList(ConfigListParam param)
        {
            TData<List<ConfigEntity>> obj = new TData<List<ConfigEntity>>();
            obj.Result = await configService.GetList(param);
            obj.Total = obj.Result.Count;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<List<ConfigEntity>>> GetPageList(ConfigListParam param, Pagination pagination)
        {
            TData<List<ConfigEntity>> obj = new TData<List<ConfigEntity>>();
            obj.Result = await configService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Status = true;
            return obj;
        }

        public async Task<TData<ConfigEntity>> GetEntity(long id)
        {
            TData<ConfigEntity> obj = new TData<ConfigEntity>();
            obj.Result = await configService.GetEntity(id);
            if (obj.Result != null)
            {
                obj.Status = true;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(ConfigEntity entity)
        {
            TData<string> obj = new TData<string>();
            await configService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Status = true;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            //await configService.DeleteForm(ids);
            //obj.Status = true;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
