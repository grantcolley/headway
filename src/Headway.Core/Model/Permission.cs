using Headway.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class Permission
    {
        public Permission()
        {
            Users = new List<User>();
            Roles = new List<Role>();
        }

        public int PermissionId { get; set; }
        public List<User> Users { get; set; }
        public List<Role> Roles { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, ErrorMessage = "Name must be between 1 and 20 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(30, ErrorMessage = "Description must be between 1 and 30 characters")]
        public string Description { get; set; }
    }
}
