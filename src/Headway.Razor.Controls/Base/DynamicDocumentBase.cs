using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Razor.Controls.Model;
using Headway.Razor.Controls.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Base
{
    public abstract class DynamicDocumentBase<T> : HeadwayComponentBase where T : class, new()
    {
        [Inject]
        protected IDynamicApiRequest DynamicService { get; set; }

        [Inject]
        protected IShowDialogService ShowDialogService { get; set; }

        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public int? Id { get; set; }

        protected EditContext CurrentEditContext { get; set; }
        protected DynamicModel<T> dynamicModel { get; set; }

        protected DynamicList<T> dynamicList;
        protected Status Status { get; set; }
        protected Alert Alert { get; set; }

        protected List<string> messages = new();

        protected bool isSaveInProgress = false;

        protected bool isDeleteInProgress = false;

        protected virtual async Task InitializeDynamicModelAsync()
        {
            IResponse<DynamicModel<T>> response;

            if (!Id.HasValue
                || Id.Value.Equals(0))
            {
                response = await DynamicService
                    .CreateDynamicModelInstanceAsync<T>(Config)
                    .ConfigureAwait(false);
            }
            else
            {
                response = await DynamicService
                    .GetDynamicModelAsync<T>(Id.Value, Config)
                    .ConfigureAwait(false);
            }

            SetCurrentModelContext(response);
        }

        protected virtual async Task InitializeDynamicListAsync()
        {
            var result =
                await DynamicService.GetDynamicListAsync<T>(Config)
                .ConfigureAwait(false);

            dynamicList = GetResponse(result);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected virtual void SetCurrentModelContext(IResponse<DynamicModel<T>> response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(IResponse<DynamicModel<T>>));
            }

            dynamicModel = GetResponse(response);

            if(dynamicModel == null)
            {
                throw new NullReferenceException(nameof(DynamicModel<T>));
            }

            CurrentEditContext = new EditContext(dynamicModel.Model);
        }

        protected virtual async Task Submit()
        {
            isSaveInProgress = true;

            if (Validate())
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

                GetResponse(response);
            }

            isSaveInProgress = false;
        }

        protected virtual bool Validate()
        {
            messages.Clear();

            if (CurrentEditContext != null
                && !CurrentEditContext.Validate())
            {
                messages.AddRange(CurrentEditContext.GetValidationMessages());
                return false;
            }

            return true;
        }

        protected virtual async Task Delete()
        {
            if (dynamicModel.Id.Equals(0))
            {
                await ShowDialogService.ShowAsync(
                    "Delete", $"Cannot delete an object with Id equal to 0",
                    "Close", false, Color.Warning, false)
                    .ConfigureAwait(false);
                return;
            }

            var deleteResult = await ShowDialogService.ShowAsync(
                "Delete", $"Do you really want to delete {dynamicModel.Title}",
                "Delete", true, Color.Warning, false)
                .ConfigureAwait(false);

            if (deleteResult.Cancelled)
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
    }
}
