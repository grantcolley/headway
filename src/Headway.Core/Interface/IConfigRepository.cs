using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IConfigRepository : IRepository
    {
        Task<IEnumerable<ListConfig>> GetListConfigsAsync();
        Task<ListConfig> GetListConfigAsync(string name);
        Task<IEnumerable<ModelConfig>> GetModelConfigsAsync();
        Task<ModelConfig> GetModelConfigAsync(string model);
    }
}
