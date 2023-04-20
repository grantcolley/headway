using Headway.Core.Notifications;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Flow.Components
{
    public class FlowComponentBase : ComponentBase
    {
        protected bool OwnerAssigned { get; set; }
        protected string Comment { get; set; }
        protected string ActionText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            ActionText = FlowConstants.FLOW_ACTION_PROCEED;
        }

        protected virtual void OnActionChanged(IEnumerable<string> values)
        {
            if (values != null 
                && values.Count().Equals(1))
            {
                ActionText = values.Single();
            }

            //await InvokeAsync(() =>
            //{
            //    StateHasChanged();
            //});
        }
    }
}
