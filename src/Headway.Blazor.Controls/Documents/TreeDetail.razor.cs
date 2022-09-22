using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Model;
using Headway.Blazor.Controls.Base;
using Headway.Blazor.Controls.Components.GenericTree;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TreeDetailBase<T> : GenericComponentBase<T> where T : class, new()
    {
        protected DynamicModel<T> dynamicModel;

        private TreeView<T> treeView;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            await ResetDynamicModelAsync().ConfigureAwait(false);
        }

        protected RenderFragment RenderTreeView() => builder =>
        {
            var genericType = Type.GetType(typeof(TreeView<T>).AssemblyQualifiedName);
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, TreeViewConstants.TREEVIEW_PARAMETER_FIELD, Field);
            builder.AddAttribute(3, TreeViewConstants.TREEVIEW_PARAMETER_COMPONENT_ARGS, ComponentArgs);
            builder.AddAttribute(4, TreeViewConstants.TREEVIEW_PARAMETER_ON_SELECT_ACTIVE_NODE, EventCallback.Factory.Create<T>(this, EditAsync));
            builder.AddComponentReferenceCapture(5, inst => { treeView = (TreeView<T>)inst; });
            builder.CloseComponent();
        };

        protected async Task NewAsync()
        {
            await ResetDynamicModelAsync().ConfigureAwait(false);
        }

        protected async Task AddAsync(DynamicModel<T> model)
        {
            if(treeView != null)
            {
                if(model.IsValid)
                {
                    treeView.Add(model.Model);

                    await ResetDynamicModelAsync().ConfigureAwait(false);
                }
            }
        }

        protected async Task RemoveAsync(DynamicModel<T> model)
        {
            if (treeView != null)
            {
                treeView.Remove(model.Model);
            }

            await ResetDynamicModelAsync().ConfigureAwait(false);
        }

        private async Task EditAsync(T model)
        {
            dynamicModel = await GetDynamicModelAsync(model, Config.Name).ConfigureAwait(false);
        }

        private async Task ResetDynamicModelAsync()
        {
            dynamicModel = await CreateDynamicModelAsync(Config.Name).ConfigureAwait(false);
        }
    }
}
