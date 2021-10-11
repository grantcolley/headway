using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Base
{
    public abstract class DynamicContainerBase<T> : HeadwayComponentBase where T : class, new()
    {
        [Parameter]
        public DynamicModel<T> DynamicModel { get; set; }
    }
}
