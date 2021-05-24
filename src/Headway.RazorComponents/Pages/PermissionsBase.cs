using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorComponents.Pages
{
    public class PermissionsBase : ComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        public IEnumerable<Permission> Permissions { get; set; }

        protected Permission addPermission = new();

        protected bool InProgress = false;

        protected override async Task OnInitializedAsync()
        {
            Permissions = await AuthorisationService.GetPermissionsAsync();

            await base.OnInitializedAsync();
        }

        protected async Task SubmitPermission()
        {
            InProgress = true;
            var newPermission = new Permission { Name = addPermission.Name };
            var result = await AuthorisationService.AddPermissionAsync(newPermission);
            addPermission.Name = string.Empty;
            InProgress = false;
        }
    }
}
