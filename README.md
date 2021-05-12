# headway

[![Build status](https://ci.appveyor.com/api/projects/status/xg606j7pr1ib54db?svg=true)](https://ci.appveyor.com/project/grantcolley/headway)

##### Technologies
###### .NET 5.0, Blazor WebAssembly, Blazor Server, IdentityServer4, ASP.NET Core Web API
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
* [Role Based Navigation Menu](#role-based-navigation-menu)
* [Logging Errors](#logging-errors)
* [Configuration](#configuration)
 * [Notes](#notes)
    * [Adding font awesome](#adding-font-awesome)


## Permissioning Navigation
Content 1 text here...

## Logging Errors

## Configuration
Content 2 text here...

### Adding Font Awesome
 * Right-click the `wwwroot\css` folder in the Blazor project and click `Add` then `Client-Side Library...`. Search for `font-awesome` and install it.
 * For a Blazor Server app add `@import url('font-awesome/css/all.min.css');` at the top of [site.css](https://github.com/grantcolley/headway/tree/main/src/Headway.BlazorServerApp/wwwroot/css). For Blazor WebAssembly app add it to [app.css](https://github.com/grantcolley/headway/blob/main/src/Headway.BlazorWebassemblyApp/wwwroot/css/app.css).
  
  
