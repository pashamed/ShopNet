namespace ShopNet.BLL.Interfaces
{
    public interface IResponseCachingService
    {
        Task CacheResponseAsync(string caheKey, object response, TimeSpan timeToLive);
        Task<string> GetCacheResponseAsync(string cacheKey);
    }
}
