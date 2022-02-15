using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Base
{
    public abstract class GenericComponentBase<T> : HeadwayComponentBase where T : class, new()
    {
        [Inject]
        public IDynamicApiRequest DynamicService { get; set; }

        [CascadingParameter]
        public EditContext CurrentEditContext { get; set; }

        [Parameter]
        public DynamicField Field { get; set; }

        [Parameter]
        public List<DynamicArg> ComponentArgs { get; set; }

        [Parameter]
        public Config Config { get; set; }

        protected async Task<DynamicList<T>> GetDynamicListAsync(IEnumerable<T> list, string config)
        {
            var result = await DynamicService.GetDynamicListAsync<T>(list, config)
                .ConfigureAwait(false);

            return GetResponse(result);
        }

        protected async Task<DynamicModel<T>> GetDynamicModelAsync(T model, string config)
        {
            var result = await DynamicService.GetDynamicModelAsync<T>(model, config)
                .ConfigureAwait(false);

            return GetResponse(result);
        }

        protected async Task<DynamicModel<T>> CreateDynamicModelAsync(string config)
        {
            var result = await DynamicService.CreateDynamicModelInstanceAsync<T>(config)
                .ConfigureAwait(false);

            return GetResponse(result);
        }
    }
}
