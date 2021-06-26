using Headway.Core.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IConfigService
    {
        Task<IServiceResult<ModelConfig>> GetModelConfigAsync<T>(HttpClient httpClient, TokenProvider tokenProvider);
    }
}
