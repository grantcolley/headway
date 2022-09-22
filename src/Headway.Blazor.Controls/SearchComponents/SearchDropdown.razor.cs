using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Extensions;
using Headway.Core.Model;
using Headway.Core.Notifications;
using Headway.Blazor.Controls.Base;
using Headway.RequestApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.SearchComponents
{
    [DynamicSearchItemComponent]
    public abstract class SearchDropdownBase : SearchItemComponentBase
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        protected IEnumerable<OptionItem> optionItems;

        protected string style = string.Empty;

        protected override Task OnInitializedAsync()
        {
            var styleArg = ComponentArgs.ArgOrDefault(Args.STYLE);

            if (styleArg != null)
            {
                style = styleArg.Value;
            }

            return base.OnInitializedAsync();
        }

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

        protected async virtual void OnValueChanged(IEnumerable<string> values)
        {
            if (SearchItem.HasLinkDependents)
            {
                StateNotification.NotifyStateHasChanged(SearchItem.SearchComponentUniqueId);
            }

            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }
    }
}
