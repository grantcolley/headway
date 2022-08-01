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

namespace Headway.Razor.Controls.SearchComponents
{
    [DynamicComponent]
    public abstract class SearchDropdownBase : SearchItemComponentBase
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        protected IEnumerable<OptionItem> optionItems;

        protected override async Task OnParametersSetAsync()
        {
            LinkFieldCheck();

            var result = await Mediator.Send(new SearchOptionItemsRequest(ComponentArgs)).ConfigureAwait(false);

            optionItems = GetResponse(result.OptionItems);

            OptionItem selectedItem = null;

            if (!string.IsNullOrWhiteSpace(SearchItem.Value))
            {
                selectedItem = optionItems.FirstOrDefault(o => o.Id != null && o.Id.Equals(SearchItem.Value));
            }

            if (selectedItem == null)
            {
                selectedItem = optionItems.First();
            }

            PropertyValue = selectedItem.Id;

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        protected virtual void OnValueChanged(IEnumerable<string> values)
        {
            if (SearchItem.HasLinkDependents)
            {
                StateNotification.NotifyStateHasChanged(SearchItem.SearchComponentUniqueId);
            }

            StateHasChanged();
        }
    }
}
