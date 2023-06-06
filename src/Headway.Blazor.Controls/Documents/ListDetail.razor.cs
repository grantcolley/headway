using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Headway.Blazor.Controls.Base;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Headway.Core.Extensions;

namespace Headway.Blazor.Controls.Documents
{
    [DynamicDocument]
    public abstract class ListDetailBase<T> : GenericComponentBase<T> where T : class, new()
    {
        protected DynamicModel<T> dynamicModel;
        protected DynamicList<T> dynamicList;
        protected string filterString;

        protected bool FilterFunction(DynamicListItem<T> item) => FilterItem(item, filterString);

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            await NewAsync().ConfigureAwait(false);

            var list = (List<T>)Field.PropertyInfo.GetValue(Field.Model, null);

            var listConfig = ComponentArgs.FirstDynamicArgValueToString(Args.LIST_CONFIG);

            dynamicList = await GetDynamicListAsync(list, listConfig).ConfigureAwait(false);
        }

        protected async Task NewAsync()
        {
            dynamicModel = await CreateDynamicModelAsync(Config.Name).ConfigureAwait(false);
            PropagateLinkedFields(dynamicModel.DynamicFields);
        }

        protected async Task AddAsync(DynamicModel<T> model)
        {
            if (model.IsValid)
            {
                var listItem = dynamicList.DynamicListItems.FirstOrDefault(i => i.Model.Equals(model.Model));

                if (listItem != null)
                {
                    return;
                }

                dynamicList.Add(model.Model);
                Field.PropertyInfo.PropertyType.GetMethod("Add").Invoke(
                    (List<T>)Field.PropertyInfo.GetValue(Field.Model, null), new T[] { model.Model });

                await NewAsync().ConfigureAwait(false);
            }
        }

        protected async Task RemoveAsync(DynamicModel<T> model)
        {
            var listItem = dynamicList.DynamicListItems.FirstOrDefault(i => i.Model.Equals(model.Model));
            
            if(listItem != null)
            {
                dynamicList.Remove(listItem);

                Field.PropertyInfo.PropertyType.GetMethod("Remove").Invoke(
                    (List<T>)Field.PropertyInfo.GetValue(Field.Model, null), new T[] { model.Model });
            }

            await NewAsync().ConfigureAwait(false);
        }

        protected async void RowClickEvent(TableRowClickEventArgs<DynamicListItem<T>> tableRowClickEventArgs)
        {
            if (tableRowClickEventArgs != null
                && tableRowClickEventArgs.Item != null)
            {
                dynamicModel = await GetDynamicModelAsync(tableRowClickEventArgs.Item.Model, Config.Name).ConfigureAwait(false);
                PropagateLinkedFields(dynamicModel.DynamicFields);
            }
        }

        private bool FilterItem(DynamicListItem<T> item, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            foreach (var column in dynamicList.ConfigItems)
            {
                var value = dynamicList.GetValue(item.Model, column.PropertyName);

                if (value != null
                    && value.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
