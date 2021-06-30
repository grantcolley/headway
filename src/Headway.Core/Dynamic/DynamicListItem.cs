namespace Headway.Core.Dynamic
{
    public class DynamicListItem<T>
    {
        public DynamicListItem(T model)
        {
            Model = model;
        }

        public T Model { get; private set; }
    }
}
