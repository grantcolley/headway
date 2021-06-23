using Headway.Core.Dynamic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IDynamicConfigService
    {
        Task<IServiceResult<DynamicModelConfig>> GetDynamicModelConfigAsync<T>();
    }
}
