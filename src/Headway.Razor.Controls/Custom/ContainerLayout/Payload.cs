using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Custom.ContainerLayout
{
    public class Payload
    {
        public ConfigContainer DragTarget { get; set; }
        public List<ConfigContainer> DragSource { get; set; }
    }
}
