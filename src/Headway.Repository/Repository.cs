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
        private bool disposedValue;

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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    applicationDbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
