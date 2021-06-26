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
        public string ModelName { get; set; }
        public string ConfigApiPath { get; set; }
        public string RedirectText { get; set; }
        public string RedirectPage { get; set; }
        public List<FieldConfig> FieldConfigs { get; set; }
    }
}
