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
        protected string OwnerAssignedTooltip { get; set; }
        
        protected bool OwnerAssigned 
        {
            get { return ownerAssigned; }
            set
            {
                if(ownerAssigned != value)
                {
                    ownerAssigned = value;

                    OwnerAssignedTooltip = ownerAssigned ? "Relinquish Ownership" : "Take Ownership";
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            ActionText = FlowConstants.FLOW_ACTION_PROCEED;

            dynamicModel = FlowTabDocument.DynamicModel;
            flow = dynamicModel.FlowContext.Flow;
            activeState = flow.ActiveState;

            if (!flow.Bootstrapped)
            {
                flow.Bootstrap(dynamicModel.FlowContext.GetFlowHistory(), true);
            }

            OwnerAssigned = !string.IsNullOrWhiteSpace(activeState.Owner);
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
