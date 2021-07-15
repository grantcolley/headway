using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Headway.Razor.Components.DynamicComponents;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Headway.Razor.Configuration.Helpers
{
    public static class ConfigurationsTypeHelper
    {
        public static IEnumerable<DynamicType> GetConfigurationsDropdownItems()
        {
            var configurations = TypeAttributeHelper
                .GetCallingAssemblyDynamicRazorComponentsByAttribute(typeof(DynamicConfigurationAttribute))
                .ToList();

            var assembly = Assembly.GetAssembly(typeof(DynamicDefault));
            configurations.Insert(0, new DynamicType
            {
                Name = typeof(DynamicDefault).Name,
                Namespace = $"{typeof(DynamicDefault).FullName}, {assembly.GetName().Name}"
            });

            return configurations;
        }
    }
}
