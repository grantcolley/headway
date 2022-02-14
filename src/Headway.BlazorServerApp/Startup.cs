using Headway.Core.Cache;
using Headway.Core.Interface;
using Headway.Core.Mediators;
using Headway.Core.Model;
using Headway.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace Headway.BlazorServerApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = "https://localhost:5001/";
                    options.ClientId = "headwayblazorserverapp";
                    options.ClientSecret = "headwayblazorserverappsecret";
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

            services.AddHttpClient("webapi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44320");
            });

            services.AddScoped<TokenProvider>();
            services.AddSingleton<IAppCache, AppCache>();
            services.AddSingleton<IConfigCache, ConfigCache>();
            services.AddSingleton<IStateNotificationMediator, StateNotificationMediator>();

            services.AddTransient<IModuleService, ModuleService>(sp =>
            {
                var tokenProvider = sp.GetRequiredService<TokenProvider>();
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new ModuleService(httpClient, tokenProvider);
            });

            services.AddTransient<IConfigurationService, ConfigurationService>(sp =>
            {
                var configCache = sp.GetRequiredService<IConfigCache>();
                var tokenProvider = sp.GetRequiredService<TokenProvider>();
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new ConfigurationService(httpClient, tokenProvider, configCache);
            });

            services.AddTransient<IDynamicService, DynamicService>(sp =>
            {
                var configCache = sp.GetRequiredService<IConfigCache>();
                var tokenProvider = sp.GetRequiredService<TokenProvider>();
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new DynamicService(httpClient, tokenProvider, configCache);
            });

            services.AddTransient<IOptionsService, OptionsService>(sp =>
            {
                var tokenProvider = sp.GetRequiredService<TokenProvider>();
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("webapi");
                return new OptionsService(httpClient, tokenProvider);
            });

            services.AddTransient<ModulesRequestHandler>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMediatR(typeof(Module).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
