using Headway.Repository.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Headway.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Seed data for testing purposes only...
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
                SeedData.Initialise(applicationDbContext);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog((hostingContext, loggerConfiguration) =>
                  loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
    }
}
