using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Repository.Data;
using System;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class DynamicConfigRepository : RepositoryBase, IDynamicConfigRepository
    {
        public DynamicConfigRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public Task<DynamicModelConfig> GetDynamicModelConfigAsync(string model)
        {
            throw new NotImplementedException();
        }
    }
}
