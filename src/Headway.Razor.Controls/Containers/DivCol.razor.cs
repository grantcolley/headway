using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Containers
{
    public abstract class DivColBase : ComponentBase
    {
        [Parameter]
        public DynamicField Field { get; set; }
    }
}
