using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Base
{
    public abstract class DynamicComponentBase<T> : HeadwayComponentBase
    {
        [Parameter]
        public DynamicModel<T> DynamicModel { get; set; }
    }
}
