using Headway.Core.Dynamic;
using Headway.Core.Mediators;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Base
{
    public class DynamicContainerBase : ComponentBase, IDisposable
    {
        [Inject]
        public IStateNotificationMediator StateNotification { get; set; }

        [Parameter]
        public DynamicContainer Container { get; set; }

        protected async override Task OnInitializedAsync()
        {
            StateNotification.Register(Container.UniqueId, StateHasChanged);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            StateNotification.Deregister(Container.UniqueId);

            GC.SuppressFinalize(this);
        }
    }
}
