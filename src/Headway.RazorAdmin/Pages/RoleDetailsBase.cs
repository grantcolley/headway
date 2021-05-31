using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorShared.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class RoleDetailsBase : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public int RoleId { get; set; }

        protected Role role { get; set; }

        protected Alert alert { get; set; }

        protected bool IsSaveInProgress = false;
        protected bool IsDeleteInProgress = false;

        protected override async Task OnInitializedAsync()
        {
            if (RoleId.Equals(0))
            {
                role = new Role();
            }
            else
            {
                var roleResponse = await AuthorisationService.GetRoleAsync(RoleId).ConfigureAwait(false);
                role = GetResponse(roleResponse);
                if(role == null)
                {
                    return;
                }
            }

            // get permissions....

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task SubmitRole()
        {
            IsSaveInProgress = true;

            if (role.RoleId.Equals(0))
            {
                var roleResponse = await AuthorisationService.AddRoleAsync(role).ConfigureAwait(false);
                role = GetResponse(roleResponse);
                if(role == null)
                {
                    return;
                }

                alert = new Alert
                {
                    AlertType = "primary",
                    Title = $"{role.Name}",
                    Message = $"has been added.",
                    RedirectText = "Return to roles.",
                    RedirectPage = "/roles"
                };
            }
            else
            {
                var roleResponse = await AuthorisationService.UpdateRoleAsync(role).ConfigureAwait(false);
                role = GetResponse(roleResponse);
                if (role == null)
                {
                    return;
                }

                alert = new Alert
                {
                    AlertType = "primary",
                    Title = $"{role.Name}",
                    Message = $"has been updated.",
                    RedirectText = "Return to roles.",
                    RedirectPage = "/roles"
                };
            }

            IsSaveInProgress = false;
        }

        public async Task DeleteRole(Role role)
        {
            IsDeleteInProgress = true;

            var deleteResponse = await AuthorisationService.DeleteRoleAsync(role.RoleId).ConfigureAwait(false);
            var deleteResult = GetResponse(deleteResponse);
            if(deleteResult.Equals(0))
            {
                return;
            }

            alert = new Alert
            {
                AlertType = "primary",
                Title = $"{role.Name}",
                Message = $"has been deleted.",
                RedirectText = "Return to roles.",
                RedirectPage = "/roles"
            };

            IsDeleteInProgress = false;
        }
    }
}
