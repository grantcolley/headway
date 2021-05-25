using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.RazorComponents.Pages
{
    public class PermissionsBase : ComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        public List<Permission> Permissions { get; set; }

        protected Permission addPermission = new();

        protected bool InProgress = false;

        protected override async Task OnInitializedAsync()
        {
            var permissions = await AuthorisationService.GetPermissionsAsync().ConfigureAwait(false);
            Permissions = new List<Permission>(permissions);

            await base.OnInitializedAsync();
        }

        protected async Task SubmitPermission()
        {
            InProgress = true;
            var newPermission = new Permission { Name = addPermission.Name };
            var permission = await AuthorisationService.AddPermissionAsync(newPermission);
            Permissions.Add(permission);
            addPermission.Name = string.Empty;
            InProgress = false;
        }

        public void EditPermission(Permission permission)
        {
            permission.CanEdit = true;
        }

        public void UndoUpdatePermission(Permission permission)
        {
            permission.CanEdit = false;
        }

        public async Task UpdatePermission(Permission permission)
        {
            InProgress = true;
            await AuthorisationService.UpdatePermissionAsync(permission);
            InProgress = false;
        }

        public async Task DeletePermission(Permission permission)
        {
            InProgress = true;
            await AuthorisationService.DeletePermissionAsync(permission.PermissionId);
            Permissions.Remove(permission);
            InProgress = false;
        }
    }
}
