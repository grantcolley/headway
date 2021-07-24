using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class ConfigurationRepository : RepositoryBase, IConfigurationRepository
    {
        public ConfigurationRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public async Task<IEnumerable<ConfigType>> GetConfigTypesAsync()
        {
            return await applicationDbContext.ConfigTypes
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Config>> GetConfigsAsync()
        {
            return await applicationDbContext.Configs
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Config> GetConfigAsync(int id)
        {
            return await applicationDbContext.Configs
                .Include(c => c.ConfigItems)
                .AsNoTracking()
                .SingleAsync(c => c.ConfigId.Equals(id))
                .ConfigureAwait(false);        
        }

        public async Task<Config> GetConfigAsync(string name)
        {
            return await applicationDbContext.Configs
                .Include(c => c.ConfigItems)
                .AsNoTracking()
                .SingleAsync(c => c.Name.Equals(name))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Config>> GetConfigsByTypeAsync(int configTypeId)
        {
            return await applicationDbContext.Configs
                .Where(c => c.ConfigType.ConfigTypeId.Equals(configTypeId))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
