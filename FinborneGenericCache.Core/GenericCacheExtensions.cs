using FinborneGenericCache.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinborneGenericCache.Core
{
    public static class GenericCacheExtensions
    {
        public static IServiceCollection AddGenericCache<K,V>(this IServiceCollection services, GenericCacheConfig config)
        {

            services.AddSingleton<IGenericCache<K, V>, GenericCache<K, V>>(provider =>
            {
                var cacheConfig = new GenericCacheConfig { Limit = 40 };
                return new GenericCache<K,V>(cacheConfig, provider.GetService<ILogger<GenericCache<K,V>>>());
            });


            //services.AddSingleton(config);
            //services.AddSingleton<IGenericCache<K,V>, GenericCache<K,V>>();
            return services;
        }

        
    }
}
