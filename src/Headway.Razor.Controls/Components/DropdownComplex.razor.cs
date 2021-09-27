using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public class DropdownComplexBase<T> : DynamicComponentBase
    {
        [Inject]
        public IOptionsService OptionsService { get; set; }

        protected IEnumerable<GenericItem<T>> OptionItems;

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

            var optionItems = GetResponse(result);

            if (optionItems.Any())
            {
                var displayName = ComponentArgs.Single(a => a.Name.Equals(Options.DISPLAY_FIELD)).Value.ToString();
                var propertyInfo = PropertyInfoHelper.GetPropertyInfo(optionItems.First().GetType(), displayName);
                OptionItems = optionItems.Select( oi => new GenericItem<T>(oi, propertyInfo));
            }

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        public virtual void OnValueChanged(T value)
        {
            Field.PropertyInfo.SetValue(Field.Model, value);
        }
    }
}
