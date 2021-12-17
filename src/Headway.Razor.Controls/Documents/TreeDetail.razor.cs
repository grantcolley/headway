using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TreeDetailBase<T> : GenericComponentBase<T> where T : class, new()
    {
        protected DynamicModel<T> dynamicModel;

        protected List<T> tree;

        //protected DynamicList<T> dynamicList;

        protected override async Task OnInitializedAsync()
        {
            await NewAsync().ConfigureAwait(false);

            tree = (List<T>)Field.PropertyInfo.GetValue(Field.Model, null);

            //var listConfig = ComponentArgHelper.GetArgValue(ComponentArgs, Args.LIST_CONFIG);

            //dynamicList = await GetDynamicListAsync(list, listConfig).ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task NewAsync()
        {
            dynamicModel = await CreateDynamicModelAsync(Config.Name).ConfigureAwait(false);
        }

        protected async Task EditAsync(DynamicListItem<T> listItem)
        {
            dynamicModel = await GetDynamicModelAsync(listItem.Model, Config.Name).ConfigureAwait(false);
        }

        protected async Task AddAsync(DynamicModel<T> model)
        {
            //var listItem = dynamicList.DynamicListItems.FirstOrDefault(i => i.Model.Equals(model.Model));

            // traverse for duplicates....
            var treeItem = tree.FirstOrDefault();

            if (treeItem != null)
            {
                return;
            }

            tree.Add(model.Model);
            //Field.PropertyInfo.PropertyType.GetMethod("Add").Invoke(
            //    (List<T>)Field.PropertyInfo.GetValue(Field.Model, null), new T[] { model.Model });

            await NewAsync().ConfigureAwait(false);
        }

        protected async Task RemoveAsync(DynamicModel<T> model)
        {
            //var listItem = dynamicList.DynamicListItems.FirstOrDefault(i => i.Model.Equals(model.Model));

            var treeItem = tree.FirstOrDefault();

            if (treeItem != null)
            {
                tree.Remove(treeItem);

                //Field.PropertyInfo.PropertyType.GetMethod("Remove").Invoke(
                //    (List<T>)Field.PropertyInfo.GetValue(Field.Model, null), new T[] { model.Model });
            }

            await NewAsync().ConfigureAwait(false);
        }
    }
}
