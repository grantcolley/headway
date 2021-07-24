using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Headway.Core.Dynamic
{
    public class DynamicField
    {
        public object Model { get; set; }
        public int Order { get; set; }
        public string Label { get; set; }
        public string PropertyName { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public MemberExpression MemberExpression { get; set; }
        public string DynamicComponentTypeName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public Type DynamicComponent { get; set; }
    }
}
