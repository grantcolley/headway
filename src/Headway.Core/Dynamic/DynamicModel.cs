using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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

            var constantExpression = Expression.Constant(Model);

            var typeHelper = DynamicTypeHelper.Get<T>();

            foreach (var property in typeHelper.SupportedProperties)
            {
                var dynamicField = new DynamicField
                {
                    Model = Model,
                    PropertyInfo = property,
                    PropertyName = property.Name,
                    MemberExpression = Expression.Property(constantExpression, property.Name)
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
    }
}
