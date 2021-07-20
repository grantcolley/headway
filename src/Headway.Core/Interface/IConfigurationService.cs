using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IConfigurationService
    {
        Task<IServiceResult<IEnumerable<Config>>> GetConfigsAsync();
        Task<IServiceResult<Config>> GetConfigAsync(string name);

        // Obsolete
        Task<IServiceResult<IEnumerable<ListConfig>>> GetListConfigsAsync();
        Task<IServiceResult<ListConfig>> GetListConfigAsync(string name);
        Task<IServiceResult<IEnumerable<ModelConfig>>> GetModelConfigsAsync();
        Task<IServiceResult<ModelConfig>> GetModelConfigAsync(string model);
    }
}
