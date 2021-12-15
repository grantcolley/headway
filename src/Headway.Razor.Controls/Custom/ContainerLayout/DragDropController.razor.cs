using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Custom.ContainerLayout
{
    public class DragDropControllerBase : ComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public Payload Payload { get; set; }
    }
}
