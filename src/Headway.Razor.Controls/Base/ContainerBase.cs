using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Base
{
    public abstract class ContainerBase<T> : ComponentBase
    {
        [Parameter]
        public DynamicModel<T> DynamicModel { get; set; }
    }
}
