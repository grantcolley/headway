using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class RolesBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Roles = await AuthorisationService.GetRolesAsync().ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void AddRole()
        {
            NavigationManager.NavigateTo("/roledetails");
        }

        protected void UpdateRole(int roleId)
        {
            NavigationManager.NavigateTo($"/roledetails/{roleId}");
        }
    }
}
