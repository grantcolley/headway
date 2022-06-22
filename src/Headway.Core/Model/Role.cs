using Headway.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class Role
    {
        public Role()
        {
            Users = new List<User>();
            Permissions = new List<Permission>();
            PermissionChecklist = new List<ChecklistItem>();
        }

        public int RoleId { get; set; }
        public List<User> Users { get; set; }
        public List<Permission> Permissions { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(150, ErrorMessage = "Description must be between 1 and 150 characters")]
        public string Description { get; set; }

        [NotMapped]
        public List<ChecklistItem> PermissionChecklist { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<string> PermissionList
        {
            get
            {
                if(PermissionChecklist == null)
                {
                    return new List<string>();
                }

                return PermissionChecklist
                    .Where(p => p.IsChecked)
                    .Select(r => r.Name)
                    .OrderBy(p => p)
                    .ToList();
            }
        }

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
    }
}
