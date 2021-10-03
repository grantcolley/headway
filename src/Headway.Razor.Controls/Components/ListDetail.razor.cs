using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
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

        private List<T> list;

        protected override async Task OnInitializedAsync()
        {
            await NewAsync().ConfigureAwait(false);

            list = (List<T>)Field.PropertyInfo.GetValue(Field.Model, null);

            dynamicList = await GetDynamicListAsync(list, "ConfigItems").ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task NewAsync()
        {
            dynamicModel = await CreateDynamicModelAsync(Config.Name).ConfigureAwait(false);
        }

        protected async Task EditAsync(DynamicListItem<T> listItem)
        {
            dynamicModel = await GetDynamicModelAsync(listItem.Model, Config.Name).ConfigureAwait(false);
        }

        protected void Add(DynamicModel<T> model)
        {
        }

        protected async Task RemoveAsync(DynamicModel<T> model)
        {
            var listItem = dynamicList.DynamicListItems.FirstOrDefault(i => i.Model.Equals(model.Model));
            
            if(listItem == null)
            {
                return;
            }

            list.Remove(listItem.Model);

            dynamicList.Remove(listItem);

            await NewAsync().ConfigureAwait(false);
        }
    }
}
