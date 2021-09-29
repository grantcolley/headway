using Headway.Core.Model;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace Headway.Razor.Controls.Model
{
    public class GenericItem<T>
    {
        private List<Arg> args;

        public GenericItem(T item, PropertyInfo propertyInfo)
        {
            Item = item;

            var name = propertyInfo.GetValue(item);

            if (name != null)
            {
                Name = name.ToString();
            }

            args = new List<Arg>()
            {
                new Arg { Name = "Type", Value = $"{Item.GetType().FullName}, {Item.GetType().Assembly.GetName().Name}" },
                new Arg { Name = "Value", Value = JsonSerializer.Serialize(Item)}
            };
        }

        public T Item { get; private set; }
        public string Name { get; private set; }
        public string Args
        {
            get { return JsonSerializer.Serialize(args); }
            set
            {
                if (args.ToString() != value)
                {
                }
            }
        }
    }
}
