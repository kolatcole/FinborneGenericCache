# FinborneGenericCache

FinborneGenericCache is a generic in-memory cache component which other developers can use in their applications. It provides generic cache implementation that can store
arbitrary items and also takes arbitrary keys which can be used to add and retrieve these items. The cache is implemented to have a threshold which when reached, the Least
Recently Used (LRU) item is evicted to create space for a new one. Logging is also implemented to notify users of the action that occurs. This component is also thread-safe.

# Usage

To use the FinborneGenericCache, you need to register the dependencies to service collection from your application's startup.cs as shown below:

```
    services.AddSingleton<IGenericCache<K, V>, GenericCache<K, V>>(provider =>
    {
        var cacheConfig = new GenericCacheConfig { Limit = 40 };
        return new GenericCache<K,V>(cacheConfig, provider.GetService<ILogger<GenericCache<K,V>>>());
    });
```
The limit value represent the threshold for the cache and can be configured as required.
Inject IGenericCache<K,V> where K denotes the arbitrary key and V denotes the arbitrary value for your cache
```
using FinborneGenericCache.Interface;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        private readonly IGenericCache<int, string> Cache;
        private readonly ILogger<WeatherForecastController> Logger;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IGenericCache<int, string> cache)
        {
            this.Logger = logger;
            this.Cache = cache;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var weatherData = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                Key = Random.Shared.Next(1, 5),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
               
            })
            .ToArray();
            await this.Cache.AddAsync(weatherData[0].Key, weatherData[0].Summary);
            await this.Cache.AddAsync(weatherData[1].Key, weatherData[1].Summary);
            await this.Cache.AddAsync(weatherData[2].Key, weatherData[2].Summary);

            var result = await this.Cache.GetAsync(weatherData[1].Key);
            
            this.Logger.LogInformation($"Summary {result.Value} of Key {result.Key} found in cache");
            return Ok(result);

        }
    }
}
```
You could also check this [Example](https://github.com/kolatcole/FinborneGenericCache/blob/master/FinborneGenericCache.Example/Program.cs)

# License
This project is licensed under the MIT License.
