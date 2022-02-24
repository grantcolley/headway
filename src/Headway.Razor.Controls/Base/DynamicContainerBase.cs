using Headway.Core.Dynamic;
using Headway.Core.Notifications;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Base
{
    public class DynamicContainerBase : ComponentBase, IDisposable
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [Parameter]
        public DynamicContainer Container { get; set; }

        public void Dispose()
        {
            StateNotification.Deregister(Container.UniqueId);

            GC.SuppressFinalize(this);
        }

        protected async override Task OnInitializedAsync()
        {
            StateNotification.Register(Container.UniqueId, StateHasChanged);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }
    }
}
