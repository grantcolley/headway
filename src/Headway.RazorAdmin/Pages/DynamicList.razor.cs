using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.RazorShared.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public partial class DynamicListBase<T> : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public string DetailsType { get; set; }

        protected IEnumerable<DynamicModel<T>> DynamicModels;

        protected override async Task OnInitializedAsync()
        {
            //var result = await AuthorisationService.GetPermissionsAsync().ConfigureAwait(false);
            //Permissions = GetResponse(result);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void AddPermission()
        {
            NavigationManager.NavigateTo($"/details/{DetailsType}");
        }

        protected void UpdatePermission(int permissionId)
        {
            NavigationManager.NavigateTo($"/details/{DetailsType}/{permissionId}");
        }
    }
}
