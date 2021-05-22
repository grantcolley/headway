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

        public async Task<bool> IsAuthorisedAsync(string userName, string permission)
        {
            return await applicationDbContext.Users.AnyAsync(
                u => u.UserName.Equals(userName)
                && u.Permissions.Any(p => p.Name.Equals(permission)))
                .ConfigureAwait(false);
        }
    }
}
