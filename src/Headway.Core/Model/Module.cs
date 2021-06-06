using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class Module
    {
        public Module()
        {
            Categories = new List<Category>();
        }

        public int ModuleId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Permission { get; set; }
        public List<Category> Categories { get; set; }
    }
}
