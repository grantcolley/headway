using Headway.Core.Attributes;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Headway.Core.Helpers
{
    public static class ModelHelper
    {
        public static IEnumerable<BrowserStorageItem> GetBrowserStorageItems()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var browserStorageItems = (from t in assembly.GetTypes()
                                      let attributes = t.GetCustomAttributes(typeof(DynamicModelAttribute), true)
                                       where attributes != null && attributes.Length > 0
                                       select new BrowserStorageItem
                                       {
                                           Key = t.Name,
                                           Value = $"{t.FullName}, {assembly.GetName().Name}"
                                       }).ToList();
            return browserStorageItems;
        }
    }
}
