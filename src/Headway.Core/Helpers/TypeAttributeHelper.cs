using Headway.Core.Attributes;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Headway.Core.Helpers
{
    public static class TypeAttributeHelper
    {
        public static IEnumerable<DynamicType> GetCallingAssemblyDynamicRazorComponentsByAttribute(Type attributeType)
        {
            var assembly = Assembly.GetCallingAssembly();
            var dynamicTypes = (from t in assembly.GetTypes()
                                let attributes = t.GetCustomAttributes(attributeType, true)
                                where attributes != null && attributes.Length > 0
                                select new DynamicType
                                {
                                    Name = t.Name.Replace("Base", string.Empty),
                                    DisplayName = t.Name.Replace("Base", string.Empty),
                                    Namespace = $"{t.FullName.Replace("Base", string.Empty)}, {assembly.GetName().Name}"
                                }).ToList();
            dynamicTypes.Insert(0, new DynamicType 
            {
                Name = typeof(DynamicConfigurationDefaultAttribute).Name,
                Namespace = $"{typeof(DynamicConfigurationDefaultAttribute).FullName}, {assembly.GetName().Name}"
            });
            return dynamicTypes;
        }

        public static IEnumerable<DynamicType> GetCallingAssemblyDynamicTypesByAttribute(Type attributeType)
        {
            var assembly = Assembly.GetCallingAssembly();
            var dynamicTypes = (from t in assembly.GetTypes()
                                let attributes = t.GetCustomAttributes(attributeType, true)
                                where attributes != null && attributes.Length > 0
                                select new DynamicType
                                {
                                    Name = t.Name,
                                    Namespace = $"{t.FullName}, {assembly.GetName().Name}"
                                }).ToList();
            return dynamicTypes;
        }

        public static IEnumerable<string> GetEntryAssemblyTypesByAttribute(Type attributeType)
        {
            var assembly = Assembly.GetEntryAssembly();
            var types = (from t in assembly.GetTypes()
                         let attributes = t.GetCustomAttributes(attributeType, true)
                         where attributes != null && attributes.Length > 0
                         select t.Name).ToList();
            return types;
        }

        public static IEnumerable<DynamicType> GetExecutingAssemblyDynamicTypesByAttribute(Type attributeType)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var dynamicTypes = (from t in assembly.GetTypes()
                                let attributes = t.GetCustomAttributes(attributeType, true)
                                where attributes != null && attributes.Length > 0
                                select new DynamicType
                                {
                                    Name = t.Name,
                                    Namespace = $"{t.FullName}, {assembly.GetName().Name}"
                                }).ToList();
            return dynamicTypes;
        }
    }
}
