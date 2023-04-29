using System.Collections.Generic;

namespace Headway.Blazor.Controls.Flow.Components
{
    public class FlowComponentContext
    {
        private string owner;
        private bool ownerAssigned;

        public FlowComponentContext() 
        {
            ActionTextItems = new List<string>();
            ActionTargetItems = new List<string>();
        }

        public bool ShowOwnership { get; set; }
        public bool IsOwnerAssigning { get; set; }
        public string OwnerAssignedTooltip { get; set; }
        public string ActionText { get; set; }
        public List<string> ActionTextItems { get; set; }
        public string ActionTarget { get; set; }
        public List<string> ActionTargetItems { get; set; }
        public string Comment { get; set; }
        public bool IsExecuting { get; set; }

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
    }
}
