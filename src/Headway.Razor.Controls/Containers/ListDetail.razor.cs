using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
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

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
        }
    }
}
