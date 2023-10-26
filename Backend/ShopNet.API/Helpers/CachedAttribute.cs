using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopNet.BLL.Interfaces;
using System.Text;

namespace ShopNet.API.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLiveSeconds;

        public CachedAttribute(int timeToLiveSeconds)
        {
            this.timeToLiveSeconds = timeToLiveSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCachingService>();
            var cacheKey = GenreateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedResponse = await cacheService.GetCacheResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };

                context.Result = contentResult;
                return;
            }

            var executedContext = await next();

            if(executedContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.CacheResponseAsync(cacheKey,okObjectResult.Value,TimeSpan.FromSeconds(timeToLiveSeconds));
            }
        }

        private string GenreateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path.ToString());

            foreach (var (key,value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append("|"+key + "-"+value);
            }
            return keyBuilder.ToString();
        }
    }
}
