using Headway.Core.Attributes;
using Headway.Core.Interface;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public class DropdownComplexBase<T> : DynamicComponentBase
    {
        [Inject]
        public IOptionsService OptionsService { get; set; }

        protected IEnumerable<T> OptionItems;

        public Expression<Func<T>> FieldExpression
        {
            get
            {
                return Expression.Lambda<Func<T>>(Field.MemberExpression);
            }
        }

        public T PropertyValue
        {
            get
            {
                return (T)Field.PropertyInfo.GetValue(Field.Model);
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            var result = await OptionsService.GetOptionItemsAsync<T>(ComponentArgs).ConfigureAwait(false);

            OptionItems = GetResponse(result);

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        public virtual void OnValueChanged(T value)
        {
            Field.PropertyInfo.SetValue(Field.Model, value);
        }
    }
}
