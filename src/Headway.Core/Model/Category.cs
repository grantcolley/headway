using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class Category
    {
        public Category()
        {
            MenuItems = new List<MenuItem>();
            Rights = new List<string>();
        }

        public string Name { get; set; }
        public int Order { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public List<string> Rights { get; set; }
    }
}
