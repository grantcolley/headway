using Headway.Core.Interface;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public abstract class RepositoryBase<T> : IRepository
    {
        protected readonly ApplicationDbContext applicationDbContext;
        protected readonly ILogger<T> logger;
        private bool disposedValue;

        protected RepositoryBase(ApplicationDbContext applicationDbContext, ILogger<T> logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
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
            GC.SuppressFinalize(this);
        }
    }
}
