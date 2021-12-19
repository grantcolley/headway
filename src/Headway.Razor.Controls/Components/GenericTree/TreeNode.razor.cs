using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Headway.Razor.Controls.Components.GenericTree
{
    public abstract class TreeNodeBase<T> : ComponentBase where T : class, new()
    {
        [CascadingParameter]
        TreeView<T> TreeView { get; set; }

        [Parameter]
        public Node<T> Node { get; set; }

        protected string dropClass = "";

        protected RenderFragment RenderTreeNode(Node<T> node)
        {
            return TreeNodeRenderer.RenderTreeNode(node);
        }

        protected void HandleDragStart(Node<T> node)
        {
            if (TreeView.Payload == null)
            {
                TreeView.Payload = new Payload<T>
                {
                    DragNode = node
                };
            }
        }

        protected void HandleDragEnter()
        {
            if (TreeView.Payload == null)
            {
                dropClass = "";
                return;
            }

            if ((Node.Label.Equals(TreeView.Payload.DragNode.Label))
                || Node.Nodes.Any(n => n.Equals(TreeView.Payload.DragNode)))
            {
                dropClass = "no-drop";
            }
            else
            {
                dropClass = "can-drop";
            }
        }

        protected void HandleDragLeave()
        {
            dropClass = "";
        }

        protected void HandleDrop()
        {
            if (TreeView.Payload == null)
            {
                dropClass = "";
                return;
            }

            if (dropClass.Equals("can-drop"))
            {
                TreeView.Move(TreeView.Payload.DragNode, Node);
            }

            dropClass = "";
        }
    }
}
