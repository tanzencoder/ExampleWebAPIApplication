using ExampleWebAPIApplication.Logic.Models;
using ExampleWebAPISApplication.Libraries.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleWebAPIApplication.Logic
{
    public class WeatherService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IMyDataStore dataStore;

        public WeatherService(IMyDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task<IEnumerable<WeatherForecast>> GetCurrentWeatherAsync()
        {
            //await dataStore.GetCurrentWeatherAsync();
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
