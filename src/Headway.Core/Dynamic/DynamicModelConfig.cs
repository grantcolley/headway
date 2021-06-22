using System.Collections.Generic;

namespace Headway.Core.Dynamic
{
    public class DynamicModelConfig
    {
        public string ConfigName { get; set; }
        public string Title { get; set; }
        public string RedirectText { get; set; }
        public string RedirectPage { get; set; }
        public List<DynamicFieldConfig> FieldConfigs { get; set; }
    }
}
