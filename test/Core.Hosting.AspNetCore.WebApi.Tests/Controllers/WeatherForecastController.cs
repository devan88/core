using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Hosting.AspNetCore.WebApi.Tests
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HttpService _httpService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, HttpService httpService)
        {
            _httpService = httpService;
            _logger = logger;
        }

        [HttpGet(Name = "Ping")]
        [Produces("application/xml", "application/json")]
        public async Task<IEnumerable<WeatherForecast>> Ping(CancellationToken cancellationToken)
        {
            return await _httpService.GetWeatherDataAsync<List<WeatherForecast>>(cancellationToken) ?? [];
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("GetWeatherForecast");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
