using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Headway.Blazor.Controls.Flow.Containers
{
    public abstract class FlowDivColBase : ComponentBase
    {
        [Parameter]
        public DynamicField Field { get; set; }
    }
}
