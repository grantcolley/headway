![Alt text](/readme-images/Headway-dark.png?raw=true "Headway") 

###### .NET 6.0, Blazor WebAssembly, Blazor Server, ASP.NET Core Web API, Auth0, IdentityServer4, MudBlazor, MediatR, Entity Framework Core, MS SQL Server, SQLite 
###### 
\
[![Build status](https://ci.appveyor.com/api/projects/status/xg606j7pr1ib54db?svg=true)](https://ci.appveyor.com/project/grantcolley/headway)
###### 

**Headway** is a framework for building configurable Blazor applications fast. It is based on the [blazor-solution-setup](https://github.com/grantcolley/blazor-solution-setup) project, providing a solution for a *Blazor* app supporting both hosting models, *Blazor WebAssembly* and *Blazor Server*, a *WebApi* for accessing data and an *Identity Provider* for authentication.

![Alt text](/readme-images/HeadwayDefaultSeedData.jpg?raw=true "Headway with Default Seed Data") 

## Table of Contents
* [The Framework](#the-framework)
* [Getting Started](#getting-started)
   * [Seed Data](#seed-data) 
   * [Building an Example Headway Application](#building-an-example-headway-application)
      * [Introduction to the Example Headway Application](#introduction-to-the-example-headway-application)
      * [Create the Example Headway Application](#create-the-example-headway-application)
         * [Create a Model](#create-a-model)
         * [Create Navigation](#create-navigation)
         * [Create Config for Rendering a List of Models](#create-config-for-rendering-a-list-of-models)
         * [Create Config for Rendering a Model](#create-config-for-rendering-a-model)
         * [Create a Flow](#create-a-flow)
         * [Bind the Flow to a Model](#bind-the-flow-to-a-model)
         * [Create Roles and Permissions](#create-roles-and-permissions)
         * [Create Users](#create-users)
         * [Bind Permissions to the Flow](#bind-permissions-to-the-flow)
         * [Capture History](#capture-history)
         * [Capture an Audit Trail](#capture-an-audit-trail)
* [Authentication](#authentication)
   * [Token-based Authentication](#token-based-authentication)
   * [Blazor Server vs Blazor WebAssembly](#blazor-server-vs-blazor-webassembly)
   * [Authorization Code Flow vs Authorization Code Flow with PKCE](#authorization-code-flow-vs-authorization-code-flow-with-pkce)
   * [Headway Authentication](#headway-authentication)
       * [Identity Providers](#identity-providers)
       * [Blazor WebAssembly](#blazor-webassembly)
       * [Blazor Server](#blazor-server)
       * [WebApi](#webapi)
       * [Other Implementation Examples for Identity Providers](#other-implementation-examples-for-identity-providers)
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

![Alt text](/readme-images/Architecture.png?raw=true "Headway Architecture") 

## Getting Started
### Seed Data
To help get you started the **Headway** *framework* comes with [seed data](https://github.com/grantcolley/headway/blob/main/src/Utilities/Headway.SeedData/CoreData.cs) that provides basic configuration for a default navigation menu, roles, permissions and a couple of users.
> The default seed data comes with two user accounts which will need to be registered with an [identity provider](#identity-providers) that will issue a token to the user containing a RoleClaim called `headwayuser`. The two default users are:
>  |User|Headway Role|Indentity Provider RoleClaim|
>  |----|------------|----------------------------|
>  |alice@email.com|Admin|headwayuser|
>  |grant@email.com|Developer|headwayuser|

The [database and schema](#database) can be created using EntityFramework Migrations.  

### Building an Example Headway Application
An example application will be created using Headway to demonstrate features built into the **Headway** framework including, creating a workflow, configuring dynamically rendered page layout that binds to the workflow, securing the application using **OAuth 2.0** authentication and restricting users access and functionality with by assigning roles and permissions.

#### Introduction to the Example Headway Application
The example application will be called **RemediatR**. Its purpose will be to refund customers that have been wronged in some way. The remediation flow will start with collection relevant customer and product data, then progress onto refund calculation, followed by sending a communication to the customer and finally end with payment of the refunded amount.

Different users will be responsible for different stages in the flow. They will be assigned a role to reflect their responsibility. The roles will be as follows:
-	**Remediation Owner** – monitors and progresses the case from start through to completion 
-	**Remediation Reviewer** – reviews the case as part of a four-eyes check, at critical points
-	**Finance User** – calculates the refund including any compensatory interest due
-	**Finance Reviewer** – reviews the refund calculated as part of a four-eyes check 
-	**Remunerator** – processes the refund payment

The RemediatR Flow is as follows:

![Alt text](/readme-images/StandardRemediationFlow.png?raw=true "Standard Remediation Flow") 

### Create the Example Headway Application
#### Create a Model
#### Create Navigation
#### Create Config for Rendering a List of Models
#### Create Config for Rendering a Model
#### Create a Flow
#### Bind the Flow to a Model
#### Create Roles and Permissions
#### Create Users
#### Bind Permissions to the Flow
#### Capture History
#### Capture an Audit Trail

### Token-based Authentication
Blazor applications use [token-based authentication](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-6.0#token-based-authentication) based on digitally signed [JSON Web Tokens (JWTs)](https://jwt.io/introduction), which is a safe means of representing claims that can be transferred between parties.
Token-based authentication involves an authorization server issuing an athenticated user with a token containing claims, which can be sent to a resource such as a WebApi, with an extra `authorization` header in the form of a `Bearer` token. This allows the WebApi to validate the claim and provide the user access to the resource.

[**Headway.WebApi**](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/Program.cs) authentication is configured for the `Bearer` *Authenticate* and *Challenge* scheme. **JwtBearer** middleware is added to validate the token based on the values of the `TokenValidationParameters`, *ValidIssuer* and *ValidAudience*.  
```C#
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var identityProvider = builder.Configuration["IdentityProvider:DefaultProvider"];

    options.Authority = $"https://{builder.Configuration[$"{identityProvider}:Domain"]}";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration[$"{identityProvider}:Domain"],
        ValidAudience = builder.Configuration[$"{identityProvider}:Audience"]
    };
});
```

Blazor applications obtain a token from an Identity Provider using an *authorization flow*. The type of *flow* used depends on the Blazor hosting model.

### Blazor Server vs Blazor WebAssembly
> [ASP.NET Core Blazor authentication and authorization](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/).
> \
> "Security scenarios differ between Blazor Server and Blazor WebAssembly apps. Because Blazor Server apps run on the server, authorization checks are able to determine:
> - The UI options presented to a user (for example, which menu entries are available to a user).
> - Access rules for areas of the app and components.
>
>Blazor WebAssembly apps run on the client. Authorization is only used to determine which UI options to show. Since client-side checks can be modified or bypassed by a user, a Blazor WebAssembly app can't enforce authorization access rules. "

[**Blazor Server**](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/server/) uses *Authorization Code Flow* in which a `Client Secret` is passed in the exchange. It can do this because it is a *'regular web application'* where the source code and `Client Secret` is securely stored *server-side* and not publicly exposed.

[**Blazor WebAssembly**](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/) uses *Authorization Clode Flow with Proof of Key for Code Exchange (PKCE)*, which introduces a secret created by the calling application that can be verified by the authorization server. The secret is called the `Code Verifier`. It must do this because the entire source is stored in the browser so it cannot use a `Client Secret` because it is not secure.

> The key difference between **Blazor Server** using the *Authorization Code Flow* and **Blazor WebAssembly** using the *Authorization Clode Flow with Proof of Key for Code Exchange (PKCE)*, is **Blazor Server** can use a `Client Secret` in the exchange because it can be securely stored on the server. **Blazor WebAssembly** on the other hand cannot securely store a `Client Secret` so it has to create a `code_verifier` and then generate a `code_challenge` from it, which can be used in the exchange instead.

### Authorization Code Flow vs Authorization Code Flow with PKCE
[Authorization Code Flow](https://auth0.com/docs/get-started/authentication-and-authorization-flow/authorization-code-flow) steps:
1. User clicks login in the application.
2. The user is redirected to the authorization server (`/authorize` endpoint).
3. The authorization server redirects the user to a login prompt.
4. The user authenticates.
5. the authorization server redirects the user back to the application with an `authorization code`, which can only be used once.
6. The application sends the `authorization code` along with the applications `Client ID` and `Client Secret` to the authorization server (`/oauth/token` endpoint).
7. The authorization server verifies the `authorization code`, `Client ID` and `Client Secret`.
8. The authorization server sends to the application an `ID Token` and `Access Token` (and optionally, a `Refresh Token`) .
9. The `Access Token` contains user claims.
10. When the application wants to access a resource such as a WebApi it adds the `Access Token` containing user claims to the authorization header of a `HttpClient` request in the form of a `Bearer` token.

[Authorization Clode Flow with Proof of Key for Code Exchange (PKCE)](https://auth0.com/docs/get-started/authentication-and-authorization-flow/authorization-code-flow-with-proof-key-for-code-exchange-pkce) steps:
\
The *PKCE Authorization Code Flow* builds on the standard *Authentication Code Flow* so it has very similar steps.
1. User clicks login in the application.
2. The application creates a `code_verifier` and then generates a `code_challenge` from it.
3. The user is redirected to the authorization server (`/authorize` endpoint) along with the `code_challenge`.
4. The authorization server redirects the user to a login prompt.
5. The user authenticates.
6. the authorization server stores the `code_challenge` and then redirects the user back to the application with an `authorization code`, which can only be used once.
7. The application sends the `authorization code` along with the `code_verifier` (created in step 2.) to the authorization server (`/oauth/token` endpoint).
8. The authorization server verifies the `code_challenge` and `code_verifier`.
9. The authorization server sends to the application an `ID Token` and `Access Token` (and optionally, a `Refresh Token`). The `Access Token` contains user claims.
11. When the application wants to access a resource such as a WebApi it adds the `Access Token` containing user claims to the authorization header of a `HttpClient` request in the form of a `Bearer` token.

### Headway Authentication
#### Identity Providers
To access resources via the **Headway.WebApi** the authentication server must issue a token to the user containing a RoleClaim called `headwayuser` and the users `email`. The application can then access further information about the user from the **Headway.WebApi** to determine what the user is authorised to do e.g. **Headway.WebApi** will return the menu items to build up the navigation panel. If a user does not have permission to access a menu item then **Headway.WebApi** simply wont return it.

Headway currently supports authentication from two identity providers **IdentityServer4** and **Auth0**. During development you can toggle between them by setting `IdentityProvider:DefaultProvider` in the *appsettings.json* files for [Headway.BlazorServerApp](https://github.com/grantcolley/headway/blob/main/src/Headway.BlazorServerApp/appsettings.json), [Headway.BlazorWebassemblyApp](https://github.com/grantcolley/headway/blob/main/src/Headway.BlazorWebassemblyApp/wwwroot/appsettings.json) and [Headway.WebApi](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/appsettings.json) e.g.
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
   - Accessible only to authenticated users carrying the `headwayuser` role claim and controllers are embelished with the `[Authorize(Roles="headwayuser")]` attribute.
   - A further check is made on every request using the `email` claim to confirm the user has the relevant *Headway role or permission* required to access to resource being requested.

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
Data access is abstracted behind interfaces. **Headway.Repository** provides concrete implementation for the data access layer interfaces. it currently supports **MS SQL Server** and **SQLite**, however this can be extended to any data store supported by *EntityFramework Core*.

> **Headway.Repository** is not limited to *EntityFramework Core* and can be replaced with a completely different data access implementation.

Add the connection string to [appsettings.json](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/appsettings.json) of **Headway.WebApi**.
> Note Headway will know whether you are pointing to **SQLite** or a **MS SQL Server** database based on the connection string. This can be extended in [DesignTimeDbContextFactory.cs](https://github.com/grantcolley/headway/blob/main/src/Headway.Repository/Data/DesignTimeDbContextFactory.cs) to use other databases if required.

```C#
  "ConnectionStrings": {

    /* SQLite*/
    /*"DefaultConnection": "Data Source=..\\..\\db\\Headway.db;"*/
    
    /* MS SQL Server*/
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Headway;Trusted_Connection=True;"
    
  }
````

Create the database and schema using [EF Core migrations](#entityframework-core-migrations) in [Headway.MigrationsSqlServer](https://github.com/grantcolley/headway/tree/main/src/Utilities/Headway.MigrationsSqlServer) or [MigrationsSqlite](https://github.com/grantcolley/headway/tree/main/src/Utilities/Headway.MigrationsSqlite), depending on which database you choose. If you are using Visual Studio in the `Developer PowerShell` navigate to **Headway.WebApi** folder and run the following:
![Alt text](/readme-images/EFCoreMigrations.jpg?raw=true "Headway EF Core Migrations") 

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



