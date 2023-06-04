using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Extensions;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Notifications;
using Headway.Blazor.Controls.Base;
using Headway.Blazor.Controls.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Components.Mobile
{
    [DynamicComponent]
    public abstract class DropdownComplexBase<T> : DynamicComponentBase
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [Inject]
        public IOptionsApiRequest OptionsApiRequest { get; set; }

        protected IEnumerable<GenericItem<T>> optionItems;

        private GenericItem<T> selectedItem;

        public GenericItem<T> SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    Field.PropertyInfo.SetValue(Field.Model, SelectedItem.Item);
                }
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            LinkFieldCheck();

            var displayName = ComponentArgs.DynamicArgValueToString(Options.DISPLAY_FIELD);

            var propertyInfo = PropertyInfoHelper.GetPropertyInfo(typeof(T), displayName);

            var result = await OptionsApiRequest.GetOptionItemsAsync<T>(ComponentArgs).ConfigureAwait(false);

            var items = GetResponse(result);

            if (items.Any())
            {
                optionItems = items.Select(oi => new GenericItem<T>(oi, propertyInfo));

                if (Field.PropertyInfo.GetValue(Field.Model) != null)
                {
                    var value = propertyInfo.GetValue(Field.PropertyInfo.GetValue(Field.Model));
                    if (value != null)
                    {
                        selectedItem = optionItems.FirstOrDefault(
                            oi => oi.Name != null && oi.Name.Equals(value));
                    }
                }
            }

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        public virtual void OnValueChanged(IEnumerable<GenericItem<T>> values)
        {
            if (Field.HasLinkDependents)
            {
                StateNotification.NotifyStateHasChanged(Field.ContainerUniqueId);
            }
        }
    }
}
