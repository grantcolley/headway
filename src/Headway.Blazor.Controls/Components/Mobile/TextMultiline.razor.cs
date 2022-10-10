using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Blazor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Headway.Blazor.Controls.Components.Mobile
{
    [DynamicComponent]
    public abstract class TextMultilineBase : DynamicComponentBase
    {
        protected int rows;

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

        protected override void OnInitialized()
        {
            var val = ComponentArgHelper.GetArgValue(ComponentArgs, Args.TEXT_MULTILINE_ROWS);

            if (int.TryParse(val, out int i))
            {
                rows = i;
            }

            base.OnInitialized();
        }
    }
}
