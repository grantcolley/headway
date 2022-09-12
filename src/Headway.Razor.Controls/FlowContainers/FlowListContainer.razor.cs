using Headway.Core.Dynamic;
using Headway.Razor.Controls.Base;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.FlowContainers
{
    public class FlowListContainerBase : DynamicContainerBase
    {
        protected DynamicContainer activeListItem { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            SetActiveListItem();
        }

        protected void SelectedItemChange(MudListItem mudListItem)
        {
            activeListItem = (DynamicContainer)mudListItem.Value;
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