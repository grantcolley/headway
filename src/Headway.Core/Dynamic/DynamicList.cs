using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Dynamic
{
    public class DynamicList<T> where T : class, new()
    {
        private readonly List<DynamicListItem<T>> dynamicListItems;

        public DynamicList(IEnumerable<T> listItems, Config config)
        {
            Config = config;
            TypeHelper = DynamicTypeHelper.Get<T>();

            dynamicListItems = listItems.Select(i => new DynamicListItem<T>(i)).ToList();
        }

        public DynamicTypeHelper<T> TypeHelper { get; private set; }

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

        public List<DynamicListItem<T>> DynamicListItems 
        { 
            get
            {
                if(string.IsNullOrWhiteSpace(Config.OrderModelBy))
                {
                    return dynamicListItems;
                }

                return dynamicListItems.OrderBy(
                    i => TypeHelper.GetValue(i.Model, Config.OrderModelBy))
                    .ToList();
            }
        }

        public object GetValue(T listItem, string field)
        {
            return TypeHelper.GetValue(listItem, field);
        }

        public void Add(T item)
        {
            if (item == null)
            {
                return;
            }

            dynamicListItems.Add(new DynamicListItem<T>(item));
        }

        public void Remove(DynamicListItem<T> dynamicListItem)
        {
            if (dynamicListItem == null)
            {
                return;
            }

            dynamicListItems.Remove(dynamicListItem);
        }
    }
}
