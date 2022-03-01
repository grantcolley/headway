using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Repository;
using Headway.Repository.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;

namespace Headway.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (Configuration.GetConnectionString(DataMigrations.CONNECTION_STRING).Contains(DataMigrations.SQLITE_DATABASE))
                {
                    options.UseSqlite(Configuration.GetConnectionString(DataMigrations.CONNECTION_STRING),
                        x => x.MigrationsAssembly(DataMigrations.SQLITE_MIGRATIONS));
                    options.EnableSensitiveDataLogging();
                }
                else
                {
                    options.UseSqlServer(Configuration.GetConnectionString(DataMigrations.CONNECTION_STRING),
                        x => x.MigrationsAssembly(DataMigrations.SQLSERVER_MIGRATIONS));
                    options.EnableSensitiveDataLogging();
                }
            });

            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IAuthorisationRepository, AuthorisationRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IOptionsRepository, OptionsRepository>();
            services.AddScoped<IDemoModelRepository, DemoModelRepository>();

            services.AddCors(options =>
            {
                options.AddPolicy("local",
                    builder =>
                        builder.WithOrigins("https://localhost:44300", "https://localhost:44310")                        
                               .AllowAnyHeader()
                               .AllowAnyMethod());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.Audience = "webapi";
                });

            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Headway.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Headway.WebApi v1"));
                app.UseSerilogRequestLogging();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("local");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
