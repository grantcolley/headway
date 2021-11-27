using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Containers
{
    public partial class DivModelContainer : ComponentBase
    {
        [Parameter]
        public DynamicContainer Container { get; set; }
    }
}
