using Headway.Blazor.Controls.Flow.Documents;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using Headway.Core.Interface;
using Headway.Core.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Flow.Components
{
    public class FlowComponentBase<T> : ComponentBase where T : class, new()
    {
        private bool ownerAssigned;
        private DynamicModel<T> dynamicModel;
        protected Core.Model.Flow flow;
        protected Core.Model.State activeState;

        [Parameter]
        public FlowTabDocumentBase<T> FlowTabDocument { get; set; }

        protected string Comment { get; set; }
        protected string ActionText { get; set; }
        protected string ActionTarget { get; set; }
        protected List<string> ActionTextItems { get; set; }
        protected List<string> ActionTargetItems { get; set; }
        protected string OwnerAssignedTooltip { get; set; }

        protected bool OwnerAssigned
        {
            get { return ownerAssigned; }
            set
            {
                if (ownerAssigned != value)
                {
                    ownerAssigned = value;
                }

                OwnerAssignedTooltip = ownerAssigned ? "Relinquish Ownership" : "Take Ownership";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            ActionTextItems = new List<string>();
            ActionTargetItems = new List<string>();

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
                ActionTextItems.Add(FlowConstants.FLOW_ACTION_PROCEED);
            }

            if (activeState.Regressions.Any())
            {
                ActionTextItems.Add(FlowConstants.FLOW_ACTION_REGRESS);
            }

            ActionText = ActionTextItems.FirstOrDefault();

            SetActiveTargetItems();

            OwnerAssigned = !string.IsNullOrWhiteSpace(activeState.Owner);
        }

        protected virtual void OnActionChanged(IEnumerable<string> values)
        {
            if (values != null
                && values.Count().Equals(1))
            {
                var selected = values.Single();
                if (ActionText != selected)
                {
                    ActionText = selected;
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
                ActionTarget = values.Single();
            }

            //await InvokeAsync(() =>
            //{
            //    StateHasChanged();
            //});
        }

        private void SetActiveTargetItems()
        {
            if (string.IsNullOrWhiteSpace(ActionText))
            {
                return;
            }

            ActionTargetItems.Clear();

            if (ActionText == FlowConstants.FLOW_ACTION_PROCEED)
            {
                ActionTargetItems.AddRange(activeState.Transitions.Select(t => t.Name));
            }
            else if (ActionText == FlowConstants.FLOW_ACTION_REGRESS)
            {
                ActionTargetItems.AddRange(activeState.Regressions.Select(r => r.Name));
            }

            ActionTarget = ActionTargetItems.FirstOrDefault();
        }
    }
}