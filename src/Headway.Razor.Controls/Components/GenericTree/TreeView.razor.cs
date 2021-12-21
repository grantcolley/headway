using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components.GenericTree
{
    public class TreeViewBase<T> : ComponentBase where T : class, new()
    {
        private DynamicTypeHelper<T> typeHelper;

        protected List<Node<T>> nodes;

        protected string dropClass = "";

        [Parameter]
        public List<T> Tree { get; set; }

        [Parameter]
        public string NodeLabel { get; set; }

        [Parameter]
        public string NodesProperty { get; set; }

        public Payload<T> Payload { get; set; }

        public void Move(Node<T> dragNode, Node<T> dropNode)
        {
            if (dragNode.Source == null
                && nodes.Contains(dragNode))
            { 
                nodes.Remove(dragNode);
            }
            else if(dragNode.Source.Nodes.Contains(dragNode))
            {
                dragNode.Source.Nodes.Remove(dragNode);
            }

            dragNode.Source = dropNode;
            dropNode.Nodes.Add(dragNode);
        }

        public void Add(Node<T> dragNode)
        {
            if(dragNode.Source != null
                && dragNode.Source.Nodes.Contains(dragNode))
            {
                dragNode.Source.Nodes.Remove(dragNode);
            }

            dragNode.Source = null;
            nodes.Add(dragNode);
        }

        protected override void OnInitialized()
        {
            typeHelper = DynamicTypeHelper.Get<T>();

            base.OnInitialized();
        }

        protected override Task OnParametersSetAsync()
        {
            nodes = new List<Node<T>>();

            foreach(var node in Tree)
            {
                nodes.Add(NodeBuilder(node, null));
            }

            return base.OnParametersSetAsync();
        }

        protected RenderFragment RenderTreeNode(Node<T> node)
        {
            return TreeNodeRenderer.RenderTreeNode(node);
        }

        protected void HandleDragEnter()
        {
            if (Payload == null)
            {
                dropClass = "";
                return;
            }

            if (nodes.Any(n => n.Equals(Payload.DragNode)))
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
            if (Payload != null
                && dropClass.Equals("can-drop"))
            {
                Add(Payload.DragNode);
            }

            Payload = null;
            dropClass = "";
        }

        private Node<T> NodeBuilder(T model, Node<T> source)
        {
            var node = new Node<T> 
            {
                Model = model,
                Label = (string)typeHelper.GetValue(model, NodeLabel),
                Source = source
            };

            var modelNodes = (List<T>)typeHelper.GetValue(model, NodesProperty);

            if(modelNodes != null)
            {
                foreach(var modelNode in modelNodes)
                {
                    node.Nodes.Add(NodeBuilder(modelNode, node));
                }
            }

            return node;
        }
    }
}
