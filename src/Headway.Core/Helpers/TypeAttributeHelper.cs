using Headway.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Headway.Core.Helpers
{
    public static class TypeAttributeHelper
    {
        public static IEnumerable<Model.Model> GetDynamicModels()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var browserStorageItems = (from t in assembly.GetTypes()
                                      let attributes = t.GetCustomAttributes(typeof(DynamicModelAttribute), true)
                                       where attributes != null && attributes.Length > 0
                                       select new Model.Model
                                       {
                                           Name = t.Name,
                                           Namespace = $"{t.FullName}, {assembly.GetName().Name}"
                                       }).ToList();
            return browserStorageItems;
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
    }
}
