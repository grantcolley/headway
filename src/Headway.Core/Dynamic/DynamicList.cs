using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Dynamic
{
    public class DynamicList<T>
    {
        private readonly DynamicTypeHelper<T> typeHelper;
        private readonly IEnumerable<T> listItems;

        public DynamicList(IEnumerable<T> listItems, ListConfig listConfig)
        {
            this.listItems = listItems;
            ListConfig = listConfig;

            typeHelper = DynamicTypeHelper.Get<T>();

            BuildDynamicListItems();
        }

        public ListConfig ListConfig { get; private set; }

        public string Title { get { return ListConfig.ListName; } }

        public List<DynamicListItem> DynamicListItems { get; private set; }

        private void BuildDynamicListItems()
        {
            DynamicListItems = new List<DynamicListItem>();

            var dynamicListItems = listItems.Select(i => new DynamicListItem { Model = i });

            DynamicListItems.AddRange(dynamicListItems);
        }
    }
}
