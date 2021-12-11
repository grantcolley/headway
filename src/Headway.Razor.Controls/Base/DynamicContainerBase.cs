using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Base
{
    public class DynamicContainerBase : ComponentBase
    {
        [Parameter]
        public DynamicContainer Container { get; set; }
    }
}
