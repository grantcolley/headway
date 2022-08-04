using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Razor.Controls.Base;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicContainer]
    public abstract class TabsBase : DynamicContainerBase
    {
        public DynamicContainer activePage { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            activePage = Container.DynamicContainers.First();
        }

        protected async void SetActivePage(DynamicContainer page)
        {
            await InvokeAsync(() =>
            {
                activePage = page;

                StateHasChanged();
            });
        }
    }
}
