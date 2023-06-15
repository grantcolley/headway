using Headway.Core.Extensions;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Base
{
    public class FlowDocumentBase<T> : DynamicDocumentBase<T> where T : class, new ()
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await InitializeDynamicModelAsync().ConfigureAwait(false);

            if(!DynamicModel.FlowContext.Authorisation.IsUserAuthorised(DynamicModel.FlowContext.Flow.Permission))
            {
                RaiseAuthorisationAlert(DynamicModel.FlowContext.Flow.Name);
            }
        }

        public virtual async Task FlowExecutionAsync()
        {
            isSaveInProgress = true;

            if (CurrentEditContext != null
                && CurrentEditContext.Validate())
            {
                var response = await DynamicApiRequest
                    .FlowExecutionAsync<T>(DynamicModel)
                    .ConfigureAwait(false);

                GetResponse(response);
            }

            isSaveInProgress = false;
        }
    }
}
