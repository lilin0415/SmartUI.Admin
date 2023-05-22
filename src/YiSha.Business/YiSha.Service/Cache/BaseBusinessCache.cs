using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Web.Code;

namespace YiSha.Service.Cache
{
    public abstract class BaseBusinessCache<T>
    {
        public virtual string CacheKey
        {
            get; protected set;
        }
      
        public BaseBusinessCache()
        {
            this.CacheKey = this.GetType().Name;
        }
      
        public virtual bool Remove()
        {
            return CacheFactory.Cache.RemoveCache(CacheKey);
        }

        public virtual async Task<List<T>> GetList()
        {
            var ret = await GetListOverride();
            return ret;
        }

        protected abstract Task<List<T>> GetListOverride();
    }

}
