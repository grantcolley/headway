using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class ListDetailBase<T> : GenericComponentBase<T> where T : class, new()
    {
        [Parameter]
        public DynamicField Field { get; set; }

        [Parameter]
        public List<DynamicArg> ComponentArgs { get; set; }

        [Parameter]
        public Config Config { get; set; }

        protected DynamicModel<T> dynamicModel;

        protected DynamicList<T> dynamicList;

        protected override async Task OnInitializedAsync()
        {
            await NewAsync().ConfigureAwait(false);

            var list = (List<T>)Field.PropertyInfo.GetValue(Field.Model, null);

            dynamicList = await GetDynamicListAsync(list, "ConfigItems").ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task NewAsync()
        {
            dynamicModel = await CreateDynamicModelAsync(Config.Name).ConfigureAwait(false);
        }

        protected async Task EditAsync(T model)
        {
            dynamicModel = await GetDynamicModelAsync(model, Config.Name).ConfigureAwait(false);
        }

        protected async Task AddAsync()
        {
        }

        protected async Task RemoveAsync()
        {
        }
    }
}
