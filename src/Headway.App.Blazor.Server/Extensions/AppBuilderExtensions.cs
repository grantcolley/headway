using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Headway.App.Blazor.Server.Extensions
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// A collection of additional assemblies that should be eager loaded at startup so they can be 
        /// searched for classes with Headway attributes such as [DynamicModel] etc.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="assemblies">A collection of assemblies to be eager loaded at startup.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder UseAdditionalAssemblies(this IApplicationBuilder app, IEnumerable<Assembly> assemblies)
        {
            if (assemblies is null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            // Intentionally returns app without actually doing anything.
            return app;
        }
    }
}
