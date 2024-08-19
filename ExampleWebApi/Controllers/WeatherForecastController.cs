using LiteCache.LiteCacheHelper.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ExampleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ILiteCacheService _liteCache;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ILiteCacheService liteCache)
        {
            _logger = logger;
            _liteCache = liteCache;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("set-val")]
        public async Task<IActionResult> SetValue(string key, string value)
        {
            await _liteCache.SetAsync(key, value, TimeSpan.FromHours(1));
            return Ok();
        }

        [HttpGet("get-val")]
        public async Task<IActionResult> GetValue(string key)
        {
            var value = await _liteCache.GetAsync(key);
            return Ok(value);
        }

        [HttpDelete("del-val")]
        public async Task<IActionResult> DeleteValue(string key)
        {
            await _liteCache.DeleteAsync(key);
            return Ok();
        }

        [HttpGet("get-all-key")]
        public async Task<IActionResult> GetAllKey()
        {
            var keys = await _liteCache.GetKeysAsync();
            return Ok(keys);
        }
    }
}
