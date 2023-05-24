using Headway.Blazor.Controls.Base;
using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Flow.Documents
{
    [DynamicDocument]
    public abstract class FlowTabDocumentBase<T> : FlowDocumentBase<T> where T : class, new()
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

        protected RenderFragment RenderFlowListItemContainer() => builder =>
        {
            var type = DynamicModel.Model.GetType();
            var genericType = activePage.DynamicComponent.MakeGenericType(new[] { type });
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, "Container", activePage);
            builder.AddAttribute(3, "FlowTabDocument", (FlowTabDocumentBase<T>)this);
            builder.CloseComponent();
        };

        private void SetActivePage()
        {
            if(DynamicModel != null)
            {
                if (activePage != null)
                {
                    activePage = DynamicModel.RootContainers.FirstOrDefault(c => c.ContainerId.Equals(activePage.ContainerId));
                }

                if (activePage == null)
                {
                    activePage = DynamicModel.RootContainers.First();
                }
            }
        }
    }
}
