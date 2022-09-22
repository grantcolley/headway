using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Blazor.Controls.Base;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TabDocumentBase<T> : DynamicDocumentBase<T> where T : class, new()
    {
        protected DynamicContainer activePage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            await InitializeDynamicModelAsync().ConfigureAwait(false);

            SetActivePage();
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }

        protected void SetActivePage(DynamicContainer page)
        {
            activePage = page;
        }

        private void SetActivePage()
        {
            if(dynamicModel != null)
            {
                if (activePage != null)
                {
                    activePage = dynamicModel.RootContainers.FirstOrDefault(c => c.ContainerId.Equals(activePage.ContainerId));
                }

                if (activePage == null)
                {
                    activePage = dynamicModel.RootContainers.First();
                }
            }
        }
    }
}
