using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class MenuItem
    {
        public MenuItem()
        {
            MenuItems = new List<MenuItem>();
        }

        public int Id { get; set; }
        public int ParentId { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string ImageClass { get; set; }
        public string Path { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }
}