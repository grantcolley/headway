using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components.GenericTree
{
    public class TreeViewBase<T> : ComponentBase where T : class, new()
    {
        private DynamicTypeHelper<T> typeHelper;
        private PropertyInfo modelNodesPropertyInfo;
        private string nodeLabel;
        private string nodesProperty;

        protected List<Node<T>> nodes;
        protected string dropClass = "";

        [Parameter]
        public DynamicField Field { get; set; }

        [Parameter]
        public List<DynamicArg> ComponentArgs { get; set; }

        [Parameter]
        public EventCallback<T> OnSelectActiveNode { get; set; }

        public Payload<T> Payload { get; set; }

        public void Move(Node<T> dragNode, Node<T> dropNode)
        {
            if (dragNode.Source == null
                && nodes.Contains(dragNode))
            { 
                nodes.Remove(dragNode);
                Field.PropertyInfo.PropertyType.GetMethod("Remove").Invoke(
                    (List<T>)Field.PropertyInfo.GetValue(Field.Model, null), new T[] { dragNode.Model });
            }
            else if(dragNode.Source != null
                && dragNode.Source.Nodes.Contains(dragNode))
            {
                dragNode.Source.Nodes.Remove(dragNode);
                dragNode.Source.ModelNodesPropertyInfo.PropertyType.GetMethod("Remove").Invoke(
                    (List<T>)dragNode.Source.ModelNodesPropertyInfo.GetValue(dragNode.Source.Model, null), new T[] { dragNode.Model });
            }

            dragNode.Source = dropNode;
            dropNode.Nodes.Add(dragNode);
            dropNode.ModelNodesPropertyInfo.PropertyType.GetMethod("Add").Invoke(
                (List<T>)dropNode.ModelNodesPropertyInfo.GetValue(dropNode.Model, null), new T[] { dragNode.Model });
        }

        public void Add(Node<T> dragNode)
        {
            if(dragNode.Source != null
                && dragNode.Source.Nodes.Contains(dragNode))
            {
                dragNode.Source.Nodes.Remove(dragNode);
                dragNode.Source.ModelNodesPropertyInfo.PropertyType.GetMethod("Remove").Invoke(
                    (List<T>)dragNode.Source.ModelNodesPropertyInfo.GetValue(dragNode.Source.Model, null), new T[] { dragNode.Model });
            }

            dragNode.Source = null;
            nodes.Add(dragNode);
            Field.PropertyInfo.PropertyType.GetMethod("Add").Invoke(
                (List<T>)Field.PropertyInfo.GetValue(Field.Model, null), new T[] { dragNode.Model });
        }

        public void Remove(Node<T> removeNode)
        {
            if (removeNode.Source == null
                && nodes.Contains(removeNode))
            {
                nodes.Remove(removeNode);
                Field.PropertyInfo.PropertyType.GetMethod("Remove").Invoke(
                    (List<T>)Field.PropertyInfo.GetValue(Field.Model, null), new T[] { removeNode.Model });
            }
            else if (removeNode.Source != null
                && removeNode.Source.Nodes.Contains(removeNode))
            {
                removeNode.Source.Nodes.Remove(removeNode);
                removeNode.Source.ModelNodesPropertyInfo.PropertyType.GetMethod("Remove").Invoke(
                    (List<T>)removeNode.Source.ModelNodesPropertyInfo.GetValue(removeNode.Source.Model, null), new T[] { removeNode.Model });
            }
        }

        public async Task SelectedNode(Node<T> selectedNode)
        {
            await OnSelectActiveNode.InvokeAsync(selectedNode.Model);
        }

        protected override void OnInitialized()
        {
            nodeLabel = ComponentArgHelper.GetArgValue(ComponentArgs, Args.MODEL_LABEL_PROPERTY);
            nodesProperty = ComponentArgHelper.GetArgValue(ComponentArgs, Args.MODEL_LIST_PROPERTY);

            typeHelper = DynamicTypeHelper.Get<T>();
            modelNodesPropertyInfo = typeHelper.GetPropertyInfo(nodesProperty);

            base.OnInitialized();
        }

        protected override Task OnParametersSetAsync()
        {
            nodes = new List<Node<T>>();
            var tree = (List<T>)Field.PropertyInfo.GetValue(Field.Model, null);

            foreach (var node in tree)
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
                Label = (string)typeHelper.GetValue(model, nodeLabel),
                Source = source,
                ModelNodesPropertyInfo = modelNodesPropertyInfo
            };

            var modelNodes = (List<T>)typeHelper.GetValue(model, nodesProperty);

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
