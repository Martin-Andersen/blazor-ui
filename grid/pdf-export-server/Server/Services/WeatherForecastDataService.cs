using ServerPdfExport.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerPdfExport.Server.Services
{
    public class WeatherForecastDataService
    {
        private static readonly string[] Summaries = new[]
      {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        // this static list acts as our "database" in this sample
        private static IQueryable<WeatherForecast> _forecasts { get; set; }

        public async Task<IQueryable<WeatherForecast>> GetForecasts()
        {
            // generate some data for the sake of this demo
            if (_forecasts == null)
            {
                var startDate = DateTime.Now.Date;
                _forecasts = Enumerable.Range(1, 5000).Select(index => new WeatherForecast
                {
                    Id = index,
                    Date = startDate.AddDays(index),
                    //unlike the MS template, we will not have random data here because it may get generated anew when exporting
                    //and new random values may make it seem like the data/filter/exporting does not work as expected
                    TemperatureC = index % 40,
                    Summary = Summaries[index % Summaries.Length]
                }).AsQueryable();
            }

            return await Task.FromResult(_forecasts);
        }
    }
}
