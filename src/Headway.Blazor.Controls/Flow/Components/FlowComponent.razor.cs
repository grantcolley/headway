using Headway.Blazor.Controls.Flow.Documents;
using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Flow.Components
{
    public class FlowComponentBase<T> : ComponentBase where T : class, new()
    {
        private DynamicModel<T> dynamicModel;

        [Parameter]
        public FlowTabDocumentBase<T> FlowTabDocument { get; set; }

        protected FlowComponentContext FlowComponentContext { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            dynamicModel = FlowTabDocument.DynamicModel;
            FlowComponentContext = dynamicModel.FlowContext.GetFlowComponentContext();
        }

        protected virtual void OnActionChanged(IEnumerable<string> values)
        {
            if (values != null
                && values.Count().Equals(1))
            {
                var selected = values.Single();
                if (FlowComponentContext.ActionText != selected)
                {
                    FlowComponentContext.ActionText = selected;
                }
            }

            //await InvokeAsync(() =>
            //{
            //    StateHasChanged();
            //});
        }

        protected virtual void OnActionTargetChanged(IEnumerable<string> values)
        {
            if (values != null
                && values.Count().Equals(1))
            {
                FlowComponentContext.ActionTarget = values.Single();
            }

            //await InvokeAsync(() =>
            //{
            //    StateHasChanged();
            //});
        }

        protected async Task OnOwnerAssignedChangedAsync(bool toggled)
        {
            FlowComponentContext.IsOwnerAssigning = true;

            if (FlowTabDocument?.CurrentEditContext != null)
            {
                // assign / unassign here....
                await Task.Delay(1000);
            }

            FlowComponentContext.IsOwnerAssigning = false;

            //await InvokeAsync(() =>
            //{
            //    StateHasChanged();
            //});
        }

        protected async Task OnExecutingClickAsync()
        {
            FlowComponentContext.IsExecuting = true;

            if (FlowTabDocument?.CurrentEditContext != null
                && FlowTabDocument.CurrentEditContext.Validate())
            {
                // executing here....
                await Task.Delay(1000);
            }

            FlowComponentContext.IsExecuting = false;

            //await InvokeAsync(() =>
            //{
            //    StateHasChanged();
            //});
        }
    }
}