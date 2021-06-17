using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Headway.Core.Dynamic
{
    public class DynamicModel<T>
    {
        public DynamicModel(T model)
        {
            Model = model;

            BuildDynamicFields();
        }

        public T Model { get; private set; }
        public List<DynamicField> DynamicFields { get; private set; }

        private void BuildDynamicFields()
        {
            DynamicFields = new List<DynamicField>();

            foreach(var property in GetPropertyInfos())
            {
                var dynamicField = new DynamicField
                {
                    Model = Model,
                    PropertyInfo = property,
                    PropertyName = property.Name,
                };

                if (property.PropertyType.Equals(typeof(string)))
                {
                    dynamicField.DynamicComponentTypeName = "Headway.RazorShared.Components.LabelText, Headway.RazorShared";
                }
                else if(property.PropertyType.Equals(typeof(int)))
                {
                    dynamicField.DynamicComponentTypeName = "Headway.RazorShared.Components.LabelData, Headway.RazorShared";
                }

                dynamicField.DynamicComponent = Type.GetType(dynamicField.DynamicComponentTypeName);

                dynamicField.Parameters = new Dictionary<string, object> { { "Field", dynamicField } };

                DynamicFields.Add(dynamicField);
            }
        }

        private static IEnumerable<PropertyInfo> GetPropertyInfos()
        {
            var propertyInfoResults = new List<PropertyInfo>();

            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

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
