using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class ConfigContainer
    {
        public int ConfigContainerId { get; set; }
        public string Container { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public List<ConfigContainer> ConfigContainers { get; set; }
    }
}
