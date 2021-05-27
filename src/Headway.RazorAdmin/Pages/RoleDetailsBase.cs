using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorShared.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class RoleDetailsBase : ComponentBase
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
                role = await AuthorisationService.GetRoleAsync(RoleId).ConfigureAwait(false);
            }

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task SubmitRole()
        {
            IsSaveInProgress = true;

            if (role.RoleId.Equals(0))
            {
                role = await AuthorisationService.AddRoleAsync(role).ConfigureAwait(false);

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
                role = await AuthorisationService.UpdateRoleAsync(role).ConfigureAwait(false);

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

            await AuthorisationService.DeleteRoleAsync(role.RoleId).ConfigureAwait(false);

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
