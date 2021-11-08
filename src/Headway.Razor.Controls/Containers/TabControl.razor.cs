using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Containers
{
    [DynamicContainer]
    public abstract class TabControlBase<T> : DynamicContainerBase<T> where T : class, new()
    {
        [Inject]
        public IDynamicService DynamicService { get; set; }

        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public TabPage<T> ActivePage { get; set; }
        protected List<TabPage<T>> Pages = new List<TabPage<T>>();
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

        internal void AddPage(TabPage<T> tabPage)
        {
            Pages.Add(tabPage);

            if (Pages.Count == 1)
            {
                ActivePage = tabPage;
            }

            StateHasChanged();
        }

        protected string GetTabButtonClass(TabPage<T> page)
        {
            return page == ActivePage ? "btn-primary" : "btn-secondary";
        }

        protected void SetActivePage(TabPage<T> page)
        {
            ActivePage = page;
        }

        protected async Task Submit()
        {
            isSaveInProgress = true;

            IServiceResult<DynamicModel<T>> serviceResult;
            string message;

            if (DynamicModel.Id.Equals(0))
            {
                //serviceResult = await DynamicService
                //    .AddDynamicModelAsync<T>(DynamicModel)
                //    .ConfigureAwait(false);

                message = "has been added.";
            }
            else
            {
                //serviceResult = await DynamicService
                //    .UpdateDynamicModelAsync<T>(DynamicModel)
                //    .ConfigureAwait(false);

                message = "has been updated.";
            }

            //DynamicModel = GetResponse(serviceResult);

            if (DynamicModel == null)
            {
                return;
            }

            Alert = new Alert
            {
                AlertType = Alerts.PRIMARY,
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

            //var serviceResult = await DynamicService
            //    .DeleteDynamicModelAsync(DynamicModel)
            //    .ConfigureAwait(false);

            //var result = GetResponse(serviceResult);

            //if (result.Equals(0))
            //{
            //    return;
            //}

            Alert = new Alert
            {
                AlertType = Alerts.DANGER,
                Title = DynamicModel.Title,
                Message = $"has been deleted.",
                //RedirectText = dynamicModel.Config.NavigateText,
                RedirectPage = DynamicModel.Config.NavigateTo
            };

            isDeleteInProgress = false;
        }
    }
}
