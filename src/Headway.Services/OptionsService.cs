using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Services.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class OptionsService : ServiceBase, IOptionsService
    {
        private readonly Dictionary<string, IOptionItems> localOptionItems = new Dictionary<string, IOptionItems>();

        public OptionsService(HttpClient httpClient)
            : this(httpClient, false, null)
        {

        }

        public OptionsService(HttpClient httpClient, TokenProvider tokenProvider)
            : this(httpClient, true, tokenProvider)
        {
        }

        private OptionsService(HttpClient httpClient, bool useAccessToken, TokenProvider tokenProvider)
            : base(httpClient, useAccessToken, tokenProvider)
        {
            localOptionItems.Add(typeof(ModelOptionItems).Name, new ModelOptionItems());
            localOptionItems.Add(typeof(ComponentOptionItems).Name, new ComponentOptionItems());
        }

        public async Task<IServiceResult<IEnumerable<OptionItem>>> GetOptionItemsAsync(string optionsCode)
        {
            if (localOptionItems.ContainsKey(optionsCode))
            {
                return new ServiceResult<IEnumerable<OptionItem>>
                {
                    IsSuccess = true,
                    Result = await localOptionItems[optionsCode].GetOptionItemsAsync()
                };
            }

            var httpResponseMessage = await httpClient.GetAsync($"Options/{optionsCode}")
                .ConfigureAwait(false);

            return await GetServiceResultAsync<IEnumerable<OptionItem>>(httpResponseMessage)
                .ConfigureAwait(false);
        }
    }
}
