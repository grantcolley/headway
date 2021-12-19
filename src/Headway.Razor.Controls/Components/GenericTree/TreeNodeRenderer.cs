using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Components.GenericTree
{
    public static class TreeNodeRenderer
    {
        public static RenderFragment RenderTreeView<T>(List<T> tree, string nodeLabel, string nodesProperty) where T : class, new()
            => builder =>
        {
            var type = typeof(T).GetType();
            var component = typeof(TreeView<T>).GetType();
            var genericType = component.MakeGenericType(new[] { type });
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, TreeViewConstants.TREEVIEW_PARAMETER_TREE, tree);
            builder.AddAttribute(3, TreeViewConstants.TREEVIEW_PARAMETER_NODE_LABEL, nodeLabel);
            builder.AddAttribute(4, TreeViewConstants.TREEVIEW_PARAMETER_NODES_PROPERTY, nodesProperty);
            builder.CloseComponent();
        };

        public static RenderFragment RenderTreeNode<T>(Node<T> node) where T : class, new()
            => builder =>
        {
            var type = node.GetType();
            var component = typeof(TreeNode<T>).GetType();
            var genericType = component.MakeGenericType(new[] { type });
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, TreeViewConstants.TREENODE_PARAMETER_NODE, node);
            builder.CloseComponent();
        };
    }
}
