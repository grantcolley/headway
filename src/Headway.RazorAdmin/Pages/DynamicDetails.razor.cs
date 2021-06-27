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
            IServiceResult<DynamicModel<T>> serviceResult;

            if (Id.Equals(0))
            {
                serviceResult = await AuthorisationService
                    .CreateDynamicModelInstanceAsync<T>()
                    .ConfigureAwait(false);
            }
            else
            {
                serviceResult = await AuthorisationService
                    .GetDynamicModelAsync<T>(Id)
                    .ConfigureAwait(false);
            }

            DynamicModel = GetResponse(serviceResult);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task Submit()
        {
            IsSaveInProgress = true;

            IServiceResult<DynamicModel<T>> serviceResult;
            string message;

            if (DynamicModel.Id.Equals(0))
            {
                serviceResult = await AuthorisationService
                    .AddDynamicModelAsync<T>(DynamicModel)
                    .ConfigureAwait(false);

                message = "has been added.";
            }
            else
            {
                serviceResult = await AuthorisationService
                    .UpdateDynamicModelAsync<T>(DynamicModel)
                    .ConfigureAwait(false);

                message = "has been updated.";
            }

            DynamicModel = GetResponse(serviceResult);

            if (DynamicModel == null)
            {
                return;
            }

            Alert = new Alert
            {
                AlertType = "primary",
                Title = $"{DynamicModel.Title}",
                Message = message,
                RedirectText = "Return to permisions.",
                RedirectPage = "/permissions"
            };

            IsSaveInProgress = false;
        }

        public async Task Delete()
        {
            IsDeleteInProgress = true;

            var serviceResult = await AuthorisationService
                .DeleteDynamicModelAsync(DynamicModel)
                .ConfigureAwait(false);

            var result = GetResponse(serviceResult);

            if (result.Equals(0))
            {
                return;
            }

            Alert = new Alert
            {
                AlertType = "danger",
                Title = $"{DynamicModel.Title}",
                Message = $"has been deleted.",
                RedirectText = "Return to permisions.",
                RedirectPage = "/permissions"
            };

            IsDeleteInProgress = false;
        }
    }
}
