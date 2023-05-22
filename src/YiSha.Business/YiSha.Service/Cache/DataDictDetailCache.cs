using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;

namespace YiSha.Service.Cache
{
    public class DataDictDetailCache : BaseBusinessCache<DataDictDetailEntity>
    {
        private DataDictDetailService dataDictDetailService = new DataDictDetailService();

        public override string CacheKey => this.GetType().Name;

        protected override async Task<List<DataDictDetailEntity>> GetListOverride()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<DataDictDetailEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await dataDictDetailService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);
                return list;
            }
            else
            {
                return cacheList;
            }
        }
    }
}
