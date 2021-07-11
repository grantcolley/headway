using System;

namespace Headway.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DynamicApiControllerAttribute : Attribute
    {
    }
}
