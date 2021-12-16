using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Custom.ContainerLayout
{
    public class DragDropControllerBase : ComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public ContainerPayload Payload { get; set; }
    }
}
