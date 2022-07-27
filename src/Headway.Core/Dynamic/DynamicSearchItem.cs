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
        public string ParameterName { get; set; }
        public string Tooltip { get; set; }
        public string ComponentArgs { get; set; }
        public string Value { get; set; }
        public string SearchComponentUniqueId { get; set; }
        public Type SearchComponent { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public DynamicSearchItem LinkSource { get; set; }
        public bool IsLinkedSearchItem { get { return LinkSource != null; } }
        public bool HasLinkDependents { get; set; }

        public string LinkValue
        {
            get
            {
                if (IsLinkedSearchItem)
                {
                    return LinkSource.Value;
                }

                return null;
            }
        }
    }
}
