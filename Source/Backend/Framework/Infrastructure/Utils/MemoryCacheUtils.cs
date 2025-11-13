using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Framework.Infrastructure.Utils
{
    public static class MemoryCacheUtils
    {
        public static Task<T> GetAsync<T>(this IMemoryCache memoryCache, string key)
        {
            return Task.FromResult(memoryCache.Get<T>(key));
        }

        public static Task SetAsync<T>(this IMemoryCache memoryCache, string key, T obj, DistributedCacheEntryOptions distributedCacheEntryOptions = null)
        {
            return Task.FromResult(memoryCache.Set<T>(key, obj, distributedCacheEntryOptions.ToMemoryCacheEntryOptions()));
        }

        private static MemoryCacheEntryOptions ToMemoryCacheEntryOptions(this DistributedCacheEntryOptions distributedCacheEntryOptions)
        {
            var result = new MemoryCacheEntryOptions();
            if (distributedCacheEntryOptions == null)
                return result;

            result.AbsoluteExpiration = distributedCacheEntryOptions.AbsoluteExpiration;
            result.AbsoluteExpirationRelativeToNow = distributedCacheEntryOptions.AbsoluteExpirationRelativeToNow;
            result.SlidingExpiration = distributedCacheEntryOptions.SlidingExpiration;

            return result;
        }
    }
}