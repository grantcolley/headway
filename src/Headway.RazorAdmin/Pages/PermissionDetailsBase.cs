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
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public int PermissionId { get; set; }

        protected Permission permission { get; set; }

        protected Alert alert { get; set; }

        protected bool IsSaveInProgress = false;
        protected bool IsDeleteInProgress = false;

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

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task SubmitPermission()
        {
            IsSaveInProgress = true;

            if(permission.PermissionId.Equals(0))
            {
                permission = await AuthorisationService.AddPermissionAsync(permission).ConfigureAwait(false);

                alert = new Alert
                {
                    AlertType = "primary",
                    Title = $"{permission.Name}",
                    Message = $"has been added.",
                    RedirectText = "Return to permisions.",
                    RedirectPage = "/permissions"
                };
            }
            else
            {
                permission = await AuthorisationService.UpdatePermissionAsync(permission).ConfigureAwait(false);

                alert = new Alert
                {
                    AlertType = "primary",
                    Title = $"{permission.Name}",
                    Message = $"has been updated.",
                    RedirectText = "Return to permisions.",
                    RedirectPage = "/permissions"
                };
            }

            IsSaveInProgress = false;
        }

        public async Task DeletePermission(Permission permission)
        {
            IsDeleteInProgress = true;

            await AuthorisationService.DeletePermissionAsync(permission.PermissionId).ConfigureAwait(false);

            alert = new Alert
            {
                AlertType = "primary",
                Title = $"{permission.Name}",
                Message = $"has been deleted.",
                RedirectText = "Return to permisions.",
                RedirectPage = "/permissions"
            };

            IsDeleteInProgress = false;
        }
    }
}
