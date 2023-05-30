using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Repository.Repositories
{
    public abstract class RepositoryBase<T> : IRepository
    {
        protected readonly ApplicationDbContext applicationDbContext;
        protected readonly ILogger<T> logger;
        private bool disposedValue;

        protected string User;

        protected RepositoryBase(ApplicationDbContext applicationDbContext, ILogger<T> logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }

        public void SetUser(string user)
        {
            User = user;
            
            applicationDbContext.SetUser(user);
        }

        public async Task<bool> IsAuthorisedAsync(string claim, string permission)
        {
            return await applicationDbContext.Users
                .AsNoTracking()
                .AnyAsync(
                u => u.Email.Equals(claim)
                && (u.Permissions.Any(p => p.Name.Equals(permission))
                || u.Roles.SelectMany(r => r.Permissions).Any(p => p.Name.Equals(permission))))
                .ConfigureAwait(false);
        }

        public async Task<Authorisation> GetAuthorisationAsync(string claim)
        {
            var user = await applicationDbContext.Users
                .AsNoTracking()
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Email.Equals(claim))
                .ConfigureAwait(false);

            var permissionSet = user.GetUserPermissionSet();

            return new Authorisation { User = claim, Permissions = permissionSet };
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
            GC.SuppressFinalize(this);
        }
    }
}
