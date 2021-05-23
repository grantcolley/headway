using System.Net.Http;

namespace Headway.Core.Interface
{
    public interface IService
    {
        bool IsSuccessStatusCode(HttpResponseMessage httpResponseMessage);
    }
}
