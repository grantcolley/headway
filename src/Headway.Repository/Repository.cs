using Headway.Core.Interface;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public abstract class Repository : IRepository
    {
        protected ApplicationDbContext applicationDbContext;

        protected Repository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<bool> IsAuthorisedAsync(string claim, string permission)
        {
            return await applicationDbContext.Users.AnyAsync(
                u => u.Email.Equals(claim)
                && (u.Permissions.Any(p => p.Name.Equals(permission))
                || u.Roles.SelectMany(r => r.Permissions).Any(p => p.Name.Equals(permission))))
                .ConfigureAwait(false);
        }
    }
}
