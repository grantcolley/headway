using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class LayoutConfig
    {
        public LayoutConfig()
        {
            LayoutConfigItems = new List<LayoutConfigItem>();
        }

        public int LayoutConfigId { get; set; }
        public string Name { get; set; }
        public List<LayoutConfigItem> LayoutConfigItems { get; set; }
    }
}