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
    public partial class ListViewBase<T> : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public string ConfigName { get; set; }

        [Parameter]
        public string ModelName { get; set; }

        protected DynamicList<T> dynamicList;

        protected IEnumerable<ListItemConfig> listItemConfigs;

        protected override async Task OnInitializedAsync()
        {
            var result = 
                await AuthorisationService.GetDynamicListAsync<T>(ConfigName)
                .ConfigureAwait(false);

            dynamicList = GetResponse(result);

            listItemConfigs = dynamicList.ListConfig.ListItemConfigs;

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void Add()
        {
            NavigationManager.NavigateTo($"{dynamicList.ListConfig.NavigateTo}/{ModelName}");
        }

        protected void Update(object id)
        {
            NavigationManager.NavigateTo($"{dynamicList.ListConfig.NavigateTo}/{ModelName}/{id}");
        }
    }
}
