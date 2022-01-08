using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Headway.Razor.Controls.Components.GenericTree
{
    public class Node<T> where T : class, new()
    {
        private const string noNodes = "";
        private const string expandNodes = "fas fa-plus fa-xs";
        private const string collapseNodes = "fas fa-minus fa-xs";

        public Node()
        {
            Nodes = new List<Node<T>>();
        }

        public T Model { get; set; }
        public string Label { get; set; }
        public string UniqueValue { get; set; }
        public bool ExpandNodes { get; set; }
        
        public string ExpandCss
        {
            get
            {
                if(Nodes.Any())
                {
                    if(ExpandNodes)
                    {
                        return expandNodes;
                    }

                    return collapseNodes;
                }

                return noNodes;
            }
        }

        public Node<T> Source { get; set; }
        public List<Node<T>> Nodes { get; set; }
        public PropertyInfo ModelNodesPropertyInfo { get; set; }
    }
}
