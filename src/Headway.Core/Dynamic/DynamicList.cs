using Headway.Core.Helpers;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Headway.Core.Dynamic
{
    public class DynamicList<T> where T : class, new()
    {
        private readonly Dictionary<string, PropertyInfo> properties = new();
        private readonly List<T> listItems;

        public DynamicList(List<T> listItems, Config config)
        {
            this.listItems = listItems;
            Config = config;

            var t = typeof(T);
            var propertyInfos = PropertyInfoHelper.GetPropertyInfos(t);

            foreach (var propertyInfo in propertyInfos)
            {
                properties.Add(propertyInfo.Name, propertyInfo);
            }

            BuildDynamicListItems();
        }

        public Config Config { get; private set; }

        public List<ConfigItem> ConfigItems 
        {
            get
            {
                if(Config == null)
                {
                    return new List<ConfigItem>();
                }

                return Config.ConfigItems.OrderBy(c => c.Order).ToList();
            }
        }

        public string Title { get { return Config.Title; } }

        public List<DynamicListItem<T>> DynamicListItems { get; private set; }

        public object GetValue(T listItem, string field)
        {
            return properties[field].GetValue(listItem);
        }

        public void Add(T item)
        {
            if (item == null)
            {
                return;
            }

            listItems.Add(item);
            DynamicListItems.Add(new DynamicListItem<T>(item));
        }

        public void Remove(DynamicListItem<T> dynamicListItem)
        {
            if (dynamicListItem == null)
            {
                return;
            }

            listItems.Remove(dynamicListItem.Model);
            DynamicListItems.Remove(dynamicListItem);
        }

        private void BuildDynamicListItems()
        {
            DynamicListItems = new List<DynamicListItem<T>>();

            var dynamicListItems = listItems.Select(i => new DynamicListItem<T>(i));

            DynamicListItems.AddRange(dynamicListItems);
        }
    }
}
