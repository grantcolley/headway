using Headway.Core.Interface;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.RazorComponents.Pages
{
    public class PermissionsBase : ComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
