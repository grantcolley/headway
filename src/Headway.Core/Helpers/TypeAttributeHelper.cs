using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Headway.Core.Helpers
{
    public static class TypeAttributeHelper
    {
        public static IEnumerable<string> GetEntryAssemblyTypeNamesByAttribute(Type attributeType)
        {
            var assembly = Assembly.GetEntryAssembly();
            var types = (from t in assembly.GetTypes()
                         let attributes = t.GetCustomAttributes(attributeType, true)
                         where attributes != null && attributes.Length > 0
                         select t.Name).ToList();
            return types;
        }

        public static IEnumerable<DynamicType> GetHeadwayDynamicComponentsByAttribute(Type attributeType)
        {
            var dynamicTypes = new List<DynamicType>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name.StartsWith("Headway"))
                .ToList();

            foreach (var assembly in assemblies)
            {
                var types = (from t in assembly.GetTypes()
                             let attributes = t.GetCustomAttributes(attributeType, true)
                             where attributes != null && attributes.Length > 0
                             select new DynamicType
                             {
                                 Name = t.Name.Replace("Base", string.Empty),
                                 DisplayName = t.Name.Replace("Base", string.Empty),
                                 Namespace = $"{t.FullName.Replace("Base", string.Empty)}, {assembly.GetName().Name}"
                             }).ToList();

                dynamicTypes.AddRange(types);
            }

            return dynamicTypes;
        }

        public static IEnumerable<DynamicType> GetHeadwayTypesByAttribute(Type attributeType)
        {
            var dynamicTypes = new List<DynamicType>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name.StartsWith("Headway"))
                .ToList();

            foreach (var assembly in assemblies)
            {
                var types = (from t in assembly.GetTypes()
                                    let attributes = t.GetCustomAttributes(attributeType, true)
                                    where attributes != null && attributes.Length > 0
                                    select new DynamicType
                                    {
                                        Name = t.Name,
                                        DisplayName = t.Name,
                                        Namespace = $"{t.FullName}, {assembly.GetName().Name}"
                                    }).ToList();

                dynamicTypes.AddRange(types);
            }

            return dynamicTypes;
        }
    }
}
