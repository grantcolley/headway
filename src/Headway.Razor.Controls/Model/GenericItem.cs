using System.Reflection;

namespace Headway.Razor.Controls.Model
{
    public class GenericItem<T>
    {
        public GenericItem(T item, PropertyInfo propertyInfo)
        {
            Item = item;

            var name = propertyInfo.GetValue(item);

            if(name != null)
            {
                Name = name.ToString();
            }
        }

        public T Item {  get; private set; }
        public string Name { get; private set; }
    }
}
