using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorShared.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorShared.Components
{
    public partial class ListViewBase<T> : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public string TypeName { get; set; }

        protected DynamicList<T> dynamicList;

        protected IEnumerable<ListItemConfig> listItemConfigs;

        protected override async Task OnInitializedAsync()
        {
            var result = 
                await AuthorisationService.GetDynamicListAsync<T>(this.GetType().Name)
                .ConfigureAwait(false);

            dynamicList = GetResponse(result);

            listItemConfigs = dynamicList.ListConfig.ListItemConfigs;

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void Add()
        {
            NavigationManager.NavigateTo($"{dynamicList.ListConfig.NavigateTo}/{TypeName}");
        }

        protected void Update(object id)
        {
            NavigationManager.NavigateTo($"{dynamicList.ListConfig.NavigateTo}/{TypeName}/{id}");
        }
    }
}
