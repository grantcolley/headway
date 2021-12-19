using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components.GenericTree
{
    public class TreeViewBase<T> : ComponentBase where T : class, new()
    {
        [Parameter]
        public List<T> Tree { get; set; }

        [Parameter]
        public string NodeLabel { get; set; }

        [Parameter]
        public string NodesProperty { get; set; }

        public Payload<T> Payload { get; set; }

        protected List<Node<T>> nodes;

        private DynamicTypeHelper<T> typeHelper;
        
        public void Move(Node<T> dragNode, Node<T> dropNode)
        {

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
                nodes.Add(NodeBuilder(node));
            }

            return base.OnParametersSetAsync();
        }

        protected RenderFragment RenderTreeNode(Node<T> node)
        {
            return TreeNodeRenderer.RenderTreeNode(node);
        }

        private Node<T> NodeBuilder(T model)
        {
            var node = new Node<T> 
            {
                Model = model,
                Label = (string)typeHelper.GetValue(model, NodeLabel)
            };

            var modelNodes = (List<T>)typeHelper.GetValue(model, NodesProperty);

            if(modelNodes != null)
            {
                foreach(var modelNode in modelNodes)
                {
                    node.Nodes.Add(NodeBuilder(modelNode));
                }
            }

            return node;
        }
    }
}
