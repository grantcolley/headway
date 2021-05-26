using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class PermissionsBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        public IEnumerable<Permission> Permissions { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Permissions = await AuthorisationService.GetPermissionsAsync().ConfigureAwait(false);

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
