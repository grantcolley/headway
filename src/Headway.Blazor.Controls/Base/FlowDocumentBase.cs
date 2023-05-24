﻿using Headway.Core.Dynamic;
using Headway.Core.Interface;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Base
{
    public class FlowDocumentBase<T> : DynamicDocumentBase<T> where T : class, new ()
    {
        protected virtual async Task FlowExecuteAsync()
        {
            isSaveInProgress = true;

            if (CurrentEditContext != null
                && CurrentEditContext.Validate())
            {
                IResponse<DynamicModel<T>> response;

                if (DynamicModel.Id.Equals(0))
                {
                    response = null;
                    // TODO: execute flow action on new model i.e. model Id is 0
                    response = await DynamicApiRequest
                        .AddDynamicModelAsync<T>(DynamicModel)
                        .ConfigureAwait(false);
                }
                else
                {
                    response = null;
                    // TODO: execute flow action on existing model
                    response = await DynamicApiRequest
                        .UpdateDynamicModelAsync<T>(DynamicModel)
                        .ConfigureAwait(false);
                }

                GetResponse(response);
            }

            isSaveInProgress = false;
        }
    }
}
