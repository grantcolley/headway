using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Components.GenericTree;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TreeDetailBase<T> : GenericComponentBase<T> where T : class, new()
    {
        protected DynamicModel<T> dynamicModel;
        private string nodeLabel;
        private string nodesProperty;

        public async Task EditAsync(T model)
        {
            dynamicModel = await GetDynamicModelAsync(model, Config.Name).ConfigureAwait(false);
        }

        protected override async Task OnInitializedAsync()
        {
            nodeLabel = ComponentArgHelper.GetArgValue(ComponentArgs, Args.MODEL_LABEL_PROPERTY);
            nodesProperty = ComponentArgHelper.GetArgValue(ComponentArgs, Args.MODEL_LIST_PROPERTY);

            await NewAsync().ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected RenderFragment RenderTreeView()
        {
            return TreeNodeRenderer.RenderTreeView<T>(Field, nodeLabel, nodesProperty);
        }

        protected async Task NewAsync()
        {
            dynamicModel = await CreateDynamicModelAsync(Config.Name).ConfigureAwait(false);
        }

        protected async Task AddAsync(DynamicModel<T> model)
        {
            var tree = (List<T>)Field.PropertyInfo.GetValue(Field.Model, null);

            Field.PropertyInfo.PropertyType.GetMethod("Add").Invoke(
                (List<T>)Field.PropertyInfo.GetValue(Field.Model, null), new T[] { model.Model });

            await NewAsync().ConfigureAwait(false);
        }

        protected async Task RemoveAsync(DynamicModel<T> model)
        {
            //var treeItem = tree.FirstOrDefault();

            //if (treeItem != null)
            //{
            //    tree.Remove(treeItem);

            //    //Field.PropertyInfo.PropertyType.GetMethod("Remove").Invoke(
            //    //    (List<T>)Field.PropertyInfo.GetValue(Field.Model, null), new T[] { model.Model });
            //}

            await NewAsync().ConfigureAwait(false);
        }
    }
}
