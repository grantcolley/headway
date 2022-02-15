using Headway.Core.Dynamic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IDynamicApiRequest
    {
        Task<IResponse<DynamicList<T>>> GetDynamicListAsync<T>(IEnumerable<T> list, string config) where T : class, new();
        Task<IResponse<DynamicList<T>>> GetDynamicListAsync<T>(string configName) where T : class, new();
        Task<IResponse<DynamicModel<T>>> GetDynamicModelAsync<T>(T model, string config) where T : class, new();
        Task<IResponse<DynamicModel<T>>> GetDynamicModelAsync<T>(int id, string config) where T : class, new();
        Task<IResponse<DynamicModel<T>>> CreateDynamicModelInstanceAsync<T>(string config) where T : class, new();
        Task<IResponse<DynamicModel<T>>> AddDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new();
        Task<IResponse<DynamicModel<T>>> UpdateDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new();
        Task<IResponse<int>> DeleteDynamicModelAsync<T>(DynamicModel<T> dynamicModel) where T : class, new();
    }
}
