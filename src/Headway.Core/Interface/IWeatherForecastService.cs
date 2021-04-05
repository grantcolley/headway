using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecasts();
    }
}
