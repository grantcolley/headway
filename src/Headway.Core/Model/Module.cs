using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class Module
    {
        public Module()
        {
            Categories = new List<Category>();
            Roles = new List<string>();
        }

        public string Name { get; set; }
        public int Order { get; set; }
        public List<Category> Categories { get; set; }
        public List<string> Roles { get; set; }
    }
}
