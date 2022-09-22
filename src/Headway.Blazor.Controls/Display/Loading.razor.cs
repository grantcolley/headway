using Microsoft.AspNetCore.Components;

namespace Headway.Blazor.Controls.Display
{
    public partial class Loading : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Message { get; set; }
    }
}
