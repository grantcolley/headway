using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Headway.SeedData
{
    public class RemediatRData
    {
        private static ApplicationDbContext dbContext;

        private static Dictionary<string, Permission> permissions = new Dictionary<string, Permission>();
        private static Dictionary<string, Role> roles = new Dictionary<string, Role>();
        private static Dictionary<string, User> users = new Dictionary<string, User>();

        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            dbContext = applicationDbContext;

            TruncateTables();

            Permissions();
            Roles();
            Users();
            AssignUsersRoles();
        }

        private static void TruncateTables()
        {
            //((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE RoleUser");
            //((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionUser");
            //((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionRole");
            //((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Users");
            //((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Users, RESEED, 1)");
        }

        private static void Permissions()
        {
            permissions.Add("Redress Read", new Permission { Name = "Redress Read", Description = "RemediatR Redress Read" });
            permissions.Add("Redress Write", new Permission { Name = "Redress Write", Description = "RemediatR Redress Write" });
            permissions.Add("Redress Transition", new Permission { Name = "Redress Transition", Description = "RemediatR Redress Transition" });
            permissions.Add("Communication Dispatch Transition", new Permission { Name = "Communication Dispatch Transition", Description = "RemediatR Communication Dispatch Transition" });
            permissions.Add("Awaiting Response Transition", new Permission { Name = "Awaiting Response Transition", Description = "RemediatR Awaiting Response Transition" });
            permissions.Add("Redress Review Transition", new Permission { Name = "Redress Review Transition", Description = "RemediatR Redress Refund Review Transition" });
            permissions.Add("Redress Complete", new Permission { Name = "Redress Complete", Description = "RemediatR Redress Complete" });
            permissions.Add("Refund Calculation Complete", new Permission { Name = "Refund Calculation Complete", Description = "RemediatR Refund Calculation Complete" });
            permissions.Add("Refund Varification Complete", new Permission { Name = "Refund Varification Complete", Description = "RemediatR Refund Varification Complete" });
            permissions.Add("Refund Review Transition", new Permission { Name = "Refund Review Transition", Description = "RemediatR Refund Review Transition" });
            permissions.Add("Admin", new Permission { Name = "Admin", Description = "RemediatR Administrator" });

            foreach(var permission in permissions.Values)
            {
                dbContext.Permissions.Add(permission);
            }

            dbContext.SaveChanges();
        }

        private static void Roles()
        {
            roles.Add("Redress Case Owner", new Role { Name = "Redress Case Owner", Description = "RemediatR Redress Case Owner" });
            roles.Add("Redress Reviewer", new Role { Name = "Redress Reviewer", Description = "RemediatR Redress Reviewer" });
            roles.Add("Refund Assessor", new Role { Name = "Refund Assessor", Description = "RemediatR Refund Assessor" });
            roles.Add("Refund Reviewer", new Role { Name = "Refund Reviewer", Description = "RemediatR Refund Reviewer" });
            roles.Add("Admin", new Role { Name = "Admin", Description = "RemediatR Administrator" });

            foreach (var role in roles.Values)
            {
                dbContext.Roles.Add(role);
            }

            roles["Redress Case Owner"].Permissions.Add(permissions["Redress Write"]);
            roles["Redress Case Owner"].Permissions.Add(permissions["Redress Transition"]);
            roles["Redress Case Owner"].Permissions.Add(permissions["Communication Dispatch Transition"]);
            roles["Redress Case Owner"].Permissions.Add(permissions["Awaiting Response Transition"]);
            roles["Redress Reviewer"].Permissions.Add(permissions["Redress Review Transition"]);
            roles["Redress Reviewer"].Permissions.Add(permissions["Redress Complete"]);
            roles["Refund Assessor"].Permissions.Add(permissions["Refund Calculation Complete"]);
            roles["Refund Assessor"].Permissions.Add(permissions["Refund Varification Complete"]);
            roles["Refund Reviewer"].Permissions.Add(permissions["Refund Review Transition"]);
            roles["Admin"].Permissions.Add(permissions["Admin"]);

            dbContext.SaveChanges();
        }

        private static void Users()
        {
            users.Add("bill", new User { UserName = "bill", Email = "bill@email.com" });
            users.Add("jane", new User { UserName = "jane", Email = "jane@email.com" });
            users.Add("will", new User { UserName = "will", Email = "will@email.com" });
            users.Add("mel", new User { UserName = "mel", Email = "mel@email.com" });
            users.Add("grace", new User { UserName = "grace", Email = "grace@email.com" });
            users.Add("mary", new User { UserName = "mary", Email = "mary@email.com" });

            foreach (var user in users.Values)
            {
                dbContext.Users.Add(user);
            }

            dbContext.SaveChanges();
        }

        private static void AssignUsersRoles()
        {
            users["grace"].Roles.Add(roles["Redress Case Owner"]);
            users["mel"].Roles.Add(roles["Redress Reviewer"]);
            users["jane"].Roles.Add(roles["Refund Assessor"]);
            users["will"].Roles.Add(roles["Refund Assessor"]);
            users["mary"].Roles.Add(roles["Refund Reviewer"]);
            users["bill"].Roles.Add(roles["Admin"]);

            dbContext.SaveChanges();
        }
    }
}
