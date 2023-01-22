using Auth0.AspNetCore.Authentication;
using Headway.App.Blazor.Server.Extensions;
using Headway.Blazor.Controls.Services;
using Headway.Core.Cache;
using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Core.Notifications;
using Headway.RequestApi.Api;
using Headway.RequestApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using RemediatR.Core.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

builder.Services.AddMediatR(typeof(ModuleApiRequest).Assembly);

builder.Services.AddMudServices();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var identityProvider = builder.Configuration["IdentityProvider:DefaultProvider"];

if (identityProvider.Equals(IdentityProvider.IDENTITY_SERVER_4))
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = builder.Configuration[$"{identityProvider}:Authority"];
            options.ClientId = builder.Configuration[$"{identityProvider}:ClientId"];
            options.ClientSecret = builder.Configuration[$"{identityProvider}:ClientSecret"];
            options.ResponseType = "code";
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");
            options.Scope.Add("webapi");
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.ClaimActions.Add(new JsonKeyClaimAction("role", "role", "role"));
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };
        });
}
else if(identityProvider.Equals(IdentityProvider.AUTH_0))
{
    builder.Services
        .AddAuth0WebAppAuthentication(Auth0Constants.AuthenticationScheme, options =>
        {
            options.Domain = builder.Configuration[$"{identityProvider}:Domain"];
            options.ClientId = builder.Configuration[$"{identityProvider}:ClientId"];
            options.ClientSecret = builder.Configuration[$"{identityProvider}:ClientSecret"];
            options.ResponseType = "code";
        }).WithAccessToken(options =>
        {
            options.Audience = builder.Configuration[$"{identityProvider}:Audience"];
        });
}

builder.Services.AddHttpClient("webapi", client =>
{
    client.BaseAddress = new Uri("https://localhost:44320");
});

builder.Services.AddScoped<TokenProvider>();
builder.Services.AddSingleton<IAppCache, AppCache>();
builder.Services.AddSingleton<IConfigCache, ConfigCache>();
builder.Services.AddSingleton<IStateNotification, StateNotification>();
builder.Services.AddTransient<IShowDialogService, ShowDialogService>();
builder.Services.AddTransient<ConfigGetByNameRequestHandler>();
builder.Services.AddTransient<LogRequestHandler>();

builder.Services.AddTransient<IModuleApiRequest, ModuleApiRequest>(sp =>
{
    var tokenProvider = sp.GetRequiredService<TokenProvider>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("webapi");
    return new ModuleApiRequest(httpClient, tokenProvider);
});

builder.Services.AddTransient<IConfigurationApiRequest, ConfigurationApiRequest>(sp =>
{
    var configCache = sp.GetRequiredService<IConfigCache>();
    var tokenProvider = sp.GetRequiredService<TokenProvider>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("webapi");
    return new ConfigurationApiRequest(httpClient, tokenProvider, configCache);
});

builder.Services.AddTransient<IDynamicApiRequest, DynamicApiRequest>(sp =>
{
    var configCache = sp.GetRequiredService<IConfigCache>();
    var tokenProvider = sp.GetRequiredService<TokenProvider>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("webapi");
    return new DynamicApiRequest(httpClient, tokenProvider, configCache);
});

builder.Services.AddTransient<IOptionsApiRequest, OptionsApiRequest>(sp =>
{
    var tokenProvider = sp.GetRequiredService<TokenProvider>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("webapi");
    return new OptionsApiRequest(httpClient, tokenProvider);
});

builder.Services.AddTransient<ILogApiRequest, LogApiRequest>(sp =>
{
    var tokenProvider = sp.GetRequiredService<TokenProvider>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("webapi");
    return new LogApiRequest(httpClient, tokenProvider);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAdditionalAssemblies(new[] { typeof(Redress).Assembly });

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();