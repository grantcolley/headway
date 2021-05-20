using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorComponents.Pages
{
    public class UsersBase : ComponentBase
    {
        [Inject]
        public IAuthorisationService UserService { get; set; }

        public IEnumerable<User> Users { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Users = await UserService.GetUsersAsync();

            await base.OnInitializedAsync();
        }
    }
}
