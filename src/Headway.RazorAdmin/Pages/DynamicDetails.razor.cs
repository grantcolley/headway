using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.RazorShared.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public partial class DynamicDetailsBase<T> : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public string DetailsType { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected DynamicModel<T> DynamicModel;
        protected Alert Alert { get; set; }
        protected bool IsSaveInProgress = false;
        protected bool IsDeleteInProgress = false;

        protected override async Task OnInitializedAsync()
        {
            if (Id.Equals(0))
            {
                DynamicModel = AuthorisationService.CreateDynamicModelInstance<T>();
            }
            else
            {
                var response = await AuthorisationService.GetDynamicModelAsync<T>(Id).ConfigureAwait(false);
                DynamicModel = GetResponse(response);
            }

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task Submit()
        {
            IsSaveInProgress = true;

            //if (Permission.PermissionId.Equals(0))
            //{
            //    var permissionResponse = await AuthorisationService.AddPermissionAsync(Permission).ConfigureAwait(false);
            //    Permission = GetResponse(permissionResponse);
            //    if (Permission == null)
            //    {
            //        return;
            //    }

            //    Alert = new Alert
            //    {
            //        AlertType = "primary",
            //        Title = $"{Permission.Name}",
            //        Message = $"has been added.",
            //        RedirectText = "Return to permisions.",
            //        RedirectPage = "/permissions"
            //    };
            //}
            //else
            //{
            //    var permissionResponse = await AuthorisationService.UpdatePermissionAsync(Permission).ConfigureAwait(false);
            //    Permission = GetResponse(permissionResponse);
            //    if (Permission == null)
            //    {
            //        return;
            //    }

            //    Alert = new Alert
            //    {
            //        AlertType = "primary",
            //        Title = $"{Permission.Name}",
            //        Message = $"has been updated.",
            //        RedirectText = "Return to permisions.",
            //        RedirectPage = "/permissions"
            //    };
            //}

            IsSaveInProgress = false;
        }

        public async Task Delete()
        {
            IsDeleteInProgress = true;

            //var deleteResponse = await AuthorisationService.DeletePermissionAsync(permission.PermissionId).ConfigureAwait(false);
            //var deleteResult = GetResponse(deleteResponse);
            //if (deleteResult.Equals(0))
            //{
            //    return;
            //}

            //Alert = new Alert
            //{
            //    AlertType = "danger",
            //    Title = $"{permission.Name}",
            //    Message = $"has been deleted.",
            //    RedirectText = "Return to permisions.",
            //    RedirectPage = "/permissions"
            //};

            IsDeleteInProgress = false;
        }
    }
}
