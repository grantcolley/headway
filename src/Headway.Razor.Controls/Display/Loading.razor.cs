using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Display
{
    public partial class Loading : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Message { get; set; }
    }
}
