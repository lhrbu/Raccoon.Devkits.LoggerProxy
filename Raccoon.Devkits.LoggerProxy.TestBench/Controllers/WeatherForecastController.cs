using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raccoon.Devkits.LoggerProxy.TestBench.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raccoon.Devkits.LoggerProxy.TestBench.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITestService _testService;
        public WeatherForecastController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _testService.TestMethod();
            _testService.Value = 2;
            Console.WriteLine(_testService.Value);
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
