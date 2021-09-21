using Headway.Core.Dynamic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IDynamicService
    {
        Task<IServiceResult<DynamicList<T>>> GetDynamicListAsync<T>(IEnumerable<T> list, string config) where T : class, new();
        Task<IServiceResult<DynamicList<T>>> GetDynamicListAsync<T>(string configName) where T : class, new();
        Task<IServiceResult<DynamicModel<T>>> GetDynamicModelAsync<T>(int id, string config) where T : class, new();
        Task<IServiceResult<DynamicModel<T>>> CreateDynamicModelInstanceAsync<T>(string config) where T : class, new();
        Task<IServiceResult<DynamicModel<T>>> AddDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new();
        Task<IServiceResult<DynamicModel<T>>> UpdateDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new();
        Task<IServiceResult<int>> DeleteDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new();
    }
}
