using Microsoft.AspNetCore.Components;
using System;

namespace Headway.Blazor.Controls.Components.GenericTree
{
    public static class TreeNodeRenderer
    {
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
