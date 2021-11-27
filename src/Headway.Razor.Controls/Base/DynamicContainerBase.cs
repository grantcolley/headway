using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Base
{
    public abstract class DynamicContainerBase<T> : HeadwayComponentBase where T : class, new()
    {
        [Inject]
        protected IDynamicService DynamicService { get; set; }

        [Parameter]
        public DynamicContainer Container { get; set; }

        [Parameter]
        public DynamicModel<T> DynamicModel { get; set; }

        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public int? Id { get; set; }
    }
}
