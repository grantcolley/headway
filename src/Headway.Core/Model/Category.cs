using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class Category
    {
        public Category()
        {
            MenuItems = new List<MenuItem>();
            Authorised = new List<string>();
        }

        public string Name { get; set; }
        public int Order { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public List<string> Authorised { get; set; }
    }
}
