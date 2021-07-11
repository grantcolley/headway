using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class ListConfig
    {
        public ListConfig()
        {
            ListItemConfigs = new List<ListItemConfig>();
        }

        public int ListConfigId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ConfigPath { get; set; }
        public string NavigateTo { get; set; }
        public string NavigationPropertyName { get; set; }
        public List<ListItemConfig> ListItemConfigs { get; set; }
    }
}
