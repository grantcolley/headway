using Microsoft.AspNetCore.Components;

namespace Headway.Blazor.Controls.Components
{
    public partial class Status
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public string Class { get; set; }
    }
}
