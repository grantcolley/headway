using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headway.RazorComponents.Pages
{
    public class UserDetailsBase : ComponentBase
    {
        [Inject]
        public IUserService UserService { get; set; }

        [Parameter]
        public User User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
