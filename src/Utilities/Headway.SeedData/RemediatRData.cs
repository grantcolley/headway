using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace Headway.SeedData
{
    public class RemediatRData
    {
        private static Permission user = null;
        private static Permission admin = null;
        private static Permission developer = null;

        private static Role developerRole = null;
        private static Role adminRole = null;
        private static Role userRole = null;

        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            TruncateTables(applicationDbContext);

            Permissions(applicationDbContext);
            Roles(applicationDbContext);
            Users(applicationDbContext);
        }

        private static void TruncateTables(ApplicationDbContext applicationDbContext)
        {
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE RoleUser");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionUser");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionRole");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DELETE FROM Users");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Users, RESEED, 1)");
        }

        private static void Permissions(ApplicationDbContext applicationDbContext)
        {
            user = new Permission { Name = "User", Description = "Headway User" };
            admin = new Permission { Name = "Admin", Description = "Administrator" };
            developer = new Permission { Name = "Developer", Description = "Developer" };

            applicationDbContext.Permissions.Add(user);
            applicationDbContext.Permissions.Add(admin);
            applicationDbContext.Permissions.Add(developer);

            applicationDbContext.SaveChanges();
        }

        private static void Roles(ApplicationDbContext applicationDbContext)
        {
            developerRole = new Role { Name = "Developer", Description = "Developer Role" };
            adminRole = new Role { Name = "Admin", Description = "Administrator Role" };
            userRole = new Role { Name = "User", Description = "Headway User Role" };

            applicationDbContext.Roles.Add(adminRole);
            applicationDbContext.Roles.Add(developerRole);
            applicationDbContext.Roles.Add(userRole);

            adminRole.Permissions.Add(admin);
            adminRole.Permissions.Add(user);
            developerRole.Permissions.Add(developer);
            developerRole.Permissions.Add(admin);
            developerRole.Permissions.Add(user);
            userRole.Permissions.Add(user);

            applicationDbContext.SaveChanges();
        }

        private static void Users(ApplicationDbContext applicationDbContext)
        {
            var bob = new User { UserName = "bob", Email = "bob@email.com" };
            var jane = new User { UserName = "jane", Email = "jane@email.com" };
            var will = new User { UserName = "will", Email = "will@email.com" };

            applicationDbContext.Users.Add(bob);
            applicationDbContext.Users.Add(jane);
            applicationDbContext.Users.Add(will);

            bob.Roles.Add(userRole);
            jane.Roles.Add(userRole);
            will.Roles.Add(userRole);

            applicationDbContext.SaveChanges();
        }
    }
}
