using Headway.Core.Attributes;
using System.Collections.Generic;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class Config
    {
        public Config()
        {
            ConfigItems = new List<ConfigItem>();
        }

        public int ConfigId { get; set; }
        public ConfigType ConfigType { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Model { get; set; }
        public string ModelApi { get; set; }
        public string Component { get; set; }
        public string NavigateTo { get; set; }
        public string NavigateToProperty { get; set; }
        public string NavigateToConfig { get; set; }
        public string NavigateBack { get; set; }
        public string NavigateBackProperty { get; set; }
        public string NavigateBackConfig { get; set; }
        public List<ConfigItem> ConfigItems { get; set; }
    }
}
