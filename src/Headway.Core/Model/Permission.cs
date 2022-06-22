using Headway.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

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

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(150, ErrorMessage = "Description must be between 1 and 150 characters")]
        public string Description { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<string> UserList
        {
            get
            {
                if (Users == null)
                {
                    return new List<string>();
                }

                return Users
                    .Where(u => u != null)
                    .Select(u => u.Email)
                    .OrderBy(e => e)
                    .ToList();
            }
        }

        [NotMapped]
        [JsonIgnore]
        public List<string> RoleList
        {
            get
            {
                if (Roles == null)
                {
                    return new List<string>();
                }

                return Roles
                    .Where(r => r != null)
                    .Select(r => r.Name)
                    .OrderBy(r => r)
                    .ToList();
            }
        }
    }
}
