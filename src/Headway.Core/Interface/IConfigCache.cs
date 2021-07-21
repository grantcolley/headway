using Headway.Core.Model;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IConfigCache
    {
        Task<Config> GetConfigAsync(string configName);
    }
}
