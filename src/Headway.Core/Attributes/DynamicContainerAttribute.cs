using Headway.Core.Enums;
using System;

namespace Headway.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DynamicContainerAttribute : Attribute
    {
        public DynamicContainerAttribute(ContainerType ContainerType)
        {
            this.ContainerType = ContainerType;
        }

        public ContainerType ContainerType { get; set; }
    }
}