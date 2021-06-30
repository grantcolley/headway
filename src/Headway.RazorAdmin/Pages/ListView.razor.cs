using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorShared.Model;
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
        public string DetailsType { get; set; }

        protected DynamicList<T> DynamicList;

        protected List<ListItemConfig> ListItemConfigs;

        protected override async Task OnInitializedAsync()
        {
            //var result = await AuthorisationService.GetPermissionsAsync().ConfigureAwait(false);
            //Permissions = GetResponse(result);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void Add()
        {
            NavigationManager.NavigateTo($"/details/{DetailsType}");
        }

        protected void Update(object id)
        {
            NavigationManager.NavigateTo($"/details/{DetailsType}/{id}");
        }
    }
}
