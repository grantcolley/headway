using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public class DropdownComplexBase<T> : DynamicComponentBase
    {
        [Inject]
        public IOptionsService OptionsService { get; set; }

        public string Value { get; set; }

        protected IEnumerable<GenericItem<T>> OptionItems;

        protected override async Task OnParametersSetAsync()
        {
            var displayName = ComponentArgs.Single(a => a.Name.Equals(Options.DISPLAY_FIELD)).Value.ToString();
            var propertyInfo = PropertyInfoHelper.GetPropertyInfo(typeof(T), displayName);
            var value = propertyInfo.GetValue(Field.PropertyInfo.GetValue(Field.Model));

            if (value != null)
            {
                Value = value.ToString();
            }

            var result = await OptionsService.GetOptionItemsAsync<T>(ComponentArgs).ConfigureAwait(false);

            var optionItems = GetResponse(result);

            if (optionItems.Any())
            {
                OptionItems = optionItems.Select( oi => new GenericItem<T>(oi, propertyInfo));
            }

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        public virtual void OnValueChanged(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                Field.PropertyInfo.SetValue(Field.Model, null);
            }
            else
            {
                var optionItem = OptionItems.First(o => o.Name.Equals(value));
                Field.PropertyInfo.SetValue(Field.Model, optionItem);
            }
        }
    }
}
