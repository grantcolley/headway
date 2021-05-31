using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorShared.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class PermissionsBase : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        public IEnumerable<Permission> Permissions { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = await AuthorisationService.GetPermissionsAsync().ConfigureAwait(false);
            Permissions = GetResponse(result);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void AddPermission()
        {
            NavigationManager.NavigateTo("/permissiondetails");
        }

        protected void UpdatePermission(int permissionId)
        {
            NavigationManager.NavigateTo($"/permissiondetails/{permissionId}");
        }
    }
}
