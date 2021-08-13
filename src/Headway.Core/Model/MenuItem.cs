using Headway.Core.Attributes;
using Headway.Core.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class MenuItem : IPermissionable
    {
        public int MenuItemId { get; set; }
        public int Order { get; set; }
        public Category Category { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name must be between 1 and 20 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ImageClass is required")]
        [StringLength(30, ErrorMessage = "ImageClass must be between 1 and 30 characters")]
        public string ImageClass { get; set; }

        [Required(ErrorMessage = "NavigateTo is required")]
        [StringLength(20, ErrorMessage = "NavigateTo must be between 1 and 20 characters")]
        public string NavigateTo { get; set; }

        [Required(ErrorMessage = "Config is required")]
        [StringLength(20, ErrorMessage = "Config must be between 1 and 20 characters")]
        public string Config { get; set; }

        [Required(ErrorMessage = "Permission is required")]
        [StringLength(20, ErrorMessage = "Permission must be between 1 and 20 characters")]
        public string Permission { get; set; }

        public string NavigateFullPath()
        {
            return $@"{NavigateTo}\{Config}";
        }

        public bool IsPermitted(IEnumerable<string> permissions)
        {
            if (permissions.Contains(Permission))
            {
                return true;
            }
            else
            {
                Category.MenuItems.Remove(this);
                return false;
            }
        }
    }
}