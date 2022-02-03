using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Headway.Core.Dynamic
{
    public class DynamicField
    {
        public DynamicField()
        {
            Parameters = new Dictionary<string, object>();
            LinkDependents = new List<DynamicField>();
        }

        public object Model { get; set; }
        public int Order { get; set; }
        public string Label { get; set; }
        public string Tooltip { get; set; }
        public string ConfigName { get; set; }
        public string PropertyName { get; set; }
        public string ComponentArgs { get; set; }
        public string DynamicComponentTypeName { get; set; }
        public int ConfigContainerId { get; set; }
        public string ContainerUniqueId { get; set; }
        public int ValidationMessagesCount { get; set; }
        public DynamicField LinkSource { get; set; }
        public List<DynamicField> LinkDependents { get; set; }
        public Type DynamicComponent { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public MemberExpression MemberExpression { get; set; }
        public bool IsLinkedField { get { return LinkSource != null; } }
        public bool HasLinkDependents { get { return LinkDependents.Any(); } }

        public object LinkValue
        {
            get
            {
                if (IsLinkedField)
                {
                    return LinkSource.PropertyInfo.GetValue(LinkSource.Model);
                }

                return null;
            }
        }
    }
}
