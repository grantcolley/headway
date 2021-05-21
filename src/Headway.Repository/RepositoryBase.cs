using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public abstract class RepositoryBase
    {
        protected ApplicationDbContext applicationDbContext;

        protected RepositoryBase(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        protected async Task ValidateClaim(string userName, string permission)
        {
            var user = await applicationDbContext.Users.FirstOrDefaultAsync(
                u => u.UserName.Equals(userName)
                && u.Permissions.Any(p => p.Name.Equals(permission)))
                .ConfigureAwait(false);
            if (user == null)
            {
                throw new UnauthorizedAccessException(userName);
            }
        }
    }
}
