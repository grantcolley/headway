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

        protected Permission Permission { get; set; }

        protected Alert Alert { get; set; }

        protected bool IsSaveInProgress = false;
        protected bool IsDeleteInProgress = false;

        protected override async Task OnInitializedAsync()
        {
            if(PermissionId.Equals(0))
            {
                Permission = new Permission();
            }
            else
            {
                var permissionResponse = await AuthorisationService.GetPermissionAsync(PermissionId).ConfigureAwait(false);
                Permission = GetResponse(permissionResponse);
            }

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task SubmitPermission()
        {
            IsSaveInProgress = true;

            if(Permission.PermissionId.Equals(0))
            {
                var permissionResponse = await AuthorisationService.AddPermissionAsync(Permission).ConfigureAwait(false);
                Permission = GetResponse(permissionResponse);
                if(Permission == null)
                {
                    return;
                }

                Alert = new Alert
                {
                    AlertType = "primary",
                    Title = $"{Permission.Name}",
                    Message = $"has been added.",
                    RedirectText = "Return to permisions.",
                    RedirectPage = "/permissions"
                };
            }
            else
            {
                var permissionResponse = await AuthorisationService.UpdatePermissionAsync(Permission).ConfigureAwait(false);
                Permission = GetResponse(permissionResponse);
                if (Permission == null)
                {
                    return;
                }

                Alert = new Alert
                {
                    AlertType = "primary",
                    Title = $"{Permission.Name}",
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

            Alert = new Alert
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
