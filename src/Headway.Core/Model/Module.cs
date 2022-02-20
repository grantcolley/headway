using Headway.Core.Attributes;
using Headway.Core.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int Order { get; set; }
        public List<Category> Categories { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name must be between 1 and 20 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Icon is required")]
        [StringLength(30, ErrorMessage = "Icon must be between 1 and 30 characters")]
        public string Icon { get; set; }

        [Required(ErrorMessage = "Permission is required")]
        [StringLength(20, ErrorMessage = "Permission must be between 1 and 20 characters")]
        public string Permission { get; set; }

        public bool IsPermitted(IEnumerable<string> permissions)
        {
            if(permissions.Contains(Permission))
            {
                var count = Categories.Count;

                if (count > 0)
                {
                    for (int i = count - 1; i >= 0; i--)
                    {
                        if (Categories[i].IsPermitted(permissions))
                        {
                            continue;
                        }

                        Categories.RemoveAt(i);
                    }
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
