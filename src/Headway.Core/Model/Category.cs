using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class Category
    {
        public Category()
        {
            MenuItems = new List<MenuItem>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Permission { get; set; }
        public Module Module { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }
}
