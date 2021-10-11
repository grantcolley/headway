using Headway.Core.Constants;
using Headway.Core.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Headway.Core.Helpers
{
    public static class TypeAttributeHelper
    {
        public static IEnumerable<DynamicType> GetEntryAssemblyTypeNamesByAttribute(Type attributeType)
        {
            var assembly = Assembly.GetEntryAssembly();
            var types = (from t in assembly.GetTypes()
                         let attributes = t.GetCustomAttributes(attributeType, true)
                         where attributes != null && attributes.Length > 0
                         select new DynamicType
                         {
                             Name = GetName(t),
                             DisplayName = GetName(t),
                             Namespace = GetFullNamespace(t, assembly)
                         }).ToList();
            return types;
        }

        public static IEnumerable<DynamicType> GetHeadwayTypesByAttribute(Type attributeType)
        {
            var dynamicTypes = new List<DynamicType>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name.StartsWith(TypeHelpers.HEADWAY))
                .ToList();

            foreach (var assembly in assemblies)
            {
                var types = (from t in assembly.GetTypes()
                                    let attributes = t.GetCustomAttributes(attributeType, true)
                                    where attributes != null && attributes.Length > 0
                                    select new DynamicType
                                    {
                                        Name = GetName(t),
                                        DisplayName = GetName(t),
                                        Namespace = GetFullNamespace(t, assembly)
                                    }).ToList();

                dynamicTypes.AddRange(types);
            }

            return dynamicTypes;
        }

        private static string GetName(Type type)
        {
            return (type.IsAbstract && type.Name.Contains(TypeHelpers.BASE))
                ? type.Name.Replace(TypeHelpers.BASE, string.Empty)
                : type.Name;
        }

        private static string GetFullNamespace(Type type, Assembly assembly)
        {
            return (type.IsAbstract && type.Name.Contains(TypeHelpers.BASE))
                ? $"{type.Namespace}.{type.Name.Replace(TypeHelpers.BASE, string.Empty)}, {assembly.GetName().Name}"
                : $"{type.FullName}, {assembly.GetName().Name}";
        }
    }
}
