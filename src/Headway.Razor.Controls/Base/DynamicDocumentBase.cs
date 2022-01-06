using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Razor.Controls.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Base
{
    public abstract class DynamicDocumentBase<T> : HeadwayComponentBase where T : class, new()
    {
        [Inject]
        protected IDynamicService DynamicService { get; set; }

        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public int? Id { get; set; }

        protected DynamicModel<T> dynamicModel { get; set; }
        protected DynamicList<T> dynamicList;
        protected Status Status { get; set; }
        protected Alert Alert { get; set; }
        protected bool isSaveInProgress = false;
        protected bool isDeleteInProgress = false;

        protected virtual async Task InitializeDynamicModelAsync()
        {
            IServiceResult<DynamicModel<T>> serviceResult;

            if (!Id.HasValue
                || Id.Value.Equals(0))
            {
                serviceResult = await DynamicService
                    .CreateDynamicModelInstanceAsync<T>(Config)
                    .ConfigureAwait(false);
            }
            else
            {
                serviceResult = await DynamicService
                    .GetDynamicModelAsync<T>(Id.Value, Config)
                    .ConfigureAwait(false);
            }

            dynamicModel = GetResponse(serviceResult);
        }

        protected virtual async Task InitializeDynamicListAsync()
        {
            var result =
                await DynamicService.GetDynamicListAsync<T>(Config)
                .ConfigureAwait(false);

            dynamicList = GetResponse(result);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }
    }
}
