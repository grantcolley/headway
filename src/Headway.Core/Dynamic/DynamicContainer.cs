using System.Collections.Generic;

namespace Headway.Core.Dynamic
{
    public class DynamicContainer
    {
        public int Row { get; set; }
        public int Column {  get; set; }
        public string DynamicContainerTypeName { get; set; }
        public Type DynamicComponent { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public List<DynamicField> DynamicFields {  get; set; }
        public List<DynamicContainer> DynamicContainers {  get; set; }
    }
}
