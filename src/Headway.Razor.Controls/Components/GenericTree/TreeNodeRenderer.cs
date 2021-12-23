using Headway.Core.Dynamic;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Components.GenericTree
{
    public static class TreeNodeRenderer
    {
        public static RenderFragment RenderTreeView<T>(DynamicField field, List<DynamicArg> componentArgs) where T : class, new()
            => builder =>
        {
            var genericType = Type.GetType(typeof(TreeView<T>).AssemblyQualifiedName);
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, TreeViewConstants.TREEVIEW_PARAMETER_FIELD, field);
            builder.AddAttribute(3, TreeViewConstants.TREEVIEW_PARAMETER_COMPONENT_ARGS, componentArgs);
            //builder.AddAttribute(4, TreeViewConstants.TREEVIEW_PARAMETER_ON_SELECTED_NODE, null);
            builder.CloseComponent();
        };

        public static RenderFragment RenderTreeNode<T>(Node<T> node) where T : class, new()
            => builder =>
        {
            var genericType = Type.GetType(typeof(TreeNode<T>).AssemblyQualifiedName);
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, TreeViewConstants.TREENODE_PARAMETER_NODE, node);
            builder.CloseComponent();
        };
    }
}
