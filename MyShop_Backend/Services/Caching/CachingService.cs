
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace MyShop_Backend.Services.CachingServices
{
	public class CachingService : ICachingService
	{
		private readonly IMemoryCache _memoryCache;

		public CachingService(IMemoryCache memoryCache) => _memoryCache = memoryCache;
		public T? Get<T>(string cachekey)
		{
			_memoryCache.TryGetValue(cachekey, out T? value);
			return value;
		}
		public void Set<T>(string cachekey, T value, TimeSpan time)
		{
			_memoryCache.Set(cachekey, value, time);
		}
		public void Remove(string cachekey)
		{
			_memoryCache.Remove(cachekey);
		}

		public void Set<T>(string cacheKey, T value, MemoryCacheEntryOptions options)
		{
			_memoryCache.Set(cacheKey, value, options);
		}
	}
}
