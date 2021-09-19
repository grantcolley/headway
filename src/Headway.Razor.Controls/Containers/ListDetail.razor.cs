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
    public abstract class ListDetailBase<T> : GenericComponentBase<T>
    {
        [Inject]
        public IDynamicService DynamicService { get; set; }

        [Parameter]
        public DynamicField Field { get; set; }

        [Parameter]
        public List<DynamicArg> ComponentArgs { get; set; }

        [Parameter]
        public Config Config { get; set; }

        protected DynamicModel<T> dynamicModel;

        protected List<T> list;

        protected override async Task OnInitializedAsync()
        {
            list = (List<T>)Field.PropertyInfo.GetValue(Field.Model, null);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }
    }
}
