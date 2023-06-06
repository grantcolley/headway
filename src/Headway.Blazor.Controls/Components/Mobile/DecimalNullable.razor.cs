using Headway.Blazor.Controls.Base;
using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Components.Mobile
{
    [DynamicComponent]
    public abstract class DecimalNullableBase : DynamicComponentBase
    {
        protected string format = string.Empty;
        protected int? maxLength = null;

        protected override Task OnInitializedAsync()
        {
            var formatArg = ComponentArgs.FirstArgOrDefault(Args.FORMAT);

            if(formatArg != null)
            {
                format = formatArg.Value;
            }

            var maxLengthArg = ComponentArgs.FirstArgOrDefault(Args.MAX_LENGTH);

            if (maxLengthArg != null)
            {
                maxLength = int.Parse(maxLengthArg.Value);
            }

            var minArg = ComponentArgs.FirstArgOrDefault(Args.MIN);

            if (minArg != null)
            {
                Min = decimal.Parse(minArg.Value);
            }

            var maxArg = ComponentArgs.FirstArgOrDefault(Args.MAX);

            if (maxArg != null)
            {
                Max = decimal.Parse(maxArg.Value);
            }

            return base.OnInitializedAsync();
        }

        public int MaxLength { get { return maxLength ?? int.MaxValue; } }
        protected decimal? Min { get; set; }
        protected decimal? Max { get; set; }

        public Expression<Func<decimal?>> FieldExpression
        {
            get
            {
                return Expression.Lambda<Func<decimal?>>(Field.MemberExpression);
            }
        }

        public decimal? PropertyValue
        {
            get
            {
                return (decimal?)Field.PropertyInfo.GetValue(Field.Model);
            }
            set
            {
                Field.PropertyInfo.SetValue(Field.Model, value);
            }
        }
    }
}
