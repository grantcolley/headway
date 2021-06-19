using System.Collections.Generic;

namespace Headway.Core.Dynamic
{
    public class DynamicModelConfig
    {
        public string Name { get; set; }
        public List<DynamicFieldConfig> FieldConfigs { get; set; }
    }
}
