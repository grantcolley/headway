using Headway.Blazor.Controls.Flow.Documents;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Flow.Components
{
    public class FlowComponentBase<T> : ComponentBase where T : class, new()
    {
        private DynamicModel<T> dynamicModel;
        protected Core.Model.Flow flow;
        protected Core.Model.State activeState;

        [Parameter]
        public FlowTabDocumentBase<T> FlowTabDocument { get; set; }

        protected FlowComponentContext FlowComponentContext { get; set; }

        protected override async Task OnInitializedAsync()
        {
            FlowComponentContext = new FlowComponentContext();

            await base.OnInitializedAsync().ConfigureAwait(false);

            dynamicModel = FlowTabDocument.DynamicModel;
            flow = dynamicModel.FlowContext.Flow;
            activeState = flow.ActiveState;

            if (!flow.Bootstrapped)
            {
                flow.Bootstrap(dynamicModel.FlowContext.GetFlowHistory(), true);
            }

            if (activeState.Transitions.Any())
            {
                FlowComponentContext.ActionTextItems.Add(FlowConstants.FLOW_ACTION_PROCEED);
            }

            if (activeState.Regressions.Any())
            {
                FlowComponentContext.ActionTextItems.Add(FlowConstants.FLOW_ACTION_REGRESS);
            }

            FlowComponentContext.ActionText = FlowComponentContext.ActionTextItems.FirstOrDefault();

            SetActiveTargetItems();

            FlowComponentContext.Owner = activeState.Owner;
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

            // assign / unassign here....
            await Task.Delay(1000);
            FlowComponentContext.Owner = string.IsNullOrEmpty(FlowComponentContext.Owner) ? "Test123" : null;                ;

            FlowComponentContext.IsOwnerAssigning = false;
        }

        protected async Task OnExecutingClick()
        {
            FlowComponentContext.IsExecuting = true;

            // executing here....
            await Task.Delay(1000);

            FlowComponentContext.IsExecuting = false;

            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        private void SetActiveTargetItems()
        {
            if (string.IsNullOrWhiteSpace(FlowComponentContext.ActionText))
            {
                return;
            }

            FlowComponentContext.ActionTargetItems.Clear();

            if (FlowComponentContext.ActionText == FlowConstants.FLOW_ACTION_PROCEED)
            {
                FlowComponentContext.ActionTargetItems.AddRange(activeState.Transitions.Select(t => t.Name));
            }
            else if (FlowComponentContext.ActionText == FlowConstants.FLOW_ACTION_REGRESS)
            {
                FlowComponentContext.ActionTargetItems.AddRange(activeState.Regressions.Select(r => r.Name));
            }

            FlowComponentContext.ActionTarget = FlowComponentContext.ActionTargetItems.FirstOrDefault();
        }
    }
}