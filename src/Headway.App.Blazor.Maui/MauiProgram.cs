using Headway.App.Blazor.Maui.Authentication;
using Headway.App.Blazor.Maui.DevHttp;
using Headway.App.Blazor.Maui.Extensions;
using Headway.Blazor.Controls.Services;
using Headway.Core.Cache;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Core.Notifications;
using Headway.RequestApi.Api;
using Headway.RequestApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using MudBlazor.Services;
using RemediatR.Core.Model;
using System.Net.Http;
using System.Reflection;

namespace Headway.App.Blazor.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMediatR(typeof(ModuleApiRequest).Assembly);
            builder.Services.AddMudServices();

            string appSettings = string.Empty;

#if DEBUG
            appSettings = "Headway.App.Blazor.Maui.appsettings.Development.json";

		    builder.Services.AddBlazorWebViewDeveloperTools();
#else
            appSettings = "Headway.App.Blazor.Maui.appsettings.json";
#endif

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MauiProgram)).Assembly;
            using (var stream = assembly.GetManifestResourceStream(appSettings))
            {
                builder.Configuration.AddJsonStream(stream);
            }

            var identityProvider = builder.Configuration["IdentityProvider:DefaultProvider"];

            builder.Services.AddAuthorizationCore();
            builder.Services.AddSingleton<TokenProvider>();
            builder.Services.AddScoped<CustomAuthenticationStateProviderOptions>();
            builder.Services.AddScoped<CustomAuthenticationStateProvider>();

            builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
            {
                var tokenProvider = sp.GetRequiredService<TokenProvider>();
                var auth0AuthenticationStateProviderOptions = sp.GetRequiredService<CustomAuthenticationStateProviderOptions>();

                auth0AuthenticationStateProviderOptions.Scope = "openid profile";
                auth0AuthenticationStateProviderOptions.Authority = builder.Configuration[$"{identityProvider}:Authority"];
                auth0AuthenticationStateProviderOptions.ClientId = builder.Configuration[$"{identityProvider}:ClientId"];
                auth0AuthenticationStateProviderOptions.RoleClaim = builder.Configuration[$"{identityProvider}:RoleClaim"];
                auth0AuthenticationStateProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration[$"{identityProvider}:Audience"]);

#if WINDOWS
                // https://github.com/dotnet/maui/issues/8382
                auth0AuthenticationStateProviderOptions.RedirectUri = builder.Configuration[$"{identityProvider}:RedirectUri"];
                auth0AuthenticationStateProviderOptions.PostLogoutRedirectUris = builder.Configuration[$"{identityProvider}:PostLogoutRedirectUri"];
#else
                auth0AuthenticationStateProviderOptions.RedirectUri = builder.Configuration[$"{identityProvider}:RedirectUri"];
                auth0AuthenticationStateProviderOptions.PostLogoutRedirectUris = builder.Configuration[$"{identityProvider}:PostLogoutRedirectUri"];
#endif

                return sp.GetRequiredService<CustomAuthenticationStateProvider>();
            });

#if DEBUG
            builder.Services.AddDevHttpClient("webapi", 7225);
#else
            builder.Services.AddHttpClient("webapi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7225");
            });
#endif

            builder.Services.AddSingleton<IAppCache, AppCache>();
            builder.Services.AddSingleton<IConfigCache, ConfigCache>();
            builder.Services.AddSingleton<IStateNotification, StateNotification>();
            builder.Services.AddTransient<IShowDialogService, ShowDialogService>();
            builder.Services.AddTransient<ModulesGetRequestHandler>();
            builder.Services.AddTransient<ConfigGetByNameRequestHandler>();
            builder.Services.AddTransient<OptionItemsRequestHandler>();
            builder.Services.AddTransient<LogRequestHandler>();

            builder.Services.AddTransient<IModuleApiRequest, ModuleApiRequest>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new ModuleApiRequest(httpClient);
            });

            builder.Services.AddTransient<IConfigurationApiRequest, ConfigurationApiRequest>(sp =>
            {
                var configCache = sp.GetRequiredService<IConfigCache>();
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new ConfigurationApiRequest(httpClient, configCache);
            });

            builder.Services.AddTransient<IDynamicApiRequest, DynamicApiRequest>(sp =>
            {
                var configCache = sp.GetRequiredService<IConfigCache>();
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new DynamicApiRequest(httpClient, configCache);
            });

            builder.Services.AddTransient<IOptionsApiRequest, OptionsApiRequest>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new OptionsApiRequest(httpClient);
            });

            builder.Services.AddTransient<ILogApiRequest, LogApiRequest>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new LogApiRequest(httpClient);
            });

            builder.Services.UseAdditionalAssemblies(new[] { typeof(Redress).Assembly });

            return builder.Build();
        }
    }
}