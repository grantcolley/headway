using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class ConfigRepository : RepositoryBase, IConfigRepository
    {
        public ConfigRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public Task<ListConfig> GetListConfigAsync(string model)
        {
            var listConfig = new ListConfig
            {
                ListConfigId = 1,
                IdPropertyName = "PermissionId",
                ConfigPath = "Permissions",
                ListName = "Permissions",
                ListItemConfigs = new System.Collections.Generic.List<ListItemConfig>
                {
                    { new ListItemConfig { HeaderName = "Permission Id", Order = 1, PropertyName = "PermissionId" } },
                    { new ListItemConfig { HeaderName = "Name", Order = 2, PropertyName = "Name" } },
                    { new ListItemConfig { HeaderName = "Description", Order = 3, PropertyName = "Description" } }
                 }
            };

            return Task.FromResult(listConfig);
        }

        public async Task<ModelConfig> GetModelConfigAsync(string model)
        {
            return await applicationDbContext.ModelConfigs
                .Include(m => m.FieldConfigs)
                .AsNoTracking()
                .SingleAsync(m => m.ModelName.Equals(model))
                .ConfigureAwait(false);
        }
    }
}
