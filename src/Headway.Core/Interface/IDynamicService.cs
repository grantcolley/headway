using Headway.Core.Dynamic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IDynamicService
    {
        Task<IServiceResult<DynamicList<T>>> GetDynamicListAsync<T>(string configName);
        Task<IServiceResult<DynamicModel<T>>> GetDynamicModelAsync<T>(int id, string config);
        Task<IServiceResult<DynamicModel<T>>> CreateDynamicModelInstanceAsync<T>(string config);
        Task<IServiceResult<DynamicModel<T>>> AddDynamicModelAsync<T>(DynamicModel<T> dynamicModel);
        Task<IServiceResult<DynamicModel<T>>> UpdateDynamicModelAsync<T>(DynamicModel<T> dynamicModel);
        Task<IServiceResult<int>> DeleteDynamicModelAsync<T>(DynamicModel<T> dynamicModel);
    }
}
