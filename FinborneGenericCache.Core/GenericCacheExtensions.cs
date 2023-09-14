using FinborneGenericCache.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinborneGenericCache.Core
{
    public static class GenericCacheExtensions
    {
        public static IServiceCollection AddGenericCache<K,V>(this IServiceCollection services, GenericCacheConfig config)
        {
            services.AddSingleton(config);
            services.AddSingleton<IGenericCache<K, V>, GenericCache<K, V>>();
            return services;
        } 
    }
}
