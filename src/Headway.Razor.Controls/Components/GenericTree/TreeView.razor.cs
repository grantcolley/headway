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
        private string nodeUniqueProperty;
        private string parentNodeUniqueProperty;

        protected List<Node<T>> nodes;
        protected string dropClass = "";
        protected string message = "";

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

            var parentNodeUniquePropertyValue = typeHelper.GetValue(dropNode.Model, nodeUniqueProperty)?.ToString();
            typeHelper.SetValue(dragNode.Model, parentNodeUniqueProperty, parentNodeUniquePropertyValue);

            dropNode.Nodes.Add(dragNode);
            dropNode.ModelNodesPropertyInfo.PropertyType.GetMethod("Add").Invoke(
                (List<T>)dropNode.ModelNodesPropertyInfo.GetValue(dropNode.Model, null), new T[] { dragNode.Model });

            message = $"Moved {dragNode.Label} to {dropNode.Label}";
        }

        public void Add(T model)
        {
            var node = NodeBuilder(model, null);
            var duplicate = FindDuplicate(node);

            if(duplicate == null)
            {
                Add(node);
                message = $"Added {node.Label}";
            }
            else
            {
                message = $"{node.Label} already exists with {nodeUniqueProperty} {node.UniqueValue}";
            }
        }

        public void Remove(T model)
        {
            var node = NodeBuilder(model, null);
            var duplicate = FindDuplicate(node);

            if (duplicate != null)
            {
                Remove(duplicate);
                message = $"Removed {node.Label}";
            }
        }

        public async Task SelectActiveNode(Node<T> node)
        {
            ClearMessage();

            await OnSelectActiveNode.InvokeAsync(node.Model);
        }

        public void ClearMessage()
        {
            message = "";
        }

        protected override void OnInitialized()
        {
            nodeLabel = ComponentArgHelper.GetArgValue(ComponentArgs, Args.LABEL_PROPERTY);
            nodesProperty = ComponentArgHelper.GetArgValue(ComponentArgs, Args.LIST_PROPERTY);
            nodeUniqueProperty = ComponentArgHelper.GetArgValue(ComponentArgs, Args.UNIQUE_PROPERTY);
            parentNodeUniqueProperty = ComponentArgHelper.GetArgValue(ComponentArgs, Args.PARENT_UNIQUE_PROPERTY);

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
            ClearMessage();

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
            ClearMessage();
            dropClass = "";
        }

        protected void HandleDrop()
        {
            if (Payload != null)
            {
                if (dropClass.Equals("can-drop"))
                {
                    Add(Payload.DragNode);
                    message = $"Moved {Payload.DragNode.Label}";
                }
                else
                {
                    ClearMessage();
                }
            }

            Payload = null;
        }

        private void Add(Node<T> dragNode)
        {
            if (dragNode.Source != null
                && dragNode.Source.Nodes.Contains(dragNode))
            {
                dragNode.Source.Nodes.Remove(dragNode);
                dragNode.Source.ModelNodesPropertyInfo.PropertyType.GetMethod("Remove").Invoke(
                    (List<T>)dragNode.Source.ModelNodesPropertyInfo.GetValue(dragNode.Source.Model, null), new T[] { dragNode.Model });
            }

            dragNode.Source = null;

            typeHelper.SetValue(dragNode.Model, parentNodeUniqueProperty, null);

            Field.PropertyInfo.PropertyType.GetMethod("Add").Invoke(
                (List<T>)Field.PropertyInfo.GetValue(Field.Model, null), new T[] { dragNode.Model });

            nodes.Add(dragNode);
        }

        private void Remove(Node<T> removeNode)
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

        private Node<T> NodeBuilder(T model, Node<T> source)
        {
            var node = new Node<T>
            {
                Model = model,
                Source = source,
                ModelNodesPropertyInfo = modelNodesPropertyInfo,
                Label = typeHelper.GetValue(model, nodeLabel)?.ToString(),
                UniqueValue = typeHelper.GetValue(model, nodeUniqueProperty)?.ToString()
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

        private Node<T> FindDuplicate(Node<T> source)
        {
            foreach (var node in nodes)
            {
                var duplicate = GetNode(source.UniqueValue, node);
                if (duplicate != null)
                {
                    return duplicate;
                }
            }

            return null;
        }

        private Node<T> GetNode(string nodeUniqueValue, Node<T> source)
        {
            if(source.UniqueValue.Equals(nodeUniqueValue))
            {
                return source;
            }
            else
            {
                foreach(var node in source.Nodes)
                {
                    return GetNode(nodeUniqueValue, node);
                }

                return null;
            }
        }
    }
}
