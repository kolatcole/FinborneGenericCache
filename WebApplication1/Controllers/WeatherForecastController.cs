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