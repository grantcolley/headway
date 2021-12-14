using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Custom.ContainerLayout
{
    public class Payload
    {
        public ConfigContainer ConfigContainer { get; set; }
        public ConfigContainer ParentConfigContainer { get; set; }
        public List<ConfigContainer> RootContainers { get; set; }
    }
}
