using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Razor.Components.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Components.DynamicComponents
{
    [DynamicComponent]
    public abstract class ListViewBase<T> : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public string ConfigName { get; set; }

        [Parameter]
        public string ModelName { get; set; }

        protected DynamicList<T> dynamicList;

        protected IEnumerable<ConfigItem> configItems;

        protected override async Task OnInitializedAsync()
        {
            var result = 
                await AuthorisationService.GetDynamicListAsync<T>(ConfigName)
                .ConfigureAwait(false);

            dynamicList = GetResponse(result);

            configItems = dynamicList.Config.ConfigItems;

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void Add()
        {
            NavigationManager.NavigateTo($"{dynamicList.Config.NavigateTo}/{ModelName}");
        }

        protected void Update(object id)
        {
            NavigationManager.NavigateTo($"{dynamicList.Config.NavigateTo}/{ModelName}/{id}");
        }
    }
}
