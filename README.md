# headway

[![Build status](https://ci.appveyor.com/api/projects/status/xg606j7pr1ib54db?svg=true)](https://ci.appveyor.com/project/grantcolley/headway)

##### Technologies
###### .NET 6.0, Blazor WebAssembly, Blazor Server, IdentityServer4, ASP.NET Core Web API
\
**Headway** is based on the [blazor-solution-setup](https://github.com/grantcolley/blazor-solution-setup) project, providing a solution for a *Blazor* app supporting both hosting models, *Blazor WebAssembly* and *Blazor Server*, a *WebApi* for accessing data and an *Identity Provider* for authentication:
 * **Blazor WebAssembly** - running client-side on the browser.
 * **Blazor Server** - where updates and event handling are run on the server and managed over a SignalR connection. 
 * **IdentityServer4** - an OpenID Connect and OAuth 2.0 framework for authentication. 
 * **ASP.NET Core Web API** - for accessing data repositories by authenticated users.
 * **Razor Class Library** - for shared *Razor* components.
 * **Class Library** - for shared classes and interfaces.
 * **Class Library** - a services library for calling the *WebApi*.
 * **Class Library** - a repository library for access to data behind the *WebApi*.

![Alt text](/readme-images/headway-architecture.png?raw=true "Headway Architecture") 

#### Table of Contents
* [Getting Started](#getting-started)
* [Authentication and Authorization](#authentication-and-authorization)
* [Database](#database)
* [Navigation Menu](#navigation-menu)
* [Administration](#administration)
* [Configuration](#configuration)
 * [Notes](#notes)
    * [Adding font awesome](#adding-font-awesome)
    * [EntityFramework Core Migrations](#entityframework-core-migrations)
    * [Handle System.Text.Json Circular Reference Errors](#handle-systemtextjson-circular-reference-errors)
    * [Make ASP.Net Core use Json.Net](#make-aspnet-core-use-jsonnet)

## Getting Started

## Authentication and Authorization
Identity Provider authenticates user
OidcConfiguration
Token contains a RoleClaim
WebAssembly 
   - UserAccountFactory converts the RemoteUserAccount into a ClaimPrincipal for the application
   - AuthorizationMessageHandler attaches token to outgoing HttpClient requests 

BlazorServer 
   - InitialApplicationState gets the access_token, refresh_token and id_token from the HttpContext after authentication ans stores them in a scoped TokenProvider
   - The scoped TokenProvider is manually injected into each service and the bearer token is added to the Authorization header of outgoing HttpClient requests

WebApi
   - Controllers require role authorisation

## Database

## Navigation Menu

## Administration

## Configuration

## Notes

### Adding Font Awesome
 * Right-click the `wwwroot\css` folder in the Blazor project and click `Add` then `Client-Side Library...`. Search for `font-awesome` and install it.
 * For a Blazor Server app add `@import url('font-awesome/css/all.min.css');` at the top of [site.css](https://github.com/grantcolley/headway/tree/main/src/Headway.BlazorServerApp/wwwroot/css).
 * For a Blazor WebAssembly app adding `@import url('font-awesome/css/all.min.css');` to [app.css](https://github.com/grantcolley/headway/blob/main/src/Headway.BlazorWebassemblyApp/wwwroot/css/app.css) didn't work. Instead add `<link href="css/font-awesome/css/all.min.css" rel="stylesheet" />` to [index.html](https://github.com/grantcolley/headway/blob/main/src/Headway.BlazorWebassemblyApp/wwwroot/index.html).
  
### EntityFramework Core Migrations
Migrations are kept in separate projects from the [ApplicationDbContext](https://github.com/grantcolley/headway/blob/main/src/Headway.Repository/Data/ApplicationDbContext.cs).
The **ApplicationDbContext** is in the [Headway.Repository](https://github.com/grantcolley/headway/tree/main/src/Headway.Repository) library, which is referenced by [Headway.WebApi](https://github.com/grantcolley/headway/tree/main/src/Headway.WebApi). When running migrations from **Headway.WebApi**, the migrations are output to either [Headway.MigrationsSqlite](https://github.com/grantcolley/headway/tree/main/src/Utilities/Headway.MigrationsSqlite) or [Headway.MigrationsSqlServer](https://github.com/grantcolley/headway/tree/main/src/Utilities/Headway.MigrationsSqlServer), depending on which connection string is used in **Headway.WebApi**'s [appsettings.json](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/appsettings.json). For this to work, a [DesignTimeDbContextFactory](https://github.com/grantcolley/headway/blob/main/src/Headway.Repository/Data/DesignTimeDbContextFactory.cs) class must be created in **Headway.Repository**. This allows migrations to be created for a *DbContext* that is in a project other than the startup project, **Headway.WebApi**. **DesignTimeDbContextFactory** specifies which project the migration output should target based on the connection string in **Headway.WebApi**'s **appsettings.json**.

```C#
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration
                = new ConfigurationBuilder().SetBasePath(
                    Directory.GetCurrentDirectory())
                       .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Headway.WebApi/appsettings.json")
                       .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if(connectionString.Contains("Headway.db"))
            {
                builder.UseSqlite(connectionString, x => x.MigrationsAssembly("Headway.MigrationsSqlite"));
            }
            else
            {
                builder.UseSqlServer(connectionString, x => x.MigrationsAssembly("Headway.MigrationsSqlServer"));
            }

            return new ApplicationDbContext(builder.Options);
        }
    }
```

**Headway.WebApi**'s [Startup.cs](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/Startup.cs) should also specify which project the migration output should target base on the connection string.

```C#
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (Configuration.GetConnectionString("DefaultConnection").Contains("Headway.db"))
                {
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"),
                        x => x.MigrationsAssembly("Headway.MigrationsSqlite"));
                }
                else
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                        x => x.MigrationsAssembly("Headway.MigrationsSqlServer"));
                }
            });
```

In the Developer PowerShell window navigate to the Headway.WebApi project and manage migrations by running the following command:

Add a new migration:
\
\
` dotnet ef migrations add UpdateHeadway --project ..\\Utilities\\Headway.MigrationsSqlServer`

Update the database with the latest migrations. It will also create the database if it hasn't already been created:
\
\
`dotnet ef database update --project ..\\Utilities\\Headway.MigrationsSqlServer`

Remove the latest migration:
\
\
` dotnet ef migrations remove --project ..\\Utilities\\Headway.MigrationsSqlServer`

**Supporting notes:**
 * Create migrations from the repository library and output them to a separate migrations projects 
 * https://medium.com/oppr/net-core-using-entity-framework-core-in-a-separate-project-e8636f9dc9e5
 * https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects?tabs=dotnet-core-cli
  
### Handle System.Text.Json Circular Reference Errors
`Newtonsoft.Json (Json.NET)` has been removed from the ASP.NET Core shared framework. The default JSON serializer for ASP.NET Core is now `System.Text.Json`, which is new in .NET Core 3.0.

Entity Framework requires the `Include()` method to specify related entities to include in the query results. An example is `GetUserAsync` in [AuthorisationRepository](https://github.com/grantcolley/headway/blob/main/src/Headway.Repository/AuthorisationRepository.cs).

```C#
        public async Task<User> GetUserAsync(string claim, int userId)
        {
            var user = await applicationDbContext.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.UserId.Equals(userId))
                .ConfigureAwait(false);
            return user;
        }
```

The query results will now contain a circular reference, where the parent references the child which references parent and so on. In order for `System.Text.Json` to handle de-serialising objects contanining circular references we have to set `JsonSerializerOptions.ReferenceHandler` to [IgnoreCycle](https://github.com/dotnet/runtime/issues/40099) in the **Headway.WebApi**'s [Startup](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/Startup.cs) class. If we don't explicitly specify that circular references should be ignored **Headway.WebApi** will return `HTTP Status 500 Internal Server Error`.

```C#
            services.AddControllers()
                .AddJsonOptions(options => 
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
```

### Make ASP.Net Core use Json.Net
The default JSON serializer for ASP.NET Core is now `System.Text.Json`. However, `System.Text.Json` is new and might currently be missing features supported by `Newtonsoft.Json (Json.NET)`. 
\
I [reported a bug in System.Text.Json](https://github.com/dotnet/aspnetcore/issues/34069) where duplicate values are nulled out when setting `JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles`. 

[How to specify ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.0&tabs=visual-studio#jsonnet-support) use `Newtonsoft.Json (Json.NET)` as the JSON serializer install [Microsoft.AspNetCore.Mvc.NewtonsoftJson](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.NewtonsoftJson) and the following to the [Startup](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/Startup.cs) of [Headway.WebApi](https://github.com/grantcolley/headway/tree/main/src/Headway.WebApi):
\
*Note: I had to do this after noticing `Syste.Text.Json` nulled out duplicate string values after setting `ReferenceHandler.IgnoreCycles`.*
```C#
            services.AddControllers()
                .AddNewtonsoftJson(options => 
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
```



