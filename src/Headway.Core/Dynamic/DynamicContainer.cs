using System;
using System.Collections.Generic;

namespace Headway.Core.Dynamic
{
    public class DynamicContainer
    {
        public DynamicContainer()
        {
            DynamicFields = new List<DynamicField>();
            DynamicContainers = new List<DynamicContainer>();
        }

        public int ContainerId { get; set; }
        public int Row { get; set; }
        public int Column {  get; set; }
        public string DynamicContainerTypeName { get; set; }
        public Type DynamicComponent { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public List<DynamicField> DynamicFields {  get; set; }
        public List<DynamicContainer> DynamicContainers {  get; set; }
    }
}
