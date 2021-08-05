using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class LayoutConfigItem
    {
        public int LayoutConfigItemId { get; set; }
        public string Name { get; set; }
        public string Component { get; set; } 
        public int ContainerRowPosition { get; set; }
        public int ContainerColumnPosition { get; set; }
        public List<LayoutConfigItem> Containers { get; set; }             
    }
}
