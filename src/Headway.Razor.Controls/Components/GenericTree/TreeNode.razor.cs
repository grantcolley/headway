using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components.GenericTree
{
    public abstract class TreeNodeBase<T> : ComponentBase where T : class, new()
    {
        [CascadingParameter]
        TreeView<T> TreeView { get; set; }

        [Parameter]
        public Node<T> Node { get; set; }

        private const string emptyDrop = "";
        private const string noDrop = "no-drop";
        private const string canDrop = "can-drop";
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
                dropClass = emptyDrop;
                return;
            }

            if (IsDecendent(Node)
                || Node.Nodes.Any(n => n.Equals(TreeView.Payload.DragNode)))
            {
                dropClass = noDrop;
            }
            else
            {
                dropClass = canDrop;
            }
        }

        protected void HandleDragLeave()
        {
            dropClass = emptyDrop;
        }

        protected void HandleDrop()
        {
            if (TreeView.Payload != null
                && dropClass.Equals(canDrop))
            {
                TreeView.Move(TreeView.Payload.DragNode, Node);
            }

            TreeView.Payload = null;
            dropClass = emptyDrop;
        }

        protected async Task SelectAsync()
        {
            await TreeView.SelectActiveNode(Node).ConfigureAwait(false);
        }

        private bool IsDecendent(Node<T> node)
        {
            if(node.Equals(TreeView.Payload.DragNode))
            {
                return true;
            }

            if (node.Source == null)
            {
                return false;
            }

            return IsDecendent(node.Source);
        }
    }
}
