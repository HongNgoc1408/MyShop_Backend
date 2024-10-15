using Microsoft.Extensions.Caching.Memory;

namespace MyShop_Backend.Services.CachingServices
{
	public interface ICachingService
	{
		T? Get<T>(string cachekey);
		void Set<T>(string cachekey, T value, TimeSpan time);
		void Set<T>(string cacheKey, T value, MemoryCacheEntryOptions options);
		void Remove(string cachekey);

	}
}
