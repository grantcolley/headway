using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorShared.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class PermissionDetailsBase : HeadwayComponentBase
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
                var permissionResponse = await AuthorisationService.GetPermissionAsync(PermissionId).ConfigureAwait(false);
                permission = GetResponse(permissionResponse);
            }

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task SubmitPermission()
        {
            IsSaveInProgress = true;

            if(permission.PermissionId.Equals(0))
            {
                var permissionResponse = await AuthorisationService.AddPermissionAsync(permission).ConfigureAwait(false);
                permission = GetResponse(permissionResponse);
                if(permission == null)
                {
                    return;
                }

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
                var permissionResponse = await AuthorisationService.UpdatePermissionAsync(permission).ConfigureAwait(false);
                permission = GetResponse(permissionResponse);
                if (permission == null)
                {
                    return;
                }

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

            var deleteResponse = await AuthorisationService.DeletePermissionAsync(permission.PermissionId).ConfigureAwait(false);
            var deleteResult = GetResponse(deleteResponse);
            if (deleteResult.Equals(0))
            {
                return;
            }

            alert = new Alert
            {
                AlertType = "danger",
                Title = $"{permission.Name}",
                Message = $"has been deleted.",
                RedirectText = "Return to permisions.",
                RedirectPage = "/permissions"
            };

            IsDeleteInProgress = false;
        }
    }
}
