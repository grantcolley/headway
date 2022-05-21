![Alt text](/readme-images/Headway-dark.png?raw=true "Headway") 

###### .NET 6.0, Blazor WebAssembly, Blazor Server, Auth0, IdentityServer4, ASP.NET Core Web API, MudBlazor, MediatR, Entity Framework Core, MS SQL Server, SQLite 
###### 
\
[![Build status](https://ci.appveyor.com/api/projects/status/xg606j7pr1ib54db?svg=true)](https://ci.appveyor.com/project/grantcolley/headway)
###### 
\
**Headway** is a framework for building configurable Blazor applications fast. It is based on the [blazor-solution-setup](https://github.com/grantcolley/blazor-solution-setup) project, providing a solution for a *Blazor* app supporting both hosting models, *Blazor WebAssembly* and *Blazor Server*, a *WebApi* for accessing data and an *Identity Provider* for authentication.
 
![Alt text](/readme-images/Architecture.png?raw=true "Headway Architecture") 

## Table of Contents
* [The Framework](#the-framework)
* [Getting Started](#getting-started)
   * [Build a New Application](#build-a-new-application)
   * [Create the Models](#create-the-models)
   * [Add a Module](#add-a-module)
   * [Add a Search Screen](#add-a-search-screen)
   * [Create a Workflow](#create-a-workflow)
   * [Add Permissions](#add-permissions)
* [Authentication](#authentication)
* [Authorization](#authorization)
* [Page Layout](#page-layout)
   * [Page Rendering](#page-rendering) 
* [Navigation Menu](#navigation-menu)
* [Components](#components)
   * [Standard Components](#standard-components)
   * [Generic Components](#generic-components)
   * [Specialized Components](#specialized-components)
   * [Communication Between Components](#communication-between-components)
        * [StateNotificationMediator](#statenotificationmediator)
        * [Linked Components](#linked-components) 
* [Configuration](#configuration)
* [Administration](#administration)
* [Database](#database)
* [Notes](#notes)
    * [Adding font awesome](#adding-font-awesome)
    * [EntityFramework Core Migrations](#entityframework-core-migrations)
    * [Handle System.Text.Json Circular Reference Errors](#handle-systemtextjson-circular-reference-errors)
    * [Make ASP.Net Core use Json.Net](#make-aspnet-core-use-jsonnet)

## The Framework
 * **Headway.BlazorWebassemblyApp** - Blazor WASM running client-side on the browser.
 * **Headway.BlazorServerApp** - Blazor Server running updates and event handling on the server over a SignalR connection. 
 * **Headway.Razor.Shared** - A *Razor Class Library* with shared components and functionality serving both Blazor hosting models. 
 * **Headway.Razor.Controls** - A *Razor Class Library* containing common *Razor* components.
 * **Headway.Core** - A *Class Library* for shared classes and interfaces.
 * **Headway.RequestApi** - a *Class Library* for handling requests to the *WebApi*.
 * **Headway.WebApi** - An *ASP.NET Core Web API* for authenticated users to access data persisted in the data store.
 * **Headway.Repository** - a *Class Library* for accessing the data store behind the *WebApi*.
 * **Identity Provider** - An *IdentityServer4* ASP.NET Core Web API, providing an OpenID Connect and OAuth 2.0 framework, for authentication.

## Getting Started
### Build a New Application

### Create the Models

### Add a Module

### Add a Search Screen

### Create a Workflow

### Add Permissions

## Authentication

#### Blazor Server vs Blazor WebAssembly
> [ASP.NET Core Blazor authentication and authorization](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/).
> \
> "Security scenarios differ between Blazor Server and Blazor WebAssembly apps. Because Blazor Server apps run on the server, authorization checks are able to determine:
> - The UI options presented to a user (for example, which menu entries are available to a user).
> - Access rules for areas of the app and components.
>
>Blazor WebAssembly apps run on the client. Authorization is only used to determine which UI options to show. Since client-side checks can be modified or bypassed by a user, a Blazor WebAssembly app can't enforce authorization access rules. "

Blazor applications use [token-based authentication](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-6.0#token-based-authentication) based on digitally signed [JSON Web Tokens (JWTs)](https://self-issued.info/docs/draft-ietf-oauth-json-web-token.html), which is safe means of representing claims that can be transferred between parties.
Token-based authentication involves an authorization server issuing an athenticated user with a token containing claims, which can be sent to a resource such as a WebApi, with an extra authorization header in the form of a Bearer token. This allows the WebApi to validate the claim and provide the user access to the resource.

The Blazor application obtains the token from the Identity Provider using an *authorization flow*. The type of *flow* used depends on the Blazor hosting model.

[**Blazor Server**](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/server/) uses *Authorization Code Flow* in which a `Client Secret` is passed in the exchange. It can do this because it is a *regular web application* where the source code and `Client Secret` is securely stored *server-side* and not publicly exposed.

[**Blazor WebAssembly**](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/) uses *Authorization Clode Flow with Proof of Key for Code Exchange (PKCE)* which introduces a secret created by the calling application called the `Code Verifier`, that can be verified by the authorization server. It must do this because the entire source is stored in the browser and therefore cannot securely store a `Client Secret`.

> The key difference between **Blazor Server** using the *Authorization Code Flow* and **Blazor WebAssembly** using the *Authorization Clode Flow with Proof of Key for Code Exchange (PKCE)*, is **Blazor Server** uses in the exchange the `Client Secret` which is securely stored on the server. **Blazor WebAssembly** on the other hand, has to create a `code_verifier` and generate a `code_challenge` from it, which can be used in the exchange.

#### Authorization Code Flow vs Authorization Code Flow with PKCE
[Authorization Code Flow](https://auth0.com/docs/get-started/authentication-and-authorization-flow/authorization-code-flow) steps:
1. User clicks login in the application
2. The user is redirected to the authorization server (`/authorize` endpoint)
3. The authorization server redirects the user to a login prompt
4. The user authenticates
5. the authorization server redirects the user back to the application with an `authorization code`, which can be used once
6. The applications send the `authorization code` along with the applications `Client ID` and `Client Secret` to the authorization server (`/oauth/token` endpoint)
7. The authorization server verifies the `authorization code`, `Client ID` and `Client Secret`
8. The authorization server responds with an `ID Token` and `Access Token` (and optionally, a `Refresh Token`) 
9. The application adds the `Access Token` to the authorization header in the form of a `Bearer` token of an `HttpClient` request to a WebApi to access a resource

[Authorization Clode Flow with Proof of Key for Code Exchange (PKCE)](https://auth0.com/docs/get-started/authentication-and-authorization-flow/authorization-code-flow-with-proof-key-for-code-exchange-pkce) steps:
\
The *PKCE Authorization Code Flow* has very similar steps because it builds on the standard *Authentication Code Flow*.
1. User clicks login in the application
2. The application creates a `code_verifier` and generates a `code_challenge` from it
3. The user is redirected to the authorization server (`/authorize` endpoint) along with the `code_challenge`
4. The authorization server redirects the user to a login prompt
5. The user authenticates
6. the authorization server stores `code_challenge` and then redirects the user back to the application with an `authorization code`, which can be used once
7. The applications send the `authorization code` along with the `code_verifier` (created in step 2.) to the authorization server (`/oauth/token` endpoint)
8. The authorization server verifies the `code_challenge` and `code_verifier`
9. The authorization server responds with an `ID Token` and `Access Token` (and optionally, a `Refresh Token`) 
10. The application adds the `Access Token` to the authorization header in the form of a `Bearer` token of an `HttpClient` request to a WebApi to access a resource

#### Identity Providers
Authentication is done through an Identity Provider that returns a token containing a RoleClaim called `headwayuser`.

Headway currently supports authentication from two identity providers **IdentityServer4** and **Auth0**. You can toggle between them by setting `IdentityProvider:DefaultProvider` in the *appsettings.json* files for [Headway.BlazorServerApp](https://github.com/grantcolley/headway/blob/main/src/Headway.BlazorServerApp/appsettings.json), [Headway.BlazorWebassemblyApp](https://github.com/grantcolley/headway/blob/main/src/Headway.BlazorWebassemblyApp/wwwroot/appsettings.json) and [Headway.WebApi](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/appsettings.json) e.g.
```C#
  "IdentityProvider": {
    "DefaultProvider": "Auth0"
  },
```

#### Blazor WebAssembly
   - UserAccountFactory converts the RemoteUserAccount into a ClaimPrincipal for the application
   - AuthorizationMessageHandler attaches token to outgoing HttpClient requests 

#### Blazor Server
   - InitialApplicationState gets the access_token, refresh_token and id_token from the HttpContext after authentication and stores them in a scoped TokenProvider
   - The scoped TokenProvider is manually injected into each request class and the bearer token is added to the Authorization header of outgoing HttpClient requests

#### WebApi
   - Controllers have role authorisation

#### Other Implementation Examples for Identity Providers
   - For **IdentityServer4** see [blazor-solution-setup](https://github.com/grantcolley/blazor-solution-setup).
   - For **Auth0** see [blazor-auth0](https://github.com/grantcolley/blazor-auth0).

## Authorization

## Page Layout
![Alt text](/readme-images/Layout.drawio.png?raw=true "Page Layout")

### Page Rendering
![Alt text](/readme-images/PageRenderHierarchy.drawio.png?raw=true "Page Render Hierarchy")

## Navigation Menu

## Components
### Standard Components

### Generic Components

### Specialized Components

### Communication Between Components

#### StateNotificationMediator

#### Linked Components 

## Configuration

## Administration

## Database

## Notes

### Adding Font Awesome
 * Right-click the `wwwroot\css` folder in the Blazor project and click `Add` then `Client-Side Library...`. Search for `font-awesome` and install it.
 * For a Blazor Server app add `@import url('font-awesome/css/all.min.css');` at the top of [site.css](https://github.com/grantcolley/headway/tree/main/src/Headway.BlazorServerApp/wwwroot/css).
 * For a Blazor WebAssembly app adding `@import url('font-awesome/css/all.min.css');` to [app.css](https://github.com/grantcolley/headway/blob/main/src/Headway.BlazorWebassemblyApp/wwwroot/css/app.css) didn't work. Instead add `<link href="css/font-awesome/css/all.min.css" rel="stylesheet" />` to [index.html](https://github.com/grantcolley/headway/blob/main/src/Headway.BlazorWebassemblyApp/wwwroot/index.html).
  
### EntityFramework Core Migrations
Migrations are kept in separate projects from the [ApplicationDbContext](https://github.com/grantcolley/headway/blob/main/src/Headway.Repository/Data/ApplicationDbContext.cs).
The **ApplicationDbContext** is in the [Headway.Repository](https://github.com/grantcolley/headway/tree/main/src/Headway.Repository) library, which is referenced by [Headway.WebApi](https://github.com/grantcolley/headway/tree/main/src/Headway.WebApi). When running migrations from **Headway.WebApi**, the migrations are output to either [Headway.MigrationsSqlite](https://github.com/grantcolley/headway/tree/main/src/Utilities/Headway.MigrationsSqlite) or [Headway.MigrationsSqlServer](https://github.com/grantcolley/headway/tree/main/src/Utilities/Headway.MigrationsSqlServer), depending on which connection string is used in **Headway.WebApi**'s [appsettings.json](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/appsettings.json). For this to work, a [DesignTimeDbContextFactory](https://github.com/grantcolley/headway/blob/main/src/Headway.Repository/Data/DesignTimeDbContextFactory.cs) class must be created in **Headway.Repository**. This allows migrations to be created for a *DbContext* that is in a project other than the startup project **Headway.WebApi**. **DesignTimeDbContextFactory** specifies which project the migration output should target based on the connection string in **Headway.WebApi**'s **appsettings.json**.

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
*Note: I had to do this after noticing `System.Text.Json` nulled out duplicate string values after setting `ReferenceHandler.IgnoreCycles`.*
```C#
            services.AddControllers()
                .AddNewtonsoftJson(options => 
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
```



