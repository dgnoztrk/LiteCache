using LiteCache.Client.Abstract;
using LiteCache.Client.Concrete;
using LiteCache.LiteCacheHelper.Abstract;
using LiteCache.LiteCacheHelper.Concrete;
using LiteCache.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LiteCache
{
    public static class ServiceRegisterExtension
    {
        public static void AddLiteCache(this IServiceCollection services, LiteCacheConfig liteCacheConfig)
        {
            services.Configure<LiteCacheConfig>(x =>
            {
                x.IP = liteCacheConfig.IP;
                x.Port = liteCacheConfig.Port;
                x.Password = liteCacheConfig.Password;
            });
            services.AddSingleton<ILiteCacheClient, LiteCacheClient>();
            services.AddSingleton<ILiteCacheService, LiteCacheService>();
        }
    }
}
