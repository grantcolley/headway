using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Containers
{
    [DynamicComponent]
    public abstract class CardBase<T> : HeadwayComponentBase
    {
        [Inject]
        public IDynamicService DynamicService { get; set; }

        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public string Title { get; set; }

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
                serviceResult = await DynamicService
                    .CreateDynamicModelInstanceAsync<T>(Config)
                    .ConfigureAwait(false);
            }
            else
            {
                serviceResult = await DynamicService
                    .GetDynamicModelAsync<T>(Id, Config)
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
                serviceResult = await DynamicService
                    .AddDynamicModelAsync<T>(dynamicModel)
                    .ConfigureAwait(false);

                message = "has been added.";
            }
            else
            {
                serviceResult = await DynamicService
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
                //RedirectText = dynamicModel.Config.NavigateText,
                RedirectPage = dynamicModel.Config.NavigateTo
            };

            isSaveInProgress = false;
        }

        public async Task Delete()
        {
            isDeleteInProgress = true;

            var serviceResult = await DynamicService
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
                //RedirectText = dynamicModel.Config.NavigateText,
                RedirectPage = dynamicModel.Config.NavigateTo
            };

            isDeleteInProgress = false;
        }
    }
}
