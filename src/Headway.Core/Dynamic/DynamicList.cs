using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Dynamic
{
    public class DynamicList<T> where T : class, new()
    {
        private readonly List<T> listItems;

        public DynamicList(IEnumerable<T> listItems, Config config)
        {
            this.listItems = new List<T>(listItems);
            Config = config;

            Helper = DynamicTypeHelper.Get<T>();

            BuildDynamicListItems();
        }

        public DynamicTypeHelper<T> Helper { get; private set; }

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
            return Helper.GetValue(listItem, field);
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
