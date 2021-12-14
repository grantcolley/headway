using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Custom.ContainerLayout
{
    public class RootContainerBase : ComponentBase
    {
        [Parameter]
        public List<ConfigContainer> ConfigContainers { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public Payload Payload { get; set; }
    }
}
