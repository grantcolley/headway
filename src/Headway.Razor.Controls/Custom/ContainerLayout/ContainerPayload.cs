using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Custom.ContainerLayout
{
    public class ContainerPayload
    {
        public ConfigContainer DragTarget { get; set; }
        public ConfigContainer DragSourceContainer { get; set; }
        public List<ConfigContainer> DragSourceContainers { get; set; }
    }
}
