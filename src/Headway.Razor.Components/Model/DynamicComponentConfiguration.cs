using System;
using System.Collections.Generic;

namespace Headway.Razor.Components.Model
{
    public class DynamicComponentConfiguration
    {
        public DynamicComponentConfiguration()
        {
            Parameters = new Dictionary<string, object>();
        }

        public Type ComponentType { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
