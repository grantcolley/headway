using Headway.Core.Attributes;
using Headway.Blazor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Headway.Blazor.Controls.Components.Mobile
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
            set
            {
                Field.PropertyInfo.SetValue(Field.Model, value);
            }
        }
    }
}
