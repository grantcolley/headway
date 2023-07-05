using Headway.Core.Args;
using Headway.Core.Enums;
using Headway.Core.Extensions;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Base
{
    public abstract class FlowDocumentBase<T> : DynamicDocumentBase<T> where T : class, new()
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await InitializeDynamicModelAsync().ConfigureAwait(false);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (!DynamicModel.FlowContext.Authorisation.IsUserAuthorised(DynamicModel.FlowContext.Flow.Permission))
            {
                RaiseAuthorisationAlert(DynamicModel.FlowContext.Flow.Name);
            }

            isReadOnly = DynamicModel.FlowContext.IsActiveStateReadOnly();
        }

        public virtual async Task FlowExecutionAsync(FlowExecutionArgs flowExecutionArgs)
        {
            if (flowExecutionArgs.FlowAction.Equals(FlowActionEnum.Complete)
                || !CurrentEditContext.Validate())
            {
                return;
            }

            isSaveInProgress = true;

            DynamicModel.FlowContext.FlowExecutionArgs = flowExecutionArgs;

            var response = await DynamicApiRequest
                .FlowExecutionAsync<T>(DynamicModel)
                .ConfigureAwait(false);

            GetResponse(response);

            isSaveInProgress = false;

            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }
    }
}
