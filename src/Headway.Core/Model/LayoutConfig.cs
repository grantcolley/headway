using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class LayoutConfig
    {
        public int LayoutConfigId { get; set; }
        public string Name { get; set; }
        public List<LayoutConfigItem> LayoutConfigItems { get; set; }
    }
}