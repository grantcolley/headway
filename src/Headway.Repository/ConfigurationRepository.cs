using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class ConfigurationRepository : RepositoryBase, IConfigurationRepository
    {
        public ConfigurationRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public async Task<IEnumerable<Config>> GetConfigsAsync()
        {
            return await applicationDbContext.Configs
                .AsNoTracking()
                .ToListAsync()
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


        //// OBSOLETE
        //public async Task<IEnumerable<ListConfig>> GetListConfigsAsync()
        //{
        //    return await applicationDbContext.ListConfigs
        //        .AsNoTracking()
        //        .ToListAsync()
        //        .ConfigureAwait(false);
        //}

        //public async Task<ListConfig> GetListConfigAsync(string name)
        //{
        //    return await applicationDbContext.ListConfigs
        //        .Include(l => l.ListItemConfigs)
        //        .AsNoTracking()
        //        .SingleAsync(l => l.Name.Equals(name))
        //        .ConfigureAwait(false);
        //}

        //public async Task<IEnumerable<ModelConfig>> GetModelConfigsAsync()
        //{
        //    return await applicationDbContext.ModelConfigs
        //        .AsNoTracking()
        //        .ToListAsync()
        //        .ConfigureAwait(false);
        //}

        //public async Task<ModelConfig> GetModelConfigAsync(string model)
        //{
        //    return await applicationDbContext.ModelConfigs
        //        .Include(m => m.FieldConfigs)
        //        .AsNoTracking()
        //        .SingleAsync(m => m.Model.Equals(model))
        //        .ConfigureAwait(false);
        //}
    }
}
