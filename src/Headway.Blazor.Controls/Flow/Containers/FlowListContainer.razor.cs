using Headway.Blazor.Controls.Base;
using Headway.Blazor.Controls.Flow.Components;
using Headway.Blazor.Controls.Flow.Documents;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using Headway.Core.Interface;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Flow.Containers
{
    public class FlowListContainerBase<T> : DynamicContainerBase where T : class, new()
    {
        [Parameter]
        public FlowTabDocumentBase<T> FlowTabDocument { get; set; }

        protected IFlowContext flowContext { get; set; }

        protected DynamicContainer activeListItem { get; set; }

        protected MudListItem selectedItem;

        protected string label;
        protected string width;

        protected override async Task OnInitializedAsync()
        {
            label = Container.DynamicArgs.FirstDynamicArgValueToString(FlowConstants.FLOW_LIST_CONTAINER_LABEL);
            width = Container.DynamicArgs.FirstDynamicArgValueToString(FlowConstants.FLOW_LIST_CONTAINER_WIDTH);

            await base.OnInitializedAsync().ConfigureAwait(false);

            flowContext = FlowTabDocument?.DynamicModel?.FlowContext;

            SetActiveListItem();
        }

        protected RenderFragment RenderFlowComponent() => builder =>
        {
            var genericType = typeof(FlowComponent<T>);
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, "FlowTabDocument", (FlowTabDocumentBase<T>)FlowTabDocument);
            builder.CloseComponent();
        };

        protected bool ShowContainer(DynamicContainer dynamicContainer)
        {
            var flowStateCode = dynamicContainer.DynamicArgs.FirstDynamicArgValueToString(FlowConstants.FLOW_STATE_CODE);

            if(flowContext.Flow.ActiveState.StateCode.Equals(flowStateCode))
            {
                return true;
            }

            var replayHistory = flowContext.Flow.ReplayHistory.SingleOrDefault(h => h.StateCode.Equals(flowStateCode));

            return replayHistory == null ? false : true;
        }

        protected void SelectedValueChange(object value)
        {
            activeListItem = (DynamicContainer)value;
        }

        private void SetActiveListItem()
        {
            if (Container != null)
            {
                if (activeListItem != null)
                {
                    activeListItem = Container.DynamicContainers.FirstOrDefault(c => c.ContainerId.Equals(activeListItem.ContainerId));
                }
                else
                {
                    activeListItem ??= Container.DynamicContainers.First();
                }
            }
        }
    }
}