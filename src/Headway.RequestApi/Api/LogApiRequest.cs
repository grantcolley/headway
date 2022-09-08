using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Headway.RequestApi.Api
{
    public class LogApiRequest : ApiRequestBase, ILogApiRequest
    {
        public LogApiRequest(HttpClient httpClient)
            : this(httpClient, false, null)
        {
        }

        public LogApiRequest(HttpClient httpClient, TokenProvider tokenProvider)
            : this(httpClient, true, tokenProvider)
        {
        }

        public LogApiRequest(HttpClient httpClient, bool useAccessToken, TokenProvider tokenProvider)
            : base(httpClient, useAccessToken, tokenProvider)
        {
        }

        public async Task LogAsync(Log log)
        {
            using var httpResponseMessage = await httpClient.PostAsJsonAsync(Controllers.LOG, log)
                .ConfigureAwait(false);
        }
    }
}
