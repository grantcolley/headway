using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorShared.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class PermissionDetailsBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public int PermissionId { get; set; }

        protected Permission permission { get; set; }

        protected Alert alert { get; set; }

        protected bool InProgress = false;

        protected override async Task OnInitializedAsync()
        {
            if(PermissionId.Equals(0))
            {
                permission = new Permission();
            }
            else
            {
                permission = await AuthorisationService.GetPermissionAsync(PermissionId).ConfigureAwait(false);
            }

            await base.OnInitializedAsync();
        }

        protected async Task SubmitPermission()
        {
            InProgress = true;

            if(permission.PermissionId.Equals(0))
            {
                permission = await AuthorisationService.AddPermissionAsync(permission).ConfigureAwait(false);

                alert = new Alert
                {
                    AlertType = "info",
                    Title = $"Added",
                    Message = $"{permission.Name} has been added.",
                    RedirectText = "Permisions",
                    RedirectPage = "/permissions"
                };
            }
            else
            {
                permission = await AuthorisationService.UpdatePermissionAsync(permission).ConfigureAwait(false);

                alert = new Alert
                {
                    AlertType = "info",
                    Title = $"Updated",
                    Message = $"{permission.Name} has been updated.",
                    RedirectText = "Permisions",
                    RedirectPage = "/permissions"
                };
            }

            InProgress = false;
        }

        public async Task DeletePermission(Permission permission)
        {
            InProgress = true;
            await AuthorisationService.DeletePermissionAsync(permission.PermissionId).ConfigureAwait(false);

            alert = new Alert
            {
                AlertType = "info",
                Title = $"Deleted",
                Message = $"{permission.Name} has been deleted.",
                RedirectText = "Permisions",
                RedirectPage = "/permissions"
            };

            InProgress = false;
        }
    }
}
