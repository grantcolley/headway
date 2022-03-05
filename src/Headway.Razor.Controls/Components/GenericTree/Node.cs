using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Headway.Razor.Controls.Components.GenericTree
{
    public class Node<T> where T : class, new()
    {
        public Node()
        {
            Nodes = new List<Node<T>>();
        }

        public T Model { get; set; }
        public string Label { get; set; }
        public string UniqueValue { get; set; }
       
        public Node<T> Source { get; set; }
        public List<Node<T>> Nodes { get; set; }
        public PropertyInfo ModelNodesPropertyInfo { get; set; }
    }
}
