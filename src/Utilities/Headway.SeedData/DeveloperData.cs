using Headway.Core.Constants;
using Headway.Core.Model;
using Headway.Repository.Data;
using System.Collections.Generic;

namespace Headway.SeedData
{
    public class DeveloperData
    {
        private static ApplicationDbContext dbContext;

        private static Role developerRole;
        private static Dictionary<string, User> developers = new Dictionary<string, User>();

        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            dbContext = applicationDbContext;

            CreateDeveloperPermission();
            CreateDeveloperRole();
            AssignDeveloperRoleAllPermissions();
            CreateDevelopers();
        }

        private static void CreateDeveloperPermission()
        {
            var developerPermission = new Permission { Name = HeadwayAuthorisation.DEVELOPER, Description = "Headway Developer" };
            dbContext.Permissions.Add(developerPermission);
            dbContext.SaveChanges();
        }

        private static void CreateDeveloperRole()
        {
            developerRole = new Role { Name = HeadwayAuthorisation.DEVELOPER, Description = "Headway Developer" };
            dbContext.Roles.Add(developerRole);
            dbContext.SaveChanges();
        }

        private static void AssignDeveloperRoleAllPermissions()
        {
            var permissions = dbContext.Permissions;
            developerRole.Permissions.AddRange(permissions);
            dbContext.SaveChanges();
        }

        private static void CreateDevelopers()
        {
            developers.Add("grant", new User { UserName = "grant", Email = "grant@email.com" });

            foreach(var developer in developers.Values)
            {
                developer.Roles.Add(developerRole);
                dbContext.Users.Add(developer);
            }

            dbContext.SaveChanges();
        }
    }
}
