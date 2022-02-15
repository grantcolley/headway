using Headway.Core.Model;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IConfigurationApiRequest
    { 
        Task<IResponse<Config>> GetConfigAsync(string name);
    }
}
