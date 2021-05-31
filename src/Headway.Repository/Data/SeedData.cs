using Headway.Core.Model;
using System.Linq;

namespace Headway.Repository.Data
{
    public class SeedData
    {
        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            if(!applicationDbContext.Users.Any()
                && !applicationDbContext.Permissions.Any())
            {
                var permission = new Permission { Name = "Admin", Description = "Administrator" };
                applicationDbContext.Permissions.Add(permission);
                applicationDbContext.SaveChanges();

                var user = new User { UserName = "alice", Email = "alice@email.com" };
                applicationDbContext.Users.Add(user);
                applicationDbContext.SaveChanges();

                var alice = applicationDbContext.Users.First(u => u.UserName.Equals("alice"));
                var admin = applicationDbContext.Permissions.First(p => p.Name.Equals("Admin"));

                alice.Permissions.Add(admin);
                applicationDbContext.SaveChanges();
            }
        }
    }
}
