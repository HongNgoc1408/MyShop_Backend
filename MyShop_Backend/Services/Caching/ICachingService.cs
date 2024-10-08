﻿namespace MyShop_Backend.Services.CachingServices
{
	public interface ICachingService
	{
		T? Get<T>(string cachekey);
		void Set<T>(string cachekey, T value, TimeSpan time);
		void Remove(string cachekey);
	}
}