using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;
using Koo.Utilities.Exceptions;
using YiSha.Util;

namespace YiSha.Service.Cache
{
    public class MenuCache : BaseBusinessCache<MenuEntity>
    {
        private MenuService menuService = new MenuService();

        public override string CacheKey => this.GetType().Name;

        public override Task<List<MenuEntity>> GetList()
        {
            throw new BizException("使用方法 GetList(bool onlyEnabled)");
        }
        protected override async Task<List<MenuEntity>> GetListOverride()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<MenuEntity>>(CacheKey);
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
        public  async Task<List<MenuEntity>> GetList(bool onlyEnabled)
        {
            var list = await GetListOverride();
            list = MenuService.FilterTree(list, onlyEnabled);
            return list;
        }
      
    }
}
