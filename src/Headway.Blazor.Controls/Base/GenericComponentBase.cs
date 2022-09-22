using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Base
{
    public abstract class GenericComponentBase<T> : DynamicComponentBase where T : class, new()
    {
        [Inject]
        public IDynamicApiRequest DynamicApiRequest { get; set; }

        [Parameter]
        public Config Config { get; set; }

        protected async Task<DynamicList<T>> GetDynamicListAsync(IEnumerable<T> list, string config)
        {
            var result = await DynamicApiRequest.GetDynamicListAsync<T>(list, config)
                .ConfigureAwait(false);

            return GetResponse(result);
        }

        protected async Task<DynamicModel<T>> GetDynamicModelAsync(T model, string config)
        {
            var result = await DynamicApiRequest.GetDynamicModelAsync<T>(model, config)
                .ConfigureAwait(false);

            return GetResponse(result);
        }

        protected async Task<DynamicModel<T>> CreateDynamicModelAsync(string config)
        {
            var result = await DynamicApiRequest.CreateDynamicModelInstanceAsync<T>(config)
                .ConfigureAwait(false);

            return GetResponse(result);
        }
    }
}
