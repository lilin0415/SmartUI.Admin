using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Model;
using Koo.Utilities.Helpers;
using System.Diagnostics;
using Koo.Utilities.FileFormaKoo.RSSHelper;

namespace YiSha.Service.Cache
{
    public class ConfigCache : BaseBusinessCache<ConfigEntity>
    {
        private ConfigService menuService = new ConfigService();

        public override string CacheKey => this.GetType().Name;

        protected override async Task<List<ConfigEntity>> GetListOverride()
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
        private async void RefreshModel()
        {

        }
        public async Task<SystemConfigModel> GetConfigModel()
        {
            var ret = new SystemConfigModel();

            var properties = TypeHelper.GetProperties(typeof(SystemConfigModel));

            var items = await GetList();
            foreach (var property in properties)
            {
                var item = items.FirstOrDefault(x => x.Code == property.Name);
                if (item != null)
                {
                    TypeHelper.SetPropertyValue(ret, property, item.Val);
                }
            }
         
            return ret;
        }
    }
}
