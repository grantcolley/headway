using Headway.Core.Args;
using Headway.Core.Interface;
using System;
using System.Collections.Generic;

namespace Headway.Core.Dynamic
{
    public class DynamicContainer : IComponentTree, IComponent
    {
        public DynamicContainer()
        {
            UniqueId = Guid.NewGuid().ToString();
            Parameters = new Dictionary<string, object>();
            DynamicFields = new List<DynamicField>();
            DynamicContainers = new List<DynamicContainer>();
            DynamicArgs = new List<DynamicArg>();
        }

        public string UniqueId { get; private set; }
        public int ContainerId { get; set; }
        public string DynamicContainerTypeName { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public string ParentCode { get; set; }
        public string ComponentArgs { get; set; }
        public string FlowArgs { get; set; }
        public List<DynamicArg> DynamicArgs {get;set;}
        public Type DynamicComponent { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public List<DynamicField> DynamicFields {  get; set; }
        public List<DynamicContainer> DynamicContainers {  get; set; }
    }
}
