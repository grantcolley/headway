using Headway.Core.Interface;
using Headway.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Headway.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<Headway.App.App>("#app");

            builder.Services.AddHttpClient<IWeatherForecastService, WeatherForecastService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44337");
            });

            await builder.Build().RunAsync();
        }
    }
}
