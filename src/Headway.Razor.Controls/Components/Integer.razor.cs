using Headway.Core.Attributes;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class IntegerBase : DynamicComponentBase
    {
        public Expression<Func<int>> FieldExpression
        {
            get
            {
                return Expression.Lambda<Func<int>>(Field.MemberExpression);
            }
        }

        public int PropertyValue
        {
            get
            {
                return (int)Field.PropertyInfo.GetValue(Field.Model);
            }
            set
            {
                Field.PropertyInfo.SetValue(Field.Model, value);
            }
        }
    }
}
