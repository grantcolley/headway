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
        public List<ConfigContainer> Source { get; set; }

        protected void HandleDragStart(ConfigContainer configContainer)
        {
            var payLoad = new Payload 
            {
                DragTarget = configContainer,
                DragSource = Source
            };

            DragDropController.Payload = payLoad;
        }
    }
}
