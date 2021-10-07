using Headway.Core.Attributes;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class TextBase : DynamicComponentBase
    {
        public Expression<Func<string>> FieldExpression
        {
            get
            {
                return Expression.Lambda<Func<string>>(Field.MemberExpression);
            }
        }

        public string PropertyValue
        {
            get
            {
                return Field.PropertyInfo.GetValue(Field.Model)?.ToString();
            }
        }

        public virtual void OnValueChanged(string value)
        {
            Field.PropertyInfo.SetValue(Field.Model, value);
        }
    }
}
