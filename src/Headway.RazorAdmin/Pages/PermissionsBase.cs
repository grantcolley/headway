using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class PermissionsBase : ComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        public List<Permission> Permissions { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var permissions = await AuthorisationService.GetPermissionsAsync().ConfigureAwait(false);
            Permissions = new List<Permission>(permissions);

            await base.OnInitializedAsync();
        }
    }
}
