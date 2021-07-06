using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorShared.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class RolesBase : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var rolesResponse = await AuthorisationService.GetRolesAsync().ConfigureAwait(false);
            Roles = GetResponse(rolesResponse);

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
