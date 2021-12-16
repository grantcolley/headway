using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Razor.Controls.Custom.ContainerLayout
{
    public class ContainersBase : ComponentBase 
    {
        [CascadingParameter]
        DragDropController DragDropController { get; set; }

        [Parameter]
        public List<ConfigContainer> ConfigContainers { get; set; }

        [Parameter]
        public ConfigContainer ConfigContainer { get; set; }

        protected string dropClass = "";

        protected void HandleDragEnter()
        {
            if(DragDropController.Payload == null)
            {
                dropClass = "";
                return;
            }

            if (ConfigContainers.Any(c => c.Label.Equals(DragDropController.Payload.DragTarget))
                || (ConfigContainer != null && ConfigContainer.Label.Equals(DragDropController.Payload.DragTarget.Label)))
            {
                dropClass = "no-drop";
            }
            else
            {
                dropClass = "can-drop";
            }
        }

        protected void HandleDragLeave()
        {
            dropClass = "";
        }

        protected void HandleDrop()
        {
            if (DragDropController.Payload == null)
            {
                dropClass = "";
                return;
            }

            if (dropClass.Equals("can-drop")
                && !ConfigContainers.Equals(DragDropController.Payload.DragSourceContainers)
                && (ConfigContainer == null
                || DragDropController.Payload.DragSourceContainer == null
                || !ConfigContainer.Label.Equals(DragDropController.Payload.DragSourceContainer.Label)))
            {
                ConfigContainers.Add(DragDropController.Payload.DragTarget);
                DragDropController.Payload.DragSourceContainers.Remove(DragDropController.Payload.DragTarget);
            }

            dropClass = "";
            DragDropController.Payload = null;
        }
    }
}
