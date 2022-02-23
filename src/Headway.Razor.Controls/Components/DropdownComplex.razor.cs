using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Notifications;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class DropdownComplexBase<T> : DynamicComponentBase
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [Inject]
        public IOptionsApiRequest OptionsService { get; set; }

        protected string value { get; set; }

        protected IEnumerable<GenericItem<T>> optionItems;

        protected override async Task OnParametersSetAsync()
        {
            value = null;

            var displayName = ComponentArgs.Single(a => a.Name.Equals(Options.DISPLAY_FIELD)).Value.ToString();
            
            var propertyInfo = PropertyInfoHelper.GetPropertyInfo(typeof(T), displayName);
            
            if (Field.PropertyInfo.GetValue(Field.Model) != null)
            {
                var val = propertyInfo.GetValue(Field.PropertyInfo.GetValue(Field.Model));
                if (val != null)
                {
                    value = val.ToString();
                }
            }

            LinkFieldCheck();

            var result = await OptionsService.GetOptionItemsAsync<T>(ComponentArgs).ConfigureAwait(false);

            var items = GetResponse(result);

            if (items.Any())
            {
                optionItems = items.Select( oi => new GenericItem<T>(oi, propertyInfo));
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
                var optionItem = optionItems.First(
                    o => !string.IsNullOrWhiteSpace(o.Name) && o.Name.Equals(value));
                Field.PropertyInfo.SetValue(Field.Model, optionItem.Item);
            }

            if (Field.HasLinkDependents)
            {
                StateNotification.NotifyStateHasChanged(Field.ContainerUniqueId);
            }
        }
    }
}
