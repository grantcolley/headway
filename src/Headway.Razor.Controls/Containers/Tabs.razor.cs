using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    public abstract class TabsBase : ComponentBase
    {
        [Parameter]
        public DynamicContainer Container { get; set; }

        public DynamicContainer ActivePage { get; set; }

        protected async override Task OnInitializedAsync()
        {
            ActivePage = Container.DynamicContainers.First();

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected string GetTabButtonClass(DynamicContainer page)
        {
            return page == ActivePage ? Css.BTN_PRIMARY : Css.BTN_SECONDARY;
        }

        protected void SetActivePage(DynamicContainer page)
        {
            ActivePage = page;
        }
    }
}
