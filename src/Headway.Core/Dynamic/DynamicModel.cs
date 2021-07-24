using Headway.Core.Model;
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

        public DynamicModel(T model, Config config)
        {
            Model = model;
            Config = config;

            var idField = config.ConfigItems.FirstOrDefault(ci => ci.IsIdentity.HasValue && ci.IsIdentity.Value);

            if (idField != null)
            {
                idFieldName = idField.PropertyName;
            }

            var titleField = config.ConfigItems.FirstOrDefault(ci => ci.IsTitle.HasValue && ci.IsTitle.Value);

            if(titleField != null)
            {
                titleFieldName = titleField.PropertyName;
            }

            typeHelper = DynamicTypeHelper.Get<T>();

            BuildDynamicFields();
        }

        public T Model { get; private set; }
        public List<DynamicField> DynamicFields { get; private set; }
        public Config Config { get; private set; }

        public int Id { get { return Convert.ToInt32(typeHelper.GetValue(Model, idFieldName)); } }

        public string Title { get { return typeHelper.GetValue(Model, titleFieldName).ToString(); } }

        private void BuildDynamicFields()
        {
            DynamicFields = new List<DynamicField>();

            var constantExpression = Expression.Constant(Model);

            static DynamicField func(T model, ConstantExpression ce, PropertyInfo p, ConfigItem c)
            {
                var dynamicField = new DynamicField
                {
                    Model = model,
                    Order = c.Order,
                    PropertyInfo = p,
                    PropertyName = p.Name,
                    Label = c.Label,
                    MemberExpression = Expression.Property(ce, p.Name),
                    DynamicComponentTypeName = c.Component,
                    DynamicComponent = Type.GetType(c.Component)
                };

                dynamicField.Parameters = new Dictionary<string, object> { { "Field", dynamicField } };

                return dynamicField;
            }

            var dynamicFields = from p in typeHelper.SupportedProperties
                                join c in Config.ConfigItems on p.Name equals c.PropertyName
                                select func(Model, constantExpression, p, c);

            DynamicFields.AddRange(dynamicFields);
        }
    }
}
