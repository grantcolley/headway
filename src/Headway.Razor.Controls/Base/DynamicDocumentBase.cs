using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Razor.Controls.Model;
using Headway.Razor.Controls.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
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

        protected virtual void HydrateDynamicModel(IResponse<DynamicModel<T>> response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(IResponse<DynamicModel<T>>));
            }

            var responseModel = GetResponse(response);

            if (responseModel == null)
            {
                throw new NullReferenceException(nameof(DynamicModel<T>));
            }

            // rehydrate model... dynamicModel
        }

        protected virtual async Task<DialogResult> CanDeleteDialog(string message)
        {
            return await ShowDialogService.DeleteAsync(message).ConfigureAwait(false);
        }
    }
}
