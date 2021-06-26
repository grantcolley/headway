using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class ModelConfig
    {
        public ModelConfig()
        {
            FieldConfigs = new List<FieldConfig>();
        }

        public string ConfigName { get; set; }
        public string ConfigPath { get; set; }
        public string RedirectText { get; set; }
        public string RedirectPage { get; set; }
        public List<FieldConfig> FieldConfigs { get; set; }
    }
}
