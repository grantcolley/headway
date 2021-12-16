using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Custom.ContainerLayout
{
    public partial class ContainerBase : ComponentBase
    {
        [CascadingParameter]
        DragDropController DragDropController { get; set; }

        [Parameter]
        public ConfigContainer ConfigContainer { get; set; }

        [Parameter]
        public ConfigContainer SourceContainer { get; set; }

        [Parameter]
        public List<ConfigContainer> SourceContainers { get; set; }

        protected void HandleDragStart(ConfigContainer configContainer)
        {
            if (DragDropController.Payload == null)
            {
                DragDropController.Payload = new ContainerPayload
                {
                    DragTarget = configContainer,
                    DragSourceContainer = SourceContainer,
                    DragSourceContainers = SourceContainers,
                };
            }
        }
    }
}
