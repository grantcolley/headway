using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorShared.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public partial class ListViewBase<T> : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public string TypeName { get; set; }

        protected DynamicList<T> DynamicList;

        protected List<ListItemConfig> ListItemConfigs;

        protected override async Task OnInitializedAsync()
        {
            var result = 
                await AuthorisationService.GetDynamicListAsync<T>(this.GetType().Name)
                .ConfigureAwait(false);

            DynamicList = GetResponse(result);

            ListItemConfigs = DynamicList.ListConfig.ListItemConfigs;

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void Add()
        {
            NavigationManager.NavigateTo($"/model/{TypeName}");
        }

        protected void Update(object id)
        {
            NavigationManager.NavigateTo($"/model/{TypeName}/{id}");
        }
    }
}
