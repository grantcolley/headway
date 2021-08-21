using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Headway.Core.Helpers
{
    public static class PropertyInfoHelper
    {
        public static IEnumerable<PropertyInfo> GetPropertyInfos(Type type)
        {
            var propertyInfoResults = new List<PropertyInfo>();

            PropertyInfo[] propertyInfos = type.GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                if (UnsupportedProperty(propertyInfo))
                {
                    continue;
                }

                propertyInfoResults.Add(propertyInfo);
            }

            return propertyInfoResults;
        }

        private static bool UnsupportedProperty(PropertyInfo propertyInfo)
        {
            // Skip non-public properties and properties that are either 
            // classes (but not strings), interfaces, lists, generic 
            // lists or arrays.
            var propertyType = propertyInfo.PropertyType;

            if (propertyType != typeof(string)
                && (propertyType.IsClass
                    || propertyType.IsInterface
                    || propertyType.IsArray
                    || propertyType.GetInterfaces()
                        .Any(
                            i =>
                                (i.GetTypeInfo().Name.Equals(typeof(IEnumerable).Name)
                                 || (i.IsGenericType &&
                                     i.GetGenericTypeDefinition().Name.Equals(typeof(IEnumerable<>).Name))))))
            {
                return true;
            }

            return false;
        }
    }
}
