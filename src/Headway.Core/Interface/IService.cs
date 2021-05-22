using System.Net.Http;

namespace Headway.Core.Interface
{
    public interface IService
    {
        void AddHttpClientAuthorisationHeader();
        bool IsSuccessStatusCode(HttpResponseMessage httpResponseMessage);
    }
}
