using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class ModelConfig
    {
        public ModelConfig()
        {
            FieldConfigs = new List<FieldConfig>();
        }

        public int ModelConfigId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string ConfigApi { get; set; }
        public string NavigateText { get; set; }
        public string NavigateTo { get; set; }
        public List<FieldConfig> FieldConfigs { get; set; }
    }
}
