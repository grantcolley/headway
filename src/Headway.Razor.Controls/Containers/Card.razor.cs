using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Containers
{
    [DynamicContainer]
    public abstract class CardBase<T> : DynamicContainerBase<T>
    {
        [Inject]
        public IDynamicService DynamicService { get; set; }

        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public int Id { get; set; }

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

            DynamicModel = GetResponse(serviceResult);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task Submit()
        {
            isSaveInProgress = true;

            IServiceResult<DynamicModel<T>> serviceResult;
            string message;

            if (DynamicModel.Id.Equals(0))
            {
                serviceResult = await DynamicService
                    .AddDynamicModelAsync<T>(DynamicModel)
                    .ConfigureAwait(false);

                message = "has been added.";
            }
            else
            {
                serviceResult = await DynamicService
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
                Title = DynamicModel.Title,
                Message = message,
                //RedirectText = dynamicModel.Config.NavigateText,
                RedirectPage = DynamicModel.Config.NavigateTo
            };

            isSaveInProgress = false;
        }

        public async Task Delete()
        {
            isDeleteInProgress = true;

            var serviceResult = await DynamicService
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
                //RedirectText = dynamicModel.Config.NavigateText,
                RedirectPage = DynamicModel.Config.NavigateTo
            };

            isDeleteInProgress = false;
        }
    }
}
