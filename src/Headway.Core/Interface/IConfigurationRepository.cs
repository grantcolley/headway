using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IConfigurationRepository : IRepository
    {
        Task<IEnumerable<ConfigType>> GetConfigTypesAsync();
        Task<IEnumerable<Config>> GetConfigsAsync();
        Task<Config> GetConfigAsync(int id);
        Task<Config> GetConfigAsync(string config);
        Task<IEnumerable<Config>> GetConfigsByTypeAsync(int configTypeId);
    }
}
