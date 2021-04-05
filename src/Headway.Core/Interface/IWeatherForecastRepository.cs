using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Core.Interface
{
    public interface IWeatherForecastRepository
    {
        IEnumerable<WeatherForecast> GetWeatherForecasts();
    }
}
