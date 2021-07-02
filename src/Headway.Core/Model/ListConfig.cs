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
        public string ListName { get; set; }
        public string ConfigPath { get; set; }
        public string IdPropertyName { get; set; }
        public List<ListItemConfig> ListItemConfigs { get; set; }
    }
}
