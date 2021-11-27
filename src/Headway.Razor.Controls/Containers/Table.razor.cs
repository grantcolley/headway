using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Containers
{
    [DynamicContainer]
    public abstract class TableBase<T> : DynamicContainerBase<T> where T : class, new()
    {
        protected DynamicList<T> dynamicList;

        protected override async Task OnInitializedAsync()
        {
            var result = 
                await DynamicService.GetDynamicListAsync<T>(Config)
                .ConfigureAwait(false);

            dynamicList = GetResponse(result);

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
