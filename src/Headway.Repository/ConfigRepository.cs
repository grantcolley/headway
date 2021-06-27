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
