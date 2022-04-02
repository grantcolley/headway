using Headway.Core.Attributes;
using Headway.Core.Model;
using Headway.Core.Notifications;
using Headway.Razor.Controls.Base;
using Headway.RequestApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class DropdownBase : DynamicComponentBase
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        protected IEnumerable<OptionItem> optionItems;

        private OptionItem selectedItem;

        public OptionItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if(selectedItem != value)
                {
                    selectedItem = value;
                    Field.PropertyInfo.SetValue(Field.Model, SelectedItem.Id);
                }
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            LinkFieldCheck();

            var result = await Mediator.Send(new OptionItemsRequest(ComponentArgs)).ConfigureAwait(false);

            optionItems = GetResponse(result.OptionItems);

            var id = Field.PropertyInfo.GetValue(Field.Model)?.ToString();
            if (!string.IsNullOrWhiteSpace(id))
            {
                SelectedItem = optionItems.FirstOrDefault(o => o.Id != null && o.Id.Equals(id));
            }

            if(selectedItem == null)
            {
                selectedItem = optionItems.FirstOrDefault();
            }

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        public virtual void OnValueChanged(IEnumerable<OptionItem> values)
        {
            if (Field.HasLinkDependents)
            {
                StateNotification.NotifyStateHasChanged(Field.ContainerUniqueId);
            }
        }
    }
}
