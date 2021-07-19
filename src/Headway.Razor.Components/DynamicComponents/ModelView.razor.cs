using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Razor.Components.Base;
using Headway.Razor.Components.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.Razor.Components.DynamicComponents
{
    [DynamicComponent]
    public abstract class ModelViewBase<T> : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public string TypeName { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected DynamicModel<T> dynamicModel;

        protected Alert Alert { get; set; }
        protected bool isSaveInProgress = false;
        protected bool isDeleteInProgress = false;

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

            dynamicModel = GetResponse(serviceResult);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task Submit()
        {
            isSaveInProgress = true;

            IServiceResult<DynamicModel<T>> serviceResult;
            string message;

            if (dynamicModel.Id.Equals(0))
            {
                serviceResult = await AuthorisationService
                    .AddDynamicModelAsync<T>(dynamicModel)
                    .ConfigureAwait(false);

                message = "has been added.";
            }
            else
            {
                serviceResult = await AuthorisationService
                    .UpdateDynamicModelAsync<T>(dynamicModel)
                    .ConfigureAwait(false);

                message = "has been updated.";
            }

            dynamicModel = GetResponse(serviceResult);

            if (dynamicModel == null)
            {
                return;
            }

            Alert = new Alert
            {
                AlertType = "primary",
                Title = dynamicModel.Title,
                Message = message,
                RedirectText = dynamicModel.ModelConfig.NavigateText,
                RedirectPage = dynamicModel.ModelConfig.NavigateTo
            };

            isSaveInProgress = false;
        }

        public async Task Delete()
        {
            isDeleteInProgress = true;

            var serviceResult = await AuthorisationService
                .DeleteDynamicModelAsync(dynamicModel)
                .ConfigureAwait(false);

            var result = GetResponse(serviceResult);

            if (result.Equals(0))
            {
                return;
            }

            Alert = new Alert
            {
                AlertType = "danger",
                Title = $"{dynamicModel.Title}",
                Message = $"has been deleted.",
                RedirectText = dynamicModel.ModelConfig.NavigateText,
                RedirectPage = dynamicModel.ModelConfig.NavigateTo
            };

            isDeleteInProgress = false;
        }
    }
}
