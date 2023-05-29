using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Headway.App.Blazor.WebAssembly.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// A collection of additional assemblies that should be eager loaded at startup so they can be 
        /// searched for classes with Headway attributes such as [DynamicModel] etc.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <param name="assemblies">A collection of assemblies to be eager loaded at startup.</param>
        /// <returns>The services collection.</returns>
        public static IServiceCollection UseAdditionalAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            if (assemblies is null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            // Intentionally returns services without actually doing anything.
            return services;
        }
    }
}
