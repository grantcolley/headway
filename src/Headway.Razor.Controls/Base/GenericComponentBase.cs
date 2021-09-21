using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Base
{
    public abstract class GenericComponentBase<T> : HeadwayComponentBase where T : class, new()
    {
        [Inject]
        public IDynamicService DynamicService { get; set; }

        protected async Task<DynamicList<T>> GetDynamicList(IEnumerable<T> list, string config)
        {
            var result = await DynamicService.GetDynamicListAsync<T>(list, config)
                .ConfigureAwait(false);

            return GetResponse(result);
        }
    }
}
