using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class ConfigurationRepository : RepositoryBase<ConfigurationRepository>, IConfigurationRepository
    {
        public ConfigurationRepository(ApplicationDbContext applicationDbContext, ILogger<ConfigurationRepository> logger)
            : base(applicationDbContext, logger)
        {
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
                .Include(c => c.Containers)
                .Include(c => c.ConfigItems)
                .SingleAsync(c => c.ConfigId.Equals(id))
                .ConfigureAwait(false);        
        }

        public async Task<Config> GetConfigAsync(string name)
        {
            return await applicationDbContext.Configs
                .Include(c => c.Containers)
                .Include(c => c.ConfigItems)
                .SingleAsync(c => c.Name.Equals(name))
                .ConfigureAwait(false);
        }

        public async Task<Config> AddConfigAsync(Config config)
        {
            await applicationDbContext.Configs
                .AddAsync(config)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return config;
        }

        public async Task<Config> UpdateConfigAsync(Config config)
        {
            try
            {
                applicationDbContext.Configs.Update(config);

                await applicationDbContext
                    .SaveChangesAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }

            return config;
        }

        public async Task<int> DeleteConfigAsync(int configId)
        {
            var config = await applicationDbContext.Configs
                .SingleAsync(c => c.ConfigId.Equals(configId))
                .ConfigureAwait(false);

            applicationDbContext.Configs.Remove(config);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}
