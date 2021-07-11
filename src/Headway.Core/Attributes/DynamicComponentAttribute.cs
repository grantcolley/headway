using Headway.Core.Enums;
using System;

namespace Headway.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DynamicComponentAttribute : Attribute
    {
        public DynamicComponentAttribute(ComponentType ComponentType)
        {
            this.ComponentType = ComponentType;
        }

        public ComponentType ComponentType { get; set; }
    }
}