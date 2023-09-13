using FinborneGenericCache.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinborneGenericCache.Core
{
    public static class GenericCacheExtensions
    {
        public static IServiceCollection AddGenericCache<K,V>(this IServiceCollection services, GenericCacheConfig config)
        {
            services.AddSingleton(config);
            services.AddSingleton<IGenericCache<K,V>, GenericCache<K,V>>();
            return services;
        }
    }
}
