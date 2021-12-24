using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Headway.Razor.Controls.Components.GenericTree;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TreeDetailBase<T> : GenericComponentBase<T> where T : class, new()
    {
        protected DynamicModel<T> dynamicModel;

        protected override async Task OnInitializedAsync()
        {
            await NewAsync().ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected RenderFragment RenderTreeView() => builder =>
        {
            var genericType = Type.GetType(typeof(TreeView<T>).AssemblyQualifiedName);
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, TreeViewConstants.TREEVIEW_PARAMETER_FIELD, Field);
            builder.AddAttribute(3, TreeViewConstants.TREEVIEW_PARAMETER_COMPONENT_ARGS, ComponentArgs);
            builder.AddAttribute(4, TreeViewConstants.TREEVIEW_PARAMETER_ON_SELECT_ACTIVE_NODE, EventCallback.Factory.Create<T>(this, EditAsync));
            builder.CloseComponent();
        };

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

        private async Task EditAsync(T model)
        {
            dynamicModel = await GetDynamicModelAsync(model, Config.Name).ConfigureAwait(false);
        }
    }
}
