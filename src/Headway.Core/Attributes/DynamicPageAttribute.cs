using Headway.Core.Enums;
using System;

namespace Headway.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DynamicPageAttribute : Attribute
    {
        public DynamicPageAttribute(PageType pageType)
        {
            this.PageType = pageType;
        }

        public PageType PageType { get; set; }
    }
}