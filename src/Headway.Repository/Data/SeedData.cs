using Headway.Core.Model;
using System.Linq;

namespace Headway.Repository.Data
{
    public class SeedData
    {
        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            if (!applicationDbContext.Users.Any()
                && !applicationDbContext.Permissions.Any())
            {
                var admin = new Permission { Name = "Admin", Description = "Administrator" };
                var user = new Permission { Name = "User", Description = "Headway User" };
                applicationDbContext.Permissions.Add(admin);
                applicationDbContext.Permissions.Add(user);
                applicationDbContext.SaveChanges();

                var alice = new User { UserName = "alice", Email = "alice@email.com" };
                applicationDbContext.Users.Add(alice);
                applicationDbContext.SaveChanges();

                var adminRole = new Role { Name = "Admin", Description = "Administrator Role" };
                var userRole = new Role { Name = "User", Description = "Headway User Role" };
                applicationDbContext.Roles.Add(adminRole);
                applicationDbContext.Roles.Add(userRole);
                applicationDbContext.SaveChanges();

                adminRole.Permissions.Add(admin);
                userRole.Permissions.Add(user);
                applicationDbContext.SaveChanges();

                alice.Roles.Add(adminRole);
                alice.Roles.Add(userRole);
                applicationDbContext.SaveChanges();

                var home = new Module { Name = "Home", Permission = user.Name };
                var administration = new Module { Name = "Administration", Permission = admin.Name };
                applicationDbContext.Modules.Add(home);
                applicationDbContext.Modules.Add(administration);
                applicationDbContext.SaveChanges();

                var homeCategory = new Category { Name = "Home Category", Permission = user.Name };
                var authorisation = new Category { Name = "Authorisation", Order = 1, Permission = admin.Name };
                var configuration = new Category { Name = "Configuration", Order = 2, Permission = admin.Name };
                applicationDbContext.Categories.Add(homeCategory);
                applicationDbContext.Categories.Add(authorisation);
                applicationDbContext.Categories.Add(configuration);
                applicationDbContext.SaveChanges();

                home.Categories.Add(homeCategory);
                applicationDbContext.SaveChanges();

                administration.Categories.Add(authorisation);
                administration.Categories.Add(configuration);
                applicationDbContext.SaveChanges();

                var homeMenuItem = new MenuItem { Name = "Home", ImageClass = "oi oi-home", Path = "/", Permission = user.Name };
                var usersMenuItem = new MenuItem { Name = "Users", ImageClass = "oi oi-person", Path = "users", Order = 1, Permission = admin.Name };
                var rolesMenuItem = new MenuItem { Name = "Roles", ImageClass = "oi oi-lock-locked", Path = "roles", Order = 2, Permission = admin.Name };
                var permissionsMenuItem = new MenuItem { Name = "Permissions", ImageClass = "oi oi-key", Path = "permissions", Order = 3, Permission = admin.Name };
                applicationDbContext.MenuItems.Add(homeMenuItem);
                applicationDbContext.MenuItems.Add(usersMenuItem);
                applicationDbContext.MenuItems.Add(rolesMenuItem);
                applicationDbContext.MenuItems.Add(permissionsMenuItem);
                applicationDbContext.SaveChanges();

                homeCategory.MenuItems.Add(homeMenuItem);
                applicationDbContext.SaveChanges();

                authorisation.MenuItems.Add(usersMenuItem);
                authorisation.MenuItems.Add(rolesMenuItem);
                authorisation.MenuItems.Add(permissionsMenuItem);
                applicationDbContext.SaveChanges();
            }
        }
    }
}
