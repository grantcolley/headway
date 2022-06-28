using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Model;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TabDocumentBase<T> : DynamicDocumentBase<T> where T : class, new()
    {
        protected DynamicContainer activePage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InitializeDynamicModelAsync().ConfigureAwait(false);

            SetActivePage();

            Debug.Print("TabDocumentBase.OnInitializedAsync()");

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected override Task OnParametersSetAsync()
        {
            Debug.Print("TabDocumentBase.OnParametersSetAsync()");

            return base.OnParametersSetAsync();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            Debug.Print("TabDocumentBase.OnAfterRenderAsync()");

            return base.OnAfterRenderAsync(firstRender);
        }

        protected void SetActivePage(DynamicContainer page)
        {
            Debug.Print("TabDocumentBase.SetActivePage()");

            activePage = page;
        }

        protected async Task Submit()
        {
            isSaveInProgress = true;

            if (CurrentEditContext != null
                && CurrentEditContext.Validate())
            {
                IResponse<DynamicModel<T>> response;

                if (dynamicModel.Id.Equals(0))
                {
                    response = await DynamicService
                        .AddDynamicModelAsync<T>(dynamicModel)
                        .ConfigureAwait(false);
                }
                else
                {
                    response = await DynamicService
                        .UpdateDynamicModelAsync<T>(dynamicModel)
                        .ConfigureAwait(false);
                }

                HydrateDynamicModel(response);

                await InvokeAsync(() => this.StateHasChanged());

                Debug.Print("TabDocumentBase.Submit()");
            }

            isSaveInProgress = false;
        }

        protected async Task Delete()
        {
            var deleteResult = await CanDeleteDialog($"Do you really want to delete {dynamicModel.Title}").ConfigureAwait(false);

            if(deleteResult.Cancelled)
            {
                return;
            }

            isDeleteInProgress = true;

            var response = await DynamicService
                .DeleteDynamicModelAsync(dynamicModel)
                .ConfigureAwait(false);

            var result = GetResponse(response);

            if (result.Equals(0))
            {
                return;
            }

            Alert = new Alert
            {
                AlertType = Alerts.INFO,
                Title = dynamicModel.Title,
                Message = $"has been deleted."
            };

            isDeleteInProgress = false;
        }

        private void SetActivePage()
        {
            if(dynamicModel != null)
            {
                if (activePage != null)
                {
                    activePage = dynamicModel.RootContainers.FirstOrDefault(c => c.ContainerId.Equals(activePage.ContainerId));
                }

                if (activePage == null)
                {
                    activePage = dynamicModel.RootContainers.First();
                }
            }
        }
    }
}
