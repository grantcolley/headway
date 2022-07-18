using Headway.Core.Attributes;
using Headway.Core.Notifications;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class DropdownEnumBase<T> : DynamicComponentBase
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        protected List<OptionItemEnum<T>> optionItems = new();

        public T PropertyValue
        {
            get
            {
                return (T)Field.PropertyInfo.GetValue(Field.Model);
            }
            set
            {
                Field.PropertyInfo.SetValue(Field.Model, value);
            }
        }

        protected override Task OnInitializedAsync()
        {
            var type = Type.GetType(nameof(T));
            var optionItemsArray = Enum.GetValues(typeof(T));

            foreach (var optionItem in optionItemsArray)
            {
                var item = optionItem.ToString();
                optionItems.Add(new OptionItemEnum<T>
                {

                    Id = (T)optionItem,
                    Display = item
                });
            }

            return base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            LinkFieldCheck();

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        protected virtual void OnValueChanged(IEnumerable<T> values)
        {
            if (Field.HasLinkDependents)
            {
                StateNotification.NotifyStateHasChanged(Field.ContainerUniqueId);
            }

            StateHasChanged();
        }
    }
}
