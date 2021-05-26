using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class UsersBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        public IEnumerable<User> Users { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Users = await AuthorisationService.GetUsersAsync().ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void AddUser()
        {
            NavigationManager.NavigateTo("/userdetails");
        }

        protected void UpdateUser(int userId)
        {
            NavigationManager.NavigateTo($"/userdetails/{userId}");
        }
    }
}
