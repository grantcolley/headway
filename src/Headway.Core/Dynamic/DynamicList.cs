using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Dynamic
{
    public class DynamicList<T>
    {
        private readonly DynamicTypeHelper<T> typeHelper;
        private readonly IEnumerable<T> listItems;

        public DynamicList(IEnumerable<T> listItems, Config config)
        {
            this.listItems = listItems;
            Config = config;

            typeHelper = DynamicTypeHelper.Get<T>();

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
            return typeHelper.GetValue(listItem, field);
        }

        private void BuildDynamicListItems()
        {
            DynamicListItems = new List<DynamicListItem<T>>();

            var dynamicListItems = listItems.Select(i => new DynamicListItem<T>(i));

            DynamicListItems.AddRange(dynamicListItems);
        }
    }
}
