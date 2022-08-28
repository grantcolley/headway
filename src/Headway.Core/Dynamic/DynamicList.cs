using Headway.Core.Args;
using Headway.Core.Constants;
using Headway.Core.Extensions;
using Headway.Core.Helpers;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Headway.Core.Dynamic
{
    public class DynamicList<T> where T : class, new()
    {
        private readonly List<DynamicListItem<T>> dynamicListItems;
        private readonly List<DynamicSearchItem> dynamicSearchItems;

        public DynamicList(IEnumerable<T> listItems, Config config)
        {
            UniqueId = Guid.NewGuid().ToString();

            SearchParameters = new Dictionary<string, object>();
            dynamicSearchItems = new List<DynamicSearchItem>();

            TypeHelper = DynamicTypeHelper.Get<T>();

            Config = config;

            SearchComponentUniqueId = Guid.NewGuid().ToString();

            if (!string.IsNullOrWhiteSpace(Config.SearchComponent))
            {
                DynamicSearchComponent = Type.GetType(Config.SearchComponent);

                foreach(var configSearchItems in Config.ConfigSearchItems)
                {
                    var dynamicSearchItem = new DynamicSearchItem
                    {
                        Order = configSearchItems.Order,
                        Label = configSearchItems.Label,
                        ParameterName = configSearchItems.ParameterName,
                        Tooltip = configSearchItems.Tooltip,
                        ComponentArgs = configSearchItems.ComponentArgs,
                        SearchComponentUniqueId = SearchComponentUniqueId,
                        SearchComponent = Type.GetType(configSearchItems.Component)
                    };

                    dynamicSearchItem.Parameters.Add(Parameters.SEARCH_ITEM, dynamicSearchItem);
                    dynamicSearchItems.Add(dynamicSearchItem);
                }

                dynamicSearchItems.AddSearchArgs();

                SearchParameters.Add(Parameters.SEARCH_COMPONENT_UNIQUE_ID, SearchComponentUniqueId);
                SearchParameters.Add(Parameters.SEARCH_ITEMS, dynamicSearchItems);
            }

            dynamicListItems = listItems.Select(i => new DynamicListItem<T>(i)).ToList();
        }

        public string UniqueId { get; private set; }

        public DynamicTypeHelper<T> TypeHelper { get; private set; }

        public Config Config { get; private set; }

        public string SearchComponentUniqueId { get; set; }

        public Type DynamicSearchComponent { get; private set; }

        public Dictionary<string, object> SearchParameters { get; private set; }

        public string Title { get { return Config.Title; } }

        public bool UseSearchCriteria { get { return Config.UseSearchComponent; } }

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

        public SearchArgs SearchArgs
        {
            get
            {
                return new SearchArgs
                {
                    SourceConfig = Config.Name,
                    Args = dynamicSearchItems.Select(
                        si => new SearchArg
                        {
                            Label = si.Label,
                            ParameterName = si.ParameterName,
                            Value = si.Value
                        }).ToList()
                };
            }
        }

        public DataArgs ToDataArgs(T listItem)
        {
            return new DataArgs
            {
                SourceConfig = Config.Name,
                Args = Config.ConfigItems.Select(
                    ci => new DataArg
                    {
                        PropertyName = ci.PropertyName,
                        Value = TypeHelper.GetValue(listItem, ci.PropertyName)
                    }).ToList()
            };
        }

        public string ToDataArgsJson(T listItem)
        {
            var dataArgs = ToDataArgs(listItem);
            return JsonSerializer.Serialize(dataArgs);
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

        public void RePopulateList(IEnumerable<T> listItems)
        {
            dynamicListItems.Clear();
            dynamicListItems.AddRange(
                listItems.Select(i => new DynamicListItem<T>(i)
                ).ToList());
        }
    }
}
