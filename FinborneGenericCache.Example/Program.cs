using FinborneGenericCache.Core;
using FinborneGenericCache.Interface;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

//var config = new GenericCacheConfig();
//config.Limit = 4;
////var cache = new GenericCache<string, string>(config);

////var services = new ServiceCollection();
//var cache;
//cache.Add("1", "one");
//cache.Add("3", "three");  // head is 3, Tail is 1     3 -> 1
//cache.Add("2", "two");      // head is 2, Tail is 1   2 -> 3 -> 1

//var one = cache.Get("1");   // head is 1, Tail is 3   1 -> 2 -> 3
//var two = cache.Get("2");   // head is 2, Tail is 3   2 -> 1 -> 3
//cache.Add("4", "four");     // head is 4, Tail is 3   4 -> 2 -> 1 -> 3
//var three = cache.Get("3");  // head is 3, Tail is 1   3 -> 4 -> 2 -> 1
//cache.Add("5", "five");      // head is 5, Tail is 2   5 -> 3 -> 4 -> 2 


public class CacheExample
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = SetupDI();
        var cache = serviceProvider.GetService<IGenericCache<string,string>>();

        await cache.AddAsync("1", "one");
        await cache.AddAsync("1", "one");
        await cache.AddAsync("3", "three");  // head is 3, Tail is 1     3 -> 1
        await cache.AddAsync("2", "two");      // head is 2, Tail is 1   2 -> 3 -> 1

        var one = await cache.GetAsync("1");   // head is 1, Tail is 3   1 -> 2 -> 3
        var two = await cache.GetAsync("2");   // head is 2, Tail is 3   2 -> 1 -> 3
        await cache.AddAsync("4", "four");     // head is 4, Tail is 3   4 -> 2 -> 1 -> 3
        var three = await cache.GetAsync("3");  // head is 3, Tail is 1   3 -> 4 -> 2 -> 1
        await cache.AddAsync("5", "five");      // head is 5, Tail is 2   5 -> 3 -> 4 -> 2 

    }

    private static ServiceProvider SetupDI()
    {
        var services = new ServiceCollection();
        Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();

        
        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: true); // dispose: true will dispose the Serilog logger when the provider is disposed
        });
        services.AddGenericCache<string,string>(new GenericCacheConfig
        {
            Limit = 4
        });
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}

