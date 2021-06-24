using Headway.Core.Dynamic;
using Headway.Core.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IDynamicConfigService
    {
        Task<IServiceResult<DynamicModelConfig>> GetDynamicModelConfigAsync<T>(HttpClient httpClient, TokenProvider tokenProvider);
    }
}
