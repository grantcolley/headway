using System;
using System.Collections.Generic;

namespace Headway.Core.Dynamic
{
    public class DynamicSearchItem
    {
        public DynamicSearchItem()
        {
            Parameters = new Dictionary<string, object>();
        }

        public int Order { get; set; }
        public string Label { get; set; }
        public string Tooltip { get; set; }
        public Type SearchComponent { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
