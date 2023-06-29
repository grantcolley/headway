using Headway.Blazor.Controls.Flow.Documents;
using Headway.Core.Args;
using Headway.Core.Constants;
using Headway.Core.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Flow.Components
{
    public class FlowComponentBase<T> : ComponentBase where T : class, new()
    {
        [Parameter]
        public FlowTabDocumentBase<T> FlowTabDocument { get; set; }

        protected FlowComponentContext FlowComponentContext { get; set; }

        protected StateStatus ActiveStateStatus { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            FlowComponentContext = FlowTabDocument.DynamicModel.FlowContext.GetFlowComponentContext();

            ActiveStateStatus = FlowComponentContext.ActiveState.StateStatus;
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

            var flowExecutionArgs = new FlowExecutionArgs
            {
                FlowAction = toggled ? FlowActionEnum.TakeOwnership : FlowActionEnum.RelinquishOwnership,
                StateCode = FlowComponentContext.ActiveState.StateCode,
                Comment = FlowComponentContext.Comment,
                Authorisation = FlowComponentContext.FlowContext.Authorisation
            };

            await FlowTabDocument.FlowExecutionAsync(flowExecutionArgs);

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
                var flowAction = FlowComponentContext.ActionText switch
                {
                    FlowConstants.FLOW_ACTION_INITIALISE => FlowActionEnum.Initialise,
                    FlowConstants.FLOW_ACTION_START => FlowActionEnum.Start,
                    FlowConstants.FLOW_ACTION_PROCEED => FlowActionEnum.Complete,
                    FlowConstants.FLOW_ACTION_REGRESS => FlowActionEnum.Reset,
                    _ => FlowActionEnum.Unknown,
                };

                var flowExecutionArgs = new FlowExecutionArgs
                {
                    FlowAction = flowAction,
                    StateCode = FlowComponentContext.ActiveState.StateCode,
                    TargetStateCode = string.IsNullOrWhiteSpace(FlowComponentContext.ActionTarget) ? FlowComponentContext.ActionTarget : null,
                    Comment = FlowComponentContext.Comment,
                    Authorisation = FlowComponentContext.FlowContext.Authorisation
                };

                await FlowTabDocument.FlowExecutionAsync(flowExecutionArgs);
            }

            FlowComponentContext.IsExecuting = false;

            //await InvokeAsync(() =>
            //{
            //    StateHasChanged();
            //});
        }
    }
}