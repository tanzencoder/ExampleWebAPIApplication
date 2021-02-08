using ExampleWebAPISApplication.Libraries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExampleWebAPIApplication.V1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMyCache cache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMyCache cache)
        {
            _logger = logger;
            this.cache = cache;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            await cache.SetString("Weather", "Bad Weather");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
