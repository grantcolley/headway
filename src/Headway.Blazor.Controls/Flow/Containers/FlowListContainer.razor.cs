using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Blazor.Controls.Base;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Flow.Containers
{
    public class FlowListContainerBase : DynamicContainerBase
    {
        protected DynamicContainer activeListItem { get; set; }

        protected MudListItem selectedItem;

        protected string label;
        protected string width;

        protected override async Task OnInitializedAsync()
        {
            label = ComponentArgHelper.GetArgValue(Container.DynamicArgs, FlowConstants.FLOW_LIST_CONTAINER_LABEL);
            width = ComponentArgHelper.GetArgValue(Container.DynamicArgs, FlowConstants.FLOW_LIST_CONTAINER_WIDTH);

            await base.OnInitializedAsync().ConfigureAwait(false);

            SetActiveListItem();
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

                if (activeListItem == null)
                {
                    activeListItem = Container.DynamicContainers.First();
                }
            }
        }
    }
}