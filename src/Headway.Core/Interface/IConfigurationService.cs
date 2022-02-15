using Headway.Core.Model;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IConfigurationService
    { 
        Task<IResponse<Config>> GetConfigAsync(string name);
    }
}
