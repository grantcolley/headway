using System.Reflection;

namespace Headway.Blazor.Controls.Model
{
    public class GenericItem<T>
    {
        public GenericItem(T item, PropertyInfo propertyInfo)
        {
            Item = item;

            var name = propertyInfo.GetValue(item);

            if (name != null)
            {
                Name = name.ToString();
            }
        }

        public T Item { get; private set; }
        public string Name { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var optionItem = obj as GenericItem<T>;
            return optionItem?.Name == this.Name;
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return "empty".GetHashCode();
            }

            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
