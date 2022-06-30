using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Razor.Controls.Base;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TabDocumentBase<T> : DynamicDocumentBase<T> where T : class, new()
    {
        protected DynamicContainer activePage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InitializeDynamicModelAsync().ConfigureAwait(false);

            SetActivePage();

            Debug.Print("TabDocumentBase.OnInitializedAsync()");

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected override Task OnParametersSetAsync()
        {
            Debug.Print("TabDocumentBase.OnParametersSetAsync()");

            return base.OnParametersSetAsync();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            Debug.Print("TabDocumentBase.OnAfterRenderAsync()");

            return base.OnAfterRenderAsync(firstRender);
        }

        protected void SetActivePage(DynamicContainer page)
        {
            Debug.Print("TabDocumentBase.SetActivePage()");

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
