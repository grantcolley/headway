using Headway.App.Blazor.WebAssembly;
using Headway.App.Blazor.WebAssembly.Account;
using Headway.App.Blazor.WebAssembly.Extensions;
using Headway.Blazor.Controls.Services;
using Headway.Core.Cache;
using Headway.Core.Interface;
using Headway.Core.Notifications;
using Headway.RequestApi.Api;
using Headway.RequestApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using RemediatR.Core.Model;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOidcAuthentication(options =>
{
    var identityProvider = builder.Configuration["IdentityProvider:DefaultProvider"];

    builder.Configuration.Bind(identityProvider, options.ProviderOptions);
    options.UserOptions.RoleClaim = "role";
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration[$"{identityProvider}:Audience"]);
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
builder.Services.AddTransient<IShowDialogService, ShowDialogService>();
builder.Services.AddTransient<ModulesGetRequestHandler>();
builder.Services.AddTransient<ConfigGetByNameRequestHandler>();
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

builder.Services.AddMediatR(typeof(ModuleApiRequest).Assembly);
builder.Services.AddMudServices();

builder.Services.UseAdditionalAssemblies(new[] { typeof(Redress).Assembly });

await builder.Build().RunAsync();