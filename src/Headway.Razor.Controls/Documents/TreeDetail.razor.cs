using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Components.GenericTree;
using Microsoft.AspNetCore.Components;
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
        private string nodeLabel;
        private string nodesProperty;

        protected override async Task OnInitializedAsync()
        {
            tree = (List<T>)Field.PropertyInfo.GetValue(Field.Model, null);
            nodeLabel = ComponentArgHelper.GetArgValue(ComponentArgs, Args.MODEL_LABEL_PROPERTY);
            nodesProperty = ComponentArgHelper.GetArgValue(ComponentArgs, Args.MODEL_LIST_PROPERTY);

            await NewAsync().ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected RenderFragment RenderTreeView()
        {
            return TreeNodeRenderer.RenderTreeView(tree, nodeLabel, nodesProperty);
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
