using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using Headway.Core.Interface;
using Headway.Core.Model;
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
    public abstract class DynamicDocumentBase<T> : HeadwayComponentBase, IDisposable where T : class, new()
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

        protected List<Arg> args = new();

        protected Status Status { get; set; }

        protected Alert Alert { get; set; }

        protected List<string> messages = new();

        protected bool isSaveInProgress = false;

        protected bool isDeleteInProgress = false;

        private bool disposedValue;

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

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

            dynamicModel = GetResponse(response);

            ExtractArgs(dynamicModel.Config);

            CurrentEditContext = new EditContext(dynamicModel.Model);
            CurrentEditContext.OnValidationStateChanged += CurrentEditContextOnValidationStateChanged;
        }

        protected virtual async Task InitializeDynamicListAsync()
        {
            var result =
                await DynamicService.GetDynamicListAsync<T>(Config)
                .ConfigureAwait(false);

            dynamicList = GetResponse(result);

            ExtractArgs(dynamicList.Config);
        }

        protected void ExtractArgs(Config config)
        {
            if (config != null
                && !string.IsNullOrWhiteSpace(config.DocumentArgs))
            {
                args = config.DocumentArgs.ToArgsList();
            }
        }

        protected virtual async Task Submit()
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

                GetResponse(response);
            }

            isSaveInProgress = false;
        }

        protected virtual void GetValidationMessages()
        {
            var before = messages.Count;

            messages.Clear();
            messages.AddRange(CurrentEditContext.GetValidationMessages());

            var after = messages.Count;

            if(before != after)
            {
                StateHasChanged();
            }
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(CurrentEditContext != null)
                    {
                        CurrentEditContext.OnValidationStateChanged -= CurrentEditContextOnValidationStateChanged;
                    }
                }

                CurrentEditContext = null;
                dynamicModel = null;
                dynamicList = null;
                messages = null;

                disposedValue = true;
            }
        }

        private void CurrentEditContextOnValidationStateChanged(object sender, ValidationStateChangedEventArgs e)
        {
            GetValidationMessages();
        }
    }
}
