using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Containers
{
    [DynamicContainer]
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
            dynamicModel = await CreateDynamicModelAsync(Config.Name);

            var list = (List<T>)Field.PropertyInfo.GetValue(Field.Model, null);
                     
            dynamicList = await GetDynamicListAsync(list, "ConfigItems").ConfigureAwait(false);



            // https://softwareengineering.stackexchange.com/questions/287980/generic-sorting-of-lists
            // get the dynamicModel for ConfigItem then get the propertyInfo for the Order field
            // List<Order> SortedList = aList.OrderBy(o=>prop.GetValue(o)).ToList();

            // Consider re-introducing the TypeHelper which will be faster than reflection on big lists. 

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void Add()
        {
        }

        protected void Update(object id)
        {
        }
    }
}
