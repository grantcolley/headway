using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Headway.Core.Dynamic
{
    public class DynamicModel<T>
    {
        private readonly string idFieldName;
        private readonly string titleFieldName;
        private readonly DynamicTypeHelper<T> typeHelper;

        public DynamicModel(T model, DynamicModelConfig dynamicModelConfig)
        {
            Model = model;
            DynamicModelConfig = dynamicModelConfig;

            var idField = dynamicModelConfig.FieldConfigs.FirstOrDefault(f => f.IsIdField);

            if (idField != null)
            {
                idFieldName = idField.PropertyName;
            }

            var titleField = dynamicModelConfig.FieldConfigs.FirstOrDefault(f => f.IsTitleField);

            if(titleField != null)
            {
                titleFieldName = titleField.PropertyName;
            }

            typeHelper = DynamicTypeHelper.Get<T>();

            BuildDynamicFields();
        }

        public T Model { get; private set; }
        public List<DynamicField> DynamicFields { get; private set; }
        public DynamicModelConfig DynamicModelConfig { get; private set; }

        public int Id { get { return Convert.ToInt32(typeHelper.GetValue(Model, idFieldName)); } }

        public string Title { get { return typeHelper.GetValue(Model, titleFieldName).ToString(); } }

        private void BuildDynamicFields()
        {
            DynamicFields = new List<DynamicField>();

            var constantExpression = Expression.Constant(Model);

            static DynamicField func(T model, ConstantExpression ce, PropertyInfo p, DynamicFieldConfig c)
            {
                var dynamicField = new DynamicField
                {
                    Model = model,
                    Order = c.Order,
                    PropertyInfo = p,
                    PropertyName = p.Name,
                    MemberExpression = Expression.Property(ce, p.Name),
                    DynamicComponentTypeName = c.DynamicComponentTypeName,
                    DynamicComponent = Type.GetType(c.DynamicComponentTypeName)
                };

                dynamicField.Parameters = new Dictionary<string, object> { { "Field", dynamicField } };

                return dynamicField;
            }

            var dynamicFields = from p in typeHelper.SupportedProperties
                                join c in DynamicModelConfig.FieldConfigs on p.Name equals c.PropertyName
                                select func(Model, constantExpression, p, c);

            DynamicFields.AddRange(dynamicFields);
        }
    }
}
