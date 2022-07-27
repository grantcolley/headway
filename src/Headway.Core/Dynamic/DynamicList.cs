using Headway.Core.Constants;
using Headway.Core.Extensions;
using Headway.Core.Helpers;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Dynamic
{
    public class DynamicList<T> where T : class, new()
    {
        private readonly List<DynamicListItem<T>> dynamicListItems;

        public DynamicList(IEnumerable<T> listItems, Config config)
        {
            SearchParameters = new Dictionary<string, object>();

            TypeHelper = DynamicTypeHelper.Get<T>();

            Config = config;

            if(!string.IsNullOrWhiteSpace(Config.SearchComponent))
            {
                DynamicSearchComponent = Type.GetType(Config.SearchComponent);

                var dynamicSearchItems = new List<DynamicSearchItem>();

                foreach(var configSearchItems in Config.ConfigSearchItems)
                {
                    var dynamicSearchItem = new DynamicSearchItem
                    {
                        Order = configSearchItems.Order,
                        Label = configSearchItems.Label,
                        Tooltip = configSearchItems.Tooltip,
                        ComponentArgs = configSearchItems.ComponentArgs,
                        SearchComponent = Type.GetType(configSearchItems.Component)
                    };

                    dynamicSearchItems.Add(dynamicSearchItem);
                }

                dynamicSearchItems.AddSearchArgs();

                SearchParameters.Add(Parameters.SEARCH_ITEMS, dynamicSearchItems);
            }

            dynamicListItems = listItems.Select(i => new DynamicListItem<T>(i)).ToList();
        }

        public DynamicTypeHelper<T> TypeHelper { get; private set; }

        public Config Config { get; private set; }

        public Type DynamicSearchComponent { get; private set; }

        public Dictionary<string, object> SearchParameters { get; private set; }

        public string Title { get { return Config.Title; } }

        public List<ConfigItem> ConfigItems
        {
            get
            {
                if (Config == null)
                {
                    return new List<ConfigItem>();
                }

                return Config.ConfigItems.OrderBy(c => c.Order).ToList();
            }
        }

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
