![Alt text](/readme-images/Headway-dark.png?raw=true "Headway") 

###### .NET 7.0, Blazor WebAssembly, Blazor Server, ASP.NET Core Web API, Auth0, IdentityServer4, MudBlazor, Entity Framework Core, MS SQL Server, SQLite 
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
   * [Introduction to RemediatR](#introduction-to-remediatr)
   * [Building RemediatR in Easy Steps](#building-remediatr-in-easy-steps)
   * [Create](#create)
      * [1. Create the RemediatR Projects](#1-create-the-remediatr-projects)
      * [2. Create the Models and Interfaces](#2-create-the-models-and-interfaces)
      * [3. Create the Repository](#3-create-the-repository)
      * [4. Create WebApi Access](#4-create-webapi-access)
      * [5. Create the WebApi Controllers](#5-create-the-webapi-controllers)
      * [6. Create model Options](#6-create-model-options)
      * [7. Create model validation](#7-create-model-validation)
   * [Reference](#reference)      
   * [Configure](#configure)
     * [1. Configure Authorisation](#1-configure-authorisation)
     * [2. Configure Navigation](#2-configure-navigation)
     * [3. Configure Model Layout](#3-configure-model-layout)
     * [4. Configure a Flow](#4-configure-a-flow)
     * [5. Bind the Flow to a Model](#5-bind-the-flow-to-a-model)
     * [6. Bind Permissions to the Flow](#6-bind-permissions-to-the-flow)
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
* [Tracking Changes](#tracking-changes)
* [Logging](#logging)
   * [Send logs from the Client](#send-logs-from-the-client)
   * [Configure Logging](#configure-Logging) 
* [Authorization](#authorization)
* [Navigation Menu](#navigation-menu)
* [Page Layout](#page-layout)
   * [Page Rendering](#page-rendering) 
* [Documents](#documents)
   * [Document](#document)
   * [TabDocument](#tabdocument)
   * [Document Validation](#document-validation)
* [Components](#components)
   * [Standard Components](#standard-components)
   * [Dropdown Components](#dropdown-components)
        * [Dropdown](#dropdown)
        * [DropdownEnum](#dropdownenum) 
        * [DropdownComplex](#dropdowncomplex)        
   * [Generic Components](#generic-components)
   * [Specialized Components](#specialized-components)
   * [Communication Between Components](#communication-between-components)
        * [StateNotificationMediator](#statenotificationmediator)
        * [Linked Components](#linked-components) 
          * [Making a Component Link Enabled](#making-a-component-link-enabled)
          * [Linking two DynamicFields in the same DynamicModel](#linking-two-dynamicfields-in-the-same-dynamicmodel)
          * [Propagating Linked DynamicFields across different DynamicModels](#propagating-linked-dynamicfields-across-different-dynamicmodels)
* [Configuration](#configuration)
* [Administration](#administration)
* [Database](#database)
* [UML Diagrams](#uml-diagrams)
	* [Blazor Server OnStart](#blazor-server-onstart)	
	* [ClaimModules API](#claimmodules-api) 
* [Notes](#notes)
    * [Adding font awesome](#adding-font-awesome)
    * [EntityFramework Core Migrations](#entityframework-core-migrations)
    * [Handle System.Text.Json Circular Reference Errors](#handle-systemtextjson-circular-reference-errors)
    * [Configure ASP.Net Core use Json.Net](#configure-aspnet-core-use-jsonnet)
* [Acknowledgements](#acknowledgements)

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

## Building an Example Headway Application
An example application will be created using **Headway** to demonstrate features available the **Headway** framework including, configuring dynamically rendered page layout, creating a navigation menu, configuring a workflow, binding page layout to the workflow, securing the application using **OAuth 2.0** authentication and restricting users access and functionality with by assigning roles and permissions.

### Introduction to RemediatR
The example application is called **RemediatR**. **RemediatR** will provide a platform to refund (remediate or redress) customers that have been wronged in some way e.g. a customer who bought a product that does not live up to it's commitments. The remediation flow will start with creating the redress case with the relevant data including customer, redress program and product data. The case progresses to refund calculation and verification, followed by sending a communication to the customer and finally end with a payment to the customer of the refunded amount.

Different users will be responsible for different stages in the flow. They will be assigned a role to reflect their responsibility. The roles will be as follows:
-	**Redress Case Owner** – creates, monitors and progresses the redress case from start through to completion 
-	**Redress Reviewer** – reviews the redress case at critical points e.g. prior to customer communication or redress completion
-	**Refund Assessor** – calculates the refund amount, including any compensatory interest due
-	**Refund Reviewer** – reviews the refund calculated as part of a four-eyes check to ensure the refunded amount is accurate

The RemediatR Flow is as follows:

![Alt text](/readme-images/StandardRemediationFlow.png?raw=true) 

### Building RemediatR in Easy Steps
**RemediatR** can be built using the **Headway** platform in several easy steps involving creating a few models and repository layer, and configuring the rest.

### Create
#### 1. Create the RemediatR Projects
- [Headway.RemediatR.Core](https://github.com/grantcolley/headway/tree/main/src/Headway.RemediatR.Core)
- [Headway.RemediatR.Repository](https://github.com/grantcolley/headway/tree/main/src/Headway.RemediatR.Repository)

#### 2. Create the Models and Interfaces
- In [Headway.RemediatR.Core](https://github.com/grantcolley/headway/tree/main/src/Headway.RemediatR.Core) 
  - Add a reference to project **Headway.Core**
  - Create the [model](https://github.com/grantcolley/headway/blob/main/src/Headway.RemediatR.Core/Model) classes.
  - Create the [IRemediatRRepository](https://github.com/grantcolley/headway/tree/main/src/Headway.RemediatR.Core/Interface/IRemediatRRepository.cs) interface.

#### 3. Create the Repository
> This example uses EntityFramework Code First.
- In [Headway.RemediatR.Repository](https://github.com/grantcolley/headway/tree/main/src/Headway.RemediatR.Repository)
  - Add a reference to project **Headway.Repository**
  - Add a reference to project **Headway.RemediatR.Core**
  - Create [RemediatRRepository](https://github.com/grantcolley/headway/tree/main/src/Headway.RemediatR.Repository/RemediatRRepository.cs) class.
- In **Headway.Repository**
  - Add a reference to project **Headway.RemediatR.Core**
  - Update [ApplicationDbContext](https://github.com/grantcolley/headway/blob/main/src/Headway.Repository/Data/ApplicationDbContext.cs) with the models
- Create the schema and update the database
  - In Visual Studio Developer PowerShell
  - `> cd Headway.WebApi`
  - `> dotnet ef migrations add RemediatR --project ..\Utilities\Headway.MigrationsSqlServer`
  - `> dotnet ef database update --project ..\Utilities\Headway.MigrationsSqlServer`    

#### 4. Create WebApi Access
- In [Headway.RemediatR.Core](https://github.com/grantcolley/headway/tree/main/src/Headway.RemediatR.Core)
  - Create the [RemediatRRoles](https://github.com/grantcolley/headway/blob/main/src/Headway.RemediatR.Core/Constants/RemediatRRoles.cs) constants. 
- In **Headway.WebApi**
  - Create the [RemediatR controller](https://github.com/grantcolley/headway/tree/main/src/Headway.WebApi/Controllers) classes.

#### 5. Create the WebApi Controllers
- In [Headway.WebApi](https://github.com/grantcolley/headway/tree/main/src/Headway.WebApi)
  - Add a reference to project **Headway.RemediatR.Core**
  - Add a reference to project **Headway.RemediatR.Repository**
  - Create the [RemediatRCustomerController](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/Controllers/RemediatRCustomerController.cs) controller.
  - Create the [RemediatRProgramController](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/Controllers/RemediatRProgramController.cs) controller.
  - Create the [RemediatRRedressController](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/Controllers/RemediatRRedressController.cs) controller.
  - Add a scoped service for [IRemediatRRepository](https://github.com/grantcolley/headway/blob/main/src/Headway.RemediatR.Core/Interface/IRemediatRRepository.cs) to [Program.cs](https://github.com/grantcolley/headway/blob/0ff19d343f2cf182a860082af7f6be1895785fd0/src/Headway.WebApi/Program.cs#L56) 
      \
      `builder.Services.AddScoped<IRemediatRRepository, RemediatRRepository>();`

#### 6. Create model Options
- In [Headway.Repository](https://github.com/grantcolley/headway/tree/main/src/Headway.Repository)
  - Add `GetCountryOptionItems` method to [OptionsRepository](https://github.com/grantcolley/headway/blob/e1e7ba29ecf8fe9e4ff182d4954b3a934fa53a54/src/Headway.Repository/OptionsRepository.cs#L105-L118)

#### 7. Create model validation
- In [Headway.WebApi](https://github.com/grantcolley/headway/tree/main/src/Headway.WebApi)
  - add package `<PackageReference Include="FluentValidation.AspNetCore" Version="11.1.2" />`
  - add to [Program.cs](https://github.com/grantcolley/headway/blob/e5b0f0b204911eae920fd264a1ff953b762b2abd/src/Headway.WebApi/Program.cs#L27-L30)
  ```C#
  builder.Services.AddControllers()
      .AddFluentValidation(
          fv => fv.RegisterValidatorsFromAssembly(Assembly.Load("Headway.RemediatR.Core")))
  ```
- In [Headway.RemediatR.Core](https://github.com/grantcolley/headway/tree/main/src/Headway.RemediatR.Core)
  - add package `<PackageReference Include="FluentValidation" Version="11.1.0" />`
  - add validators:
    - [CustomerValidation](https://github.com/grantcolley/headway/blob/main/src/Headway.RemediatR.Core/Validation/CustomerValidation.cs)
    - [ProductValidator](https://github.com/grantcolley/headway/blob/main/src/Headway.RemediatR.Core/Validation/ProductValidator.cs)
    - [ProgramValidator](https://github.com/grantcolley/headway/blob/main/src/Headway.RemediatR.Core/Validation/ProgramValidator.cs)
    
### Reference 
- In [Headway.BlazorServerApp](https://github.com/grantcolley/headway/tree/main/src/Headway.BlazorServerApp)
  - Add a project reference to **Headway.RemediatR.Core**
  - add to [Program.cs](https://github.com/grantcolley/headway/blob/bc1836e107ec9a752839987e91fdcb074d52a8fd/src/Headway.BlazorServerApp/Program.cs#L144) to ensure the *RemediatR.Core* assembly is eager loaded and it's classes available to be scanned for *Headway* attributes.
  ```C#
    app.UseAdditionalAssemblies(new[] { typeof(Redress).Assembly });
  ```
  
- In [Headway.BlazorWebassemblyApp](https://github.com/grantcolley/headway/tree/main/src/Headway.BlazorWebassemblyApp)
  - Add a project reference to **Headway.RemediatR.Core**  
  - add to [Program.cs](https://github.com/grantcolley/headway/blob/bc1836e107ec9a752839987e91fdcb074d52a8fd/src/Headway.BlazorWebassemblyApp/Program.cs#L88) to ensure the *RemediatR.Core* assembly is eager loaded and it's classes available to be scanned for *Headway* attributes. 
  ```C#
    builder.Services.UseAdditionalAssemblies(new[] { typeof(Redress).Assembly });
  ```
 
### Configure
#### 1. Configure Authorisation
Seed data for RemediatR permissions, roles and users can be found in [RemediatRData.cs](https://github.com/grantcolley/headway/blob/f0d688849192d3ed07211e86fd6a0d37294ef90c/src/Utilities/Headway.SeedData/RemediatRData.cs#L40-L134).

Alternatively, permissions, roles and users can be configured under the Authorisation category in the Administration module.

![Alt text](/readme-images/RemediatR_Authorisation.jpg?raw=true "Configure RemediatR Authorisation") 


#### 2. Configure Navigation
Seed data for RemediatR navigation can be found in [RemediatRData.cs](https://github.com/grantcolley/headway/blob/f10f8234c2b866d057b98b2af0988f449dd09aee/src/Utilities/Headway.SeedData/RemediatRData.cs#L136-L167)

Alternatively, modules, categories and menu items can be configured under the Navigation category in the Administration module.

![Alt text](/readme-images/RemediatR_Navigation.jpg?raw=true "Configure RemediatR Navigation") 

#### 3. Configure Model Layout
- Configure [Programs](https://github.com/grantcolley/headway/blob/109baa38ef67527e5eab7616a1ec1d381be08a74/src/Utilities/Headway.SeedData/RemediatR/RemediatRData.cs#L518-L538) search list
- Configure [Program](https://github.com/grantcolley/headway/blob/109baa38ef67527e5eab7616a1ec1d381be08a74/src/Utilities/Headway.SeedData/RemediatR/RemediatRData.cs#L357-L390) model
- Configure [Customers](https://github.com/grantcolley/headway/blob/109baa38ef67527e5eab7616a1ec1d381be08a74/src/Utilities/Headway.SeedData/RemediatR/RemediatRData.cs#L392-L440) search list
- Configure [Customer](https://github.com/grantcolley/headway/blob/109baa38ef67527e5eab7616a1ec1d381be08a74/src/Utilities/Headway.SeedData/RemediatR/RemediatRData.cs#L442-L484) model
- Configure [Redress Cases](https://github.com/grantcolley/headway/blob/109baa38ef67527e5eab7616a1ec1d381be08a74/src/Utilities/Headway.SeedData/RemediatR/RemediatRData.cs#L540-L598) search list
- Configure [New Redress Cases](https://github.com/grantcolley/headway/blob/109baa38ef67527e5eab7616a1ec1d381be08a74/src/Utilities/Headway.SeedData/RemediatR/RemediatRData.cs#L600-L662) search list

#### 4. Configure a Flow

#### 5. Bind the Flow to a Model

#### 6. Bind Permissions to the Flow

## Authentication

### Token-based Authentication
Blazor applications use [token-based authentication](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-6.0#token-based-authentication) based on digitally signed [JSON Web Tokens (JWTs)](https://jwt.io/introduction), which is a safe means of representing claims that can be transferred between parties.
Token-based authentication involves an authentication server issuing an athenticated user with a token containing claims, which can be sent to a resource such as a WebApi, with an extra `authorization` header in the form of a `Bearer` token. This allows the WebApi to validate the claim and provide the user access to the resource.

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

> NOTE: if implementing `Auth0` you will need to create a `Auth Pipeline Rule` to return the email and role as a claim.
```C#
function (user, context, callback) {
 	const accessTokenClaims = context.accessToken || {};
	const idTokenClaims = context.idToken || {};
  const assignedRoles = (context.authorization || {}).roles;
  accessTokenClaims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] = user.email;
  accessTokenClaims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] = assignedRoles;
  idTokenClaims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] = assignedRoles;
  return callback(null, user, context);
}
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

## Tracking Changes
When using **Entity Framework Core**, models inheriting from [ModelBase](https://github.com/grantcolley/headway/blob/main/src/Headway.Core/Model/ModelBase.cs) will automatically get properties for tracking instance creation and modification. Furthermore, an audit of changes will be logged to the `Audits` table.
```C#
    public abstract class ModelBase
    {
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
```
To log changes [ApplicationDbContext](https://github.com/grantcolley/headway/blob/bc50a2417f6f72edeae2bbe8c9a83dc3154b4bc9/src/Headway.Repository/Data/ApplicationDbContext.cs#L100) overrides `DbContext.SaveChanges` and gets the changes from `DbContext.ChangeTracker`.
Capturing the `user` is done by calling `ApplicationDbContext.SetUser(user)`. This is currently set in [RepositoryBase](https://github.com/grantcolley/headway/blob/bc50a2417f6f72edeae2bbe8c9a83dc3154b4bc9/src/Headway.Repository/RepositoryBase.cs#L23-L26) where it is called from [ApiControllerBase](https://github.com/grantcolley/headway/blob/bc50a2417f6f72edeae2bbe8c9a83dc3154b4bc9/src/Headway.WebApi/Controllers/ApiControllerBase.cs#L26-L34) which gets the user claim from to authorizing the user.

![Alt text](/readme-images/Audits.jpg?raw=true "Audit trail of add, update and delete") 

## Logging
[Headway.WebApi](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/Headway.WebApi.csproj) uses Serilog for logging and is configured to write logs to the `Log` table in the database using [Serilog.Sinks.MSSqlServer](https://github.com/serilog-mssql/serilog-sinks-mssqlserver).

### Send logs from the Client
The client can send a log entry request to the [Headway.WebApi](https://github.com/grantcolley/headway/blob/main/src/Headway.WebApi/Controllers/LogController.cs) e.g.: 
```C#
            try
            {
                var x = 1 / zero;
            }
            catch (Exception ex)
            {
                var log = new Log { Level = Core.Enums.LogLevel.Error, Message = ex.Message };

                await Mediator.Send(new LogRequest(log))
                    .ConfigureAwait(false);
            }
```

Logging is also available to api request classes inheriting [LogApiRequest](https://github.com/grantcolley/headway/blob/main/src/Headway.RequestApi/Api/LogApiRequest.cs) and can be called as follows:
```C#
            var log = new Log { Level = Core.Enums.LogLevel.Information, Message = "Log this entry..." };

            await LogAsync(log).ConfigureAwait(false);
```

### Configure Logging
In the Serilog [config](https://github.com/grantcolley/headway/blob/24d7a974fe53b0f0b7f2ccaaf7bf854486e25310/src/Headway.WebApi/appsettings.json#L12-L41) specify a custom column to be added to the `Log` table to capture the user with each entry. To automatically log EF Core SQL queries to the logs, add the override `"Microsoft.EntityFrameworkCore.Database.Command": "Information"`.

```C#
  "Serilog": {
    "Using": [ "Serilog.Sinks.MSSqlServer" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Error",
        "Microsoft.EntityFrameworkCore.Database.Command": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "MSSqlServer",
        "Args": {
          "connectionString": "Data Source=(localdb)\\mssqllocaldb;Database=Headway;Integrated Security=true",
          "tableName": "Logs",
          "autoCreateSqlTable": true,
          "columnOptionsSection": {
            "customColumns": [
              {
                "ColumnName": "User",
                "DataType": "nvarchar",
                "DataLength": 100
              }
            ]
          }
        }
      }
    ]
  },
```

More details on enriching Serilog log entries with custom properties can be found [here](https://github.com/serilog/serilog/wiki/Enrichment). For Serilog enrichment to work `loggerConfiguration.Enrich.FromLogContext()` is called when configuring logging in [Program.cs](https://github.com/grantcolley/headway/blob/436d272fd3c51a0d862b5e6d80cf7d1a5fb8c821/src/Headway.WebApi/Program.cs#L28-L30).

```C#
builder.WebHost.UseSerilog((hostingContext, loggerConfiguration) =>
                  loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                                        .Enrich.FromLogContext());
```

Middleware is also added in [Program.cs](https://github.com/grantcolley/headway/blob/383238b67e29bf5cdcd7426a6d628a6090f2af9a/src/Headway.WebApi/Program.cs#L128-L135) to get the user from the *httpContext* and push it onto the logging context for each request. The middleware must be added **AFTER** `app.UseAuthentication();` so the user claims is available in the *httpContext*.

```C#
app.UseAuthentication();

app.Use(async (httpContext, next) =>
{
    var identity = (ClaimsIdentity)httpContext.User.Identity;
    var claim = identity.FindFirst(ClaimTypes.Email);
    var user = claim.Value;
    LogContext.PushProperty("User", user);
    await next.Invoke();
});
```
## Authorization
The following UML diagram shows the ClaimModules API obtaining an authenticated users permissions which restrict the modules, categories and menu items available to the user in the `Navigation Menu`:
![Alt text](/readme-images/AuthorizationFlow.jpg?raw=true "ClaimModules API") 

## Navigation Menu

## Page Layout
![Alt text](/readme-images/Layout.drawio.png?raw=true "Page Layout")

### Page Rendering
![Alt text](/readme-images/PageRenderHierarchy.drawio.png?raw=true "Page Render Hierarchy")

## Documents
### Document

### TabDocument

### Document Validation
Headway documents use [Blazored.FluentValidation](https://github.com/Blazored/FluentValidation) where the `<FluentValidationValidator />` is placed inside the `<EditForm>` e.g.
```C#
    <EditForm EditContext="CurrentEditContext">
        <FluentValidationValidator />
```

> NOTE:
> [Blazored.FluentValidation](https://github.com/Blazored/FluentValidation) is used for client side validation only while [DataAnnotation](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/models-data/validation-with-the-data-annotation-validators-cs) and [Fluent API](https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties) is used for server side validation with Entity Framework.
> \
> \
> For additional reading see [Data Annotations Attributes](https://www.entityframeworktutorial.net/code-first/dataannotation-in-code-first.aspx) and [Fluent API Configurations](https://www.entityframeworktutorial.net/code-first/fluent-api-in-code-first.aspx) in EF 6 and and EF Core.
    
## Components
### Standard Components

### Dropdown Components
#### Dropdown
The source for a standard dropdown is `IEnumerable<OptionItem>` and the selected item is bound to `@bind-Value="SelectedItem"`.

#### DropdownEnum

#### DropdownComplex

### Generic Components

### Specialized Components

### Communication Between Components

#### StateNotificationMediator

#### Linked Components 
Fields can be linked to each other so at runtime the value of one can be dependent on the value of another. For example, in a scenario where one field is *Country* and the other is *City*, and both are rendered as dropdown lists. The dropdown list for *Country* is initially populated while the dropdown list for "City" remains empty. Only once a country has been selected will the dropdown list for *City* be populated, with a list of cities belonging to the selected country.

###### Making a Component Link Enabled
 * A link enabled component, such as [Dropdown.razor](https://github.com/grantcolley/headway/blob/main/src/Headway.Razor.Controls/Components/Dropdown.razor.cs), must inherit from [DynamicComponentBase](https://github.com/grantcolley/headway/blob/main/src/Headway.Razor.Controls/Base/DynamicComponentBase.cs).
 * The component must inherit [IStateNotification](https://github.com/grantcolley/headway/blob/89d2c0070484552f226af13e10052590a89f917b/src/Headway.Razor.Controls/Components/Dropdown.razor.cs#L17-L18).
 * It must also call [DynamicComponentBase.LinkFieldCheck()](https://github.com/grantcolley/headway/blob/89d2c0070484552f226af13e10052590a89f917b/src/Headway.Razor.Controls/Components/Dropdown.razor.cs#L43-L45) to obtain the value of the [LinkedSource](https://github.com/grantcolley/headway/blob/89d2c0070484552f226af13e10052590a89f917b/src/Headway.Core/Dynamic/DynamicField.cs#L28) field.
 * Finally, if the backing field has any [LinkedDependents](https://github.com/grantcolley/headway/blob/89d2c0070484552f226af13e10052590a89f917b/src/Headway.Core/Dynamic/DynamicField.cs#L29) the component must [notify state has changed](https://github.com/grantcolley/headway/blob/89d2c0070484552f226af13e10052590a89f917b/src/Headway.Razor.Controls/Components/Dropdown.razor.cs#L65-L71) when its value changes.

###### Linking two DynamicFields in the same DynamicModel
 * In the target field's [ConfigItem.ComponentArgs](https://github.com/grantcolley/headway/blob/5e352324d85ec6f2690c44b2a9eabf53b87fec22/src/Headway.Core/Model/ConfigItem.cs#L15) property add a `LinkedSource` key/value pair: 
   \
   e.g. `Name=LinkedSource;VALUE=[LINKED FIELD NAME]`
 * At runtime, when the [DynamicModel](https://github.com/grantcolley/headway/blob/main/src/Headway.Core/Dynamic/DynamicModel.cs) is created, linked fields will be mapped together in [ComponentArgHelper.AddDynamicArgs()](https://github.com/grantcolley/headway/blob/5e352324d85ec6f2690c44b2a9eabf53b87fec22/src/Headway.Core/Helpers/ComponentArgHelper.cs#L76-L91), so the target references the source field via it's `LinkedSource` property.

###### Propagating Linked DynamicFields across different DynamicModels
It is possible to link two DynamicFields in different DynamicModels. This is done using `PropagateFields` key/value pair: 
\
   e.g. `Name=PropagateFields;VALUE=[COMMA SEPARATED LINKED FIELD NAMES]`
\
Consider the example we have [Config.cs](https://github.com/grantcolley/headway/blob/main/src/Headway.Core/Model/Config.cs) and [ConfigItem.cs](https://github.com/grantcolley/headway/blob/main/src/Headway.Core/Model/ConfigItem.cs) where `ConfigItem.PropertyName` is dependent on the value of `Config.Model`.
\
\
`Config.Model` is rendered as a dropdown containing a list of classes with the `[DynamicModel]` attribute. `ConfigItem.PropertyName` is rendered as a dropdown containing a list of properties belonging to the class selected in `Config.Model`.

```C#
    [DynamicModel]
    public class DemoModel
    {
        // code omitted for brevity

        public string Model { get; set; }
        
        public List<DemoModelItem> DemoModelItems { get; set; }
        
        // code omitted for brevity
    }
    
    [DynamicModel]
    public class DemoModelItem
    {
        // code omitted for brevity
        
        public string PropertyName { get; set; }

        // code omitted for brevity
    }
```
To map the linked source `DemoModel.Model` to target `DemoModelItem.PropertyName`:
\
 * In the `DemoModel`'s `ConfigItem` for `DemoModelItems`, it's [ConfigItem.ComponentArgs](https://github.com/grantcolley/headway/blob/5e352324d85ec6f2690c44b2a9eabf53b87fec22/src/Headway.Core/Model/ConfigItem.cs#L15) property will contain a `PropagateFields` key/value pair: 
   \
   e.g. `Name=PropagateFields;VALUE=Model`
 * In the `DemoModelItem`'s `ConfigItem` for `PropertyName`, it's [ConfigItem.ComponentArgs](https://github.com/grantcolley/headway/blob/5e352324d85ec6f2690c44b2a9eabf53b87fec22/src/Headway.Core/Model/ConfigItem.cs#L15) property will contain a `LinkedSource` key/value pair: 
   \
   e.g. `Name=LinkedSource;VALUE=Model`
 * At runtime, when the [DynamicModel](https://github.com/grantcolley/headway/blob/main/src/Headway.Core/Dynamic/DynamicModel.cs) is created, the linked source `DemoModel.Model` will be propagated in [ComponentArgHelper.AddDynamicArgs()](https://github.com/grantcolley/headway/blob/5e352324d85ec6f2690c44b2a9eabf53b87fec22/src/Headway.Core/Helpers/ComponentArgHelper.cs#L76-L91), where the propagated args will be passed into the `DemoModel.DemoModelItems`'s component as a [DynamicArg](https://github.com/grantcolley/headway/blob/main/src/Headway.Core/Model/DynamicArg.cs) whose value is the source field `DemoModel.Model`. The component for `DemoModel.DemoModelItems` inherit from [DynamicComponentBase](https://github.com/grantcolley/headway/blob/41b67011e0b82fea0b694b5492f9885ccccb8a7b/src/Headway.Razor.Controls/Base/DynamicComponentBase.cs#L45-L61), which will map the linked fields together so the target references the source field via it's `LinkedSource` property.

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

## UML Diagrams
The following incredibly useful UML diagrams have been provided by [@VR-Architect](https://github.com/VR-Architect).

#### Blazor Server OnStart
![Alt text](/readme-images/BlazorServerOnStart.jpg?raw=true "Blazor Server OnStart") 

#### ClaimModules API 
![Alt text](/readme-images/AuthorizationFlow.jpg?raw=true "ClaimModules API") 
	
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
` dotnet ef migrations add UpdateHeadway --project ..\..\Utilities\Headway.MigrationsSqlServer`

Update the database with the latest migrations. It will also create the database if it hasn't already been created:
\
\
`dotnet ef database update --project ..\..\Utilities\Headway.MigrationsSqlServer`

Remove the latest migration:
\
\
` dotnet ef migrations remove --project ..\..\Utilities\Headway.MigrationsSqlServer`

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

### Configure ASP.Net Core use Json.Net
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

## Acknowledgements
- [@VR-Architect](https://github.com/VR-Architect) - for providing incredibly useful [UML Diagrams](#uml-diagrams)
