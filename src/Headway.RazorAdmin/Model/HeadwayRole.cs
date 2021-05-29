using Headway.Core.Model;

namespace Headway.RazorAdmin.Model
{
    public class HeadwayRole
    {
        public HeadwayRole(Role role)
        {
            Role = role;
        }

        public Role Role { get; private set; }

        public string Name { get { return Role.Name; } }

        public string Description { get { return Role.Description; } }

        public bool IsSelected { get; set; }
    }
}
