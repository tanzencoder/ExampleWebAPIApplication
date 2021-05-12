﻿using ExampleWebAPIApplication.Logic;
using ExampleWebAPIApplication.Logic.Models;
using ExampleWebAPISApplication.Libraries.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleWebAPIApplication.V1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherService weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherService weatherService)
        {
            _logger = logger;
            this.weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            var currentCity = "Albuquerque";
            var forecast = weatherService.GetCurrentWeather();
            var currentCityDict = new Dictionary<string, string>() { { "City", currentCity }, { "State", "NM" } };

            _logger.LogInformation("Example logging simple property: {currentCity}", currentCity);
            _logger.LogInformation("Example logging complex property: {@currentCityObject}", new { City = currentCity, State = "NM", Forecast = forecast });
            _logger.LogInformation("Example logging Dictionary: {@currentCityDict}", currentCityDict);
            _logger.LogInformation("Example logging complex object w dictionary: {@currentCityObject}", new { City = currentCity, State = "NM", Forecast = forecast, CityState = currentCityDict });
            return forecast;
        }
    }
}
