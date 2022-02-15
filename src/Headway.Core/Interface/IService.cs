using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IService
    {
        Task<IResponse<T>> GetServiceResultAsync<T>(HttpResponseMessage httpResponseMessage);
    }
}
