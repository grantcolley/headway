using Headway.BlazorWebassemblyApp.Account;
using Headway.Core.Cache;
using Headway.Core.Interface;
using Headway.Core.Mediators;
using Headway.Core.Model;
using Headway.Core.State;
using Headway.Services;
using MediatR;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.BlazorWebassemblyApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("OidcConfiguration", options.ProviderOptions);
                options.UserOptions.RoleClaim = "role";
            }).AddAccountClaimsPrincipalFactory<UserAccountFactory>();

            builder.Services.AddHttpClient("webapi", (sp, client) =>
            {
                client.BaseAddress = new Uri("https://localhost:44320");
            }).AddHttpMessageHandler(sp =>
            {
                var handler = sp.GetService<AuthorizationMessageHandler>()
                .ConfigureHandler(
                    authorizedUrls: new[] { "https://localhost:44320" },
                    scopes: new[] { "webapi" });
                return handler;
            });

            builder.Services.AddSingleton<IAppCache, AppCache>();
            builder.Services.AddSingleton<IConfigCache, ConfigCache>();
            builder.Services.AddSingleton<IStateNotification, StateNotification>();

            builder.Services.AddTransient<IModuleService, ModuleService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new ModuleService(httpClient);
            });

            builder.Services.AddTransient<IConfigurationService, ConfigurationService>(sp =>
            {
                var configCache = sp.GetRequiredService<IConfigCache>();
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new ConfigurationService(httpClient, configCache);
            });

            builder.Services.AddTransient<IDynamicService, DynamicService>(sp =>
            {
                var configCache = sp.GetRequiredService<IConfigCache>();
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new DynamicService(httpClient, configCache);
            });

            builder.Services.AddTransient<IOptionsService, OptionsService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new OptionsService(httpClient);
            });

            builder.Services.AddTransient<ModulesGetRequestHandler>();

            builder.Services.AddMediatR(typeof(Module).Assembly);

            await builder.Build().RunAsync();
        }
    }
}
