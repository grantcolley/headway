using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Headway.Blazor.Controls.Containers
{
    public abstract class DivColBase : ComponentBase
    {
        [Parameter]
        public DynamicField Field { get; set; }
    }
}
