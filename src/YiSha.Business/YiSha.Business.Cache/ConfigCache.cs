using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Model;

namespace YiSha.Business.Cache
{
    public class ConfigCache : BaseBusinessCache<ConfigEntity>
    {
        private ConfigService menuService = new ConfigService();

        public override string CacheKey => this.GetType().Name;

        public override async Task<List<ConfigEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<ConfigEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await menuService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);
                return list;
            }
            else
            {
                return cacheList;
            }
        }
        public async Task<SystemConfigModel> GetConfigModel()
        {
            var ret = new SystemConfigModel();

            var items = await GetList();

            ret.CorporateName = items.FirstOrDefault(x => x.Code == nameof(SystemConfigModel.CorporateName))?.Val;

            ret.PasswordPublicKey = items.FirstOrDefault(x => x.Code == nameof(SystemConfigModel.PasswordPublicKey))?.Val;
            ret.PasswordPrivateKey = items.FirstOrDefault(x => x.Code == nameof(SystemConfigModel.PasswordPrivateKey))?.Val;
            ret.VarPasswordPublicKey = items.FirstOrDefault(x => x.Code == nameof(SystemConfigModel.VarPasswordPublicKey))?.Val;
            ret.VarPasswordPrivateKey = items.FirstOrDefault(x => x.Code == nameof(SystemConfigModel.VarPasswordPrivateKey))?.Val;

            return ret;
        }
    }
}
