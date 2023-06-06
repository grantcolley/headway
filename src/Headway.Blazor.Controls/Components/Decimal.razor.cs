using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Blazor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Headway.Core.Extensions;

namespace Headway.Blazor.Controls.Components
{
    [DynamicComponent]
    public abstract class DecimalBase : DynamicComponentBase
    {
        protected string format = string.Empty;
        protected int? maxLength = null;
        protected decimal? min = null;
        protected decimal? max = null;

        protected override Task OnInitializedAsync()
        {
            var formatArg = ComponentArgs.FirstArgOrDefault(Args.FORMAT);

            if (formatArg != null)
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
                min = decimal.Parse(minArg.Value);
            }

            var maxArg = ComponentArgs.FirstArgOrDefault(Args.MAX);

            if (maxArg != null)
            {
                max = decimal.Parse(maxArg.Value);
            }

            return base.OnInitializedAsync();
        }

        public int MaxLength { get { return maxLength ?? int.MaxValue; } }
        public decimal Max { get { return max ?? decimal.MaxValue; } }
        public decimal Min { get { return min ?? decimal.MinValue; } }

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
