using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorShared.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class UserDetailsBase : ComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public int UserId { get; set; }

        protected User user { get; set; }

        protected Alert alert { get; set; }

        protected bool IsSaveInProgress = false;
        protected bool IsDeleteInProgress = false;

        protected override async Task OnInitializedAsync()
        {
            if (UserId.Equals(0))
            {
                user = new User();
            }
            else
            {
                user = await AuthorisationService.GetUserAsync(UserId).ConfigureAwait(false);
            }

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task SubmitUser()
        {
            IsSaveInProgress = true;

            if (user.UserId.Equals(0))
            {
                user = await AuthorisationService.AddUserAsync(user).ConfigureAwait(false);

                alert = new Alert
                {
                    AlertType = "info",
                    Title = $"{user.UserName}",
                    Message = $"has been added.",
                    RedirectText = "Users",
                    RedirectPage = "/Users"
                };
            }
            else
            {
                user = await AuthorisationService.UpdateUserAsync(user).ConfigureAwait(false);

                alert = new Alert
                {
                    AlertType = "info",
                    Title = $"{user.UserName}",
                    Message = $"has been updated.",
                    RedirectText = "Users",
                    RedirectPage = "/users"
                };
            }

            IsSaveInProgress = false;
        }

        public async Task DeleteUser(User user)
        {
            IsDeleteInProgress = true;

            await AuthorisationService.DeleteUserAsync(user.UserId).ConfigureAwait(false);

            alert = new Alert
            {
                AlertType = "info",
                Title = $"{user.UserName}",
                Message = $"has been deleted.",
                RedirectText = "Users",
                RedirectPage = "/users"
            };

            IsDeleteInProgress = false;
        }
    }
}
