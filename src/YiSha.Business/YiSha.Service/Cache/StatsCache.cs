using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Model.StatsModels;

namespace YiSha.Service.Cache
{
    public class StatsCache : BaseBusinessCache<BaseStatsModel>
    {
        private StatsService menuAuthorizeService = new StatsService();

        public override string CacheKey => this.GetType().Name;

        protected override async Task<List<BaseStatsModel>> GetListOverride()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<BaseStatsModel>>(CacheKey);
            if (cacheList == null)
            {
                var list = await menuAuthorizeService.GetAllStatsModel();
                var nextDay = DateTime.Now.AddDays(1);
                //CacheFactory.Cache.SetCache(CacheKey, list, new DateTime(nextDay.Year, nextDay.Month, nextDay.Day));
                return list;
            }
            else
            {
                return cacheList;
            }
        }
        public async Task<T> GetStatsModel<T>() where T : BaseStatsModel
        {
            var list = await GetList();
            return (T)list.FirstOrDefault(x => x.GetType() == typeof(T));
        }
    }
}
