using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Headway.Razor.Controls.Components
{
    public partial class TextBase : ComponentBase
    {
        [Parameter]
        public DynamicField Field { get; set; }

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

        public void OnValueChanged(string value)
        {
            Field.PropertyInfo.SetValue(Field.Model, value);
        }
    }
}
