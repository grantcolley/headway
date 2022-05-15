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

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSerilog((hostingContext, loggerConfiguration) =>
                  loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Headway.WebApi", Version = "v1" });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (builder.Configuration.GetConnectionString(DataMigrations.CONNECTION_STRING).Contains(DataMigrations.SQLITE_DATABASE))
    {
        options.UseSqlite(builder.Configuration.GetConnectionString(DataMigrations.CONNECTION_STRING),
            x => x.MigrationsAssembly(DataMigrations.SQLITE_MIGRATIONS));
        options.EnableSensitiveDataLogging();
    }
    else
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString(DataMigrations.CONNECTION_STRING),
            x => x.MigrationsAssembly(DataMigrations.SQLSERVER_MIGRATIONS));
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IAuthorisationRepository, AuthorisationRepository>();
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<IOptionsRepository, OptionsRepository>();
builder.Services.AddScoped<IDemoModelRepository, DemoModelRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("local",
        builder =>
            builder.WithOrigins("https://localhost:44300", "https://localhost:44310")
                   .AllowAnyHeader()
                   .AllowAnyMethod());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";
        options.Audience = "webapi";
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
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

// Seed data for testing purposes only...
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
    SeedData.Initialise(applicationDbContext);
}

app.Run();