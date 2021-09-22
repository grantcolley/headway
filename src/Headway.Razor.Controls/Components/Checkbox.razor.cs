using Headway.Core.Attributes;
using Headway.Razor.Controls.Base;
using System;
using System.Linq.Expressions;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public class CheckboxBase : DynamicComponentBase
    {
        public Expression<Func<bool>> FieldExpression
        {
            get
            {
                return Expression.Lambda<Func<bool>>(Field.MemberExpression);
            }
        }

        public bool PropertyValue
        {
            get
            {
                return (bool)Field.PropertyInfo.GetValue(Field.Model);
            }
        }

        public virtual void OnValueChanged(bool value)
        {
            Field.PropertyInfo.SetValue(Field.Model, value);
        }
    }
}
