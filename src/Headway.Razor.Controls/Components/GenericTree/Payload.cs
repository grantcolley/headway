namespace Headway.Razor.Controls.Components.GenericTree
{
    public class Payload<T> where T : class, new()
    {
        public Node<T> DragNode { get; set; }
    }
}
