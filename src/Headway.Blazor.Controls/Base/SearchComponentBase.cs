using Headway.Core.Dynamic;
using Headway.Core.Notifications;
using Headway.Blazor.Controls.Callbacks;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Base
{
    public class SearchComponentBase : ComponentBase, IDisposable
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [CascadingParameter]
        public SearchCallback SearchCallBack { get; set; }

        [Parameter]
        public string SearchComponentUniqueId { get; set; }

        [Parameter]
        public List<DynamicSearchItem> SearchItems { get; set; }

        protected SearchItemCallback searchItemCallback = new();

        protected bool isSearchInProgress = false;

        public void Dispose()
        {
            StateNotification.Deregister(SearchComponentUniqueId);

            GC.SuppressFinalize(this);
        }

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            searchItemCallback.OnKeyDown = SearchItemEnterAsync;

            StateNotification.Register(SearchComponentUniqueId, StateHasChanged);
        }

        protected async Task OnClickAsync()
        {
            isSearchInProgress = true;

            if(SearchCallBack != null
                && SearchCallBack.Click != null)
            {
                await SearchCallBack.Click.Invoke();
            }

            isSearchInProgress = false;

            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        protected async Task SearchItemEnterAsync()
        {
            await OnClickAsync();
        }
    }
}
