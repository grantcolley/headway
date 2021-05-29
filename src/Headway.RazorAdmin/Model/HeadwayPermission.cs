using Headway.Core.Model;

namespace Headway.RazorAdmin.Model
{
    public class HeadwayPermission
    {
        public HeadwayPermission(Permission permission)
        {
            Permission = permission;
        }

        public Permission Permission { get; private set; }

        public string Name { get { return Permission.Name; } }

        public string Description { get { return Permission.Description; } }

        public bool IsSelected { get; set; }
    }
}
