using Headway.Core.Attributes;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class DecimalBase : DynamicComponentBase
    {
        public Expression<Func<decimal>> FieldExpression
        {
            get
            {
                return Expression.Lambda<Func<decimal>>(Field.MemberExpression);
            }
        }

        public decimal PropertyValue
        {
            get
            {
                return (decimal)Field.PropertyInfo.GetValue(Field.Model);
            }
            set
            {
                Field.PropertyInfo.SetValue(Field.Model, value);
            }
        }
    }
}
