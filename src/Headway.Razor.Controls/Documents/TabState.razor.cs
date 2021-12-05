using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TabStateBase<T> : DynamicDocumentBase<T> where T : class, new()
    {
        protected Alert Alert { get; set; }
        protected bool isSaveInProgress = false;
        protected bool isDeleteInProgress = false;
        protected DynamicContainer activePage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InitializeDynamicModelAsync().ConfigureAwait(false);

            activePage = dynamicModel.RootContainers.First();

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected string GetTabButtonClass(DynamicContainer page)
        {
            return page == activePage ? Css.BTN_PRIMARY : Css.BTN_SECONDARY;
        }

        protected void SetActivePage(DynamicContainer page)
        {
            activePage = page;
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
                AlertType = Alerts.PRIMARY,
                Title = dynamicModel.Title,
                Message = message,
                //RedirectText = dynamicModel.Config.NavigateText,
                RedirectPage = dynamicModel.Config.NavigateTo
            };

            isSaveInProgress = false;
        }

        protected async Task Delete()
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
                AlertType = Alerts.DANGER,
                Title = dynamicModel.Title,
                Message = $"has been deleted.",
                //RedirectText = dynamicModel.Config.NavigateText,
                RedirectPage = dynamicModel.Config.NavigateTo
            };

            isDeleteInProgress = false;
        }
    }
}
