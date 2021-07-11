using Headway.Core.Attributes;
using Headway.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class Module : IPermissionable
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

        public bool IsPermitted(IEnumerable<string> permissions)
        {
            if(permissions.Contains(Permission))
            {
                foreach (var category in Categories)
                {
                    category.IsPermitted(permissions);
                }

                return true;
            }
            else
            {
                Categories.Clear();
                return false;
            }
        }
    }
}
