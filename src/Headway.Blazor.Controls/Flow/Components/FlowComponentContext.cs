using Headway.Core.Extensions;
using Headway.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Blazor.Controls.Flow.Components
{
    public class FlowComponentContext
    {
        private string owner;
        private bool ownerAssigned;

        public FlowComponentContext(IFlowContext flowContext) 
        {
            ActionTextItems = new List<string>();
            ActionTargetItems = new List<string>();

            FlowContext = flowContext;

            InitializeContext();
        }

        public IFlowContext FlowContext { get; private set; }
        public Core.Model.State ActiveState { get; private set; }
        public bool CanRead { get; set; }
        public bool CanEdit { get; set; }
        public bool CanExecute { get; set; }
        public bool CanManageOwnership { get; set; }
        public bool ShowOwnership { get; set; }
        public bool IsOwnerAssigning { get; set; }
        public bool IsExecuting { get; set; }
        public string OwnerAssignedTooltip { get; set; }
        public string ActionText { get; set; }
        public List<string> ActionTextItems { get; set; }
        public string ActionTarget { get; set; }
        public List<string> ActionTargetItems { get; set; }
        public string Comment { get; set; }

        public string Owner
        {
            get { return owner; }
            set
            {
                if(owner != value) 
                {
                    owner = value;
                }

                OwnerAssigned = !string.IsNullOrWhiteSpace(owner);
            }
        }

        public bool OwnerAssigned
        {
            get { return ownerAssigned; }
            set
            {
                if (ownerAssigned != value)
                {
                    ownerAssigned = value;
                }

                OwnerAssignedTooltip = ownerAssigned ? FlowConstants.FLOW_STATE_RELINQUISH_OWNERSHIP : FlowConstants.FLOW_STATE_TAKE_OWNERSHIP;
            }
        }

        private void InitializeContext()
        {
            ActiveState = FlowContext.Flow.ActiveState;

            Owner = ActiveState.Owner;

            if (ActiveState.Transitions.Any())
            {
                ActionTextItems.Add(FlowConstants.FLOW_ACTION_PROCEED);
            }

            if (ActiveState.Regressions.Any())
            {
                ActionTextItems.Add(FlowConstants.FLOW_ACTION_REGRESS);
            }

            ActionText = ActionTextItems.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(ActionText))
            {
                return;
            }

            if (ActionText == FlowConstants.FLOW_ACTION_PROCEED)
            {
                ActionTargetItems.AddRange(ActiveState.Transitions.Select(t => t.Name));
            }
            else if (ActionText == FlowConstants.FLOW_ACTION_REGRESS)
            {
                ActionTargetItems.AddRange(ActiveState.Regressions.Select(r => r.Name));
            }

            ActionTarget = ActionTargetItems.FirstOrDefault();
        }
    }
}
