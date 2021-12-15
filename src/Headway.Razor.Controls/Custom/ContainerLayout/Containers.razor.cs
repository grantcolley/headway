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
        public string Label { get; set; }

        protected string dropClass = "";

        protected void HandleDragEnter()
        {
            if(DragDropController.Payload == null)
            {
                dropClass = "";
                return;
            }

            if (ConfigContainers.Any(c => c.Label.Equals(DragDropController.Payload.DragTarget))
                || (!string.IsNullOrWhiteSpace(Label) && Label.Equals(DragDropController.Payload.DragTarget)))
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
                && !ConfigContainers.Equals(DragDropController.Payload.DragSource))
            {
                ConfigContainers.Add(DragDropController.Payload.DragTarget);
                DragDropController.Payload.DragSource.Remove(DragDropController.Payload.DragTarget);
            }

            dropClass = "";
            DragDropController.Payload = null;
        }
    }
}
