using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Headway.Blazor.Controls.Flow.Containers
{
    public class FlowListItemContainerBase : ComponentBase
    {
        [Parameter]
        public DynamicContainer Container { get; set; }
    }
}
