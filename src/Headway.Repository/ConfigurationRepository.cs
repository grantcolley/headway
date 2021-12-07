using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
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
                .AsNoTracking()
                .Include(c => c.ConfigContainers)
                .Include(c => c.ConfigItems)
                .ThenInclude(ci => ci.ConfigContainer)
                .SingleAsync(c => c.ConfigId.Equals(id))
                .ConfigureAwait(false);        
        }

        public async Task<Config> GetConfigAsync(string name)
        {
            var result = await applicationDbContext.Configs
                .AsNoTrackingWithIdentityResolution()
                .Include(c => c.ConfigContainers)
                .Include(c => c.ConfigItems)
                .ThenInclude(ci => ci.ConfigContainer)
                .SingleAsync(c => c.Name.Equals(name))
                .ConfigureAwait(false);

            return result;
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
            var existingConfig = await applicationDbContext.Configs
                .Include(c => c.ConfigItems)
                .FirstOrDefaultAsync(c => c.ConfigId.Equals(config.ConfigId))
                .ConfigureAwait(false);

            if(existingConfig == null)
            {
                applicationDbContext.Configs.Add(config);
            }
            else
            {
                applicationDbContext
                    .Entry(existingConfig)
                    .CurrentValues.SetValues(config);

                foreach(var configItem in config.ConfigItems)
                {
                    var existingConfigItem = existingConfig.ConfigItems
                        .FirstOrDefault(ci => ci.ConfigItemId.Equals(configItem.ConfigItemId));

                    if(existingConfigItem == null)
                    {
                        existingConfig.ConfigItems.Add(configItem);
                    }
                    else
                    {
                        applicationDbContext.Entry(existingConfigItem).CurrentValues.SetValues(configItem);
                    }
                }

                foreach (var configItem in existingConfig.ConfigItems)
                {
                    if (!config.ConfigItems.Any(ci => ci.ConfigItemId.Equals(configItem.ConfigItemId)))
                    {
                        applicationDbContext.Remove(configItem);
                    }
                }
            }

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

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
