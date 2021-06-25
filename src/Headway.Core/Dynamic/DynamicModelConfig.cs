using System.Collections.Generic;

namespace Headway.Core.Dynamic
{
    public class DynamicModelConfig
    {
        public DynamicModelConfig()
        {
            FieldConfigs = new List<DynamicFieldConfig>();
        }

        public string ConfigName { get; set; }
        public string ConfigPath { get; set; }
        public string RedirectText { get; set; }
        public string RedirectPage { get; set; }
        public List<DynamicFieldConfig> FieldConfigs { get; set; }
    }
}
