using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TableBase<T> : DynamicDocumentBase<T> where T : class, new()
    {
        protected override async Task OnInitializedAsync()
        {
            await InitializeDynamicListAsync().ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void Add()
        {
            NavigationManager.NavigateTo($"{dynamicList.Config.NavigateTo}/{dynamicList.Config.NavigateToConfig}");
        }

        protected void Update(object id)
        {
            NavigationManager.NavigateTo($"{dynamicList.Config.NavigateTo}/{dynamicList.Config.NavigateToConfig}/{id}");
        }
    }
}
