using Headway.Blazor.Controls.Base;
using Headway.Blazor.Controls.Flow.Components;
using Headway.Blazor.Controls.Flow.Documents;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
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

        protected RenderFragment RenderFlowComponent() => builder =>
        {
            var genericType = typeof(FlowComponent<T>);
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, "FlowTabDocument", (FlowTabDocumentBase<T>)FlowTabDocument);
            builder.CloseComponent();
        };

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