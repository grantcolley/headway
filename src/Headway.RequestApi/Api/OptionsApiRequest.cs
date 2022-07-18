using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Core.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Headway.RequestApi.Api
{
    public class OptionsApiRequest : ApiRequestBase, IOptionsApiRequest
    {
        private readonly Dictionary<string, IOptionItems> localOptionItems = new();

        public OptionsApiRequest(HttpClient httpClient)
            : this(httpClient, false, null)
        {
        }

        public OptionsApiRequest(HttpClient httpClient, TokenProvider tokenProvider)
            : this(httpClient, true, tokenProvider)
        {
        }

        private OptionsApiRequest(HttpClient httpClient, bool useAccessToken, TokenProvider tokenProvider)
            : base(httpClient, useAccessToken, tokenProvider)
        {
            localOptionItems.Add(typeof(PageOptionItems).Name, new PageOptionItems());
            localOptionItems.Add(typeof(ModelOptionItems).Name, new ModelOptionItems());
            localOptionItems.Add(typeof(ModelFieldsOptionItems).Name, new ModelFieldsOptionItems());
            localOptionItems.Add(typeof(ComponentOptionItems).Name, new ComponentOptionItems());
            localOptionItems.Add(typeof(DocumentOptionItems).Name, new DocumentOptionItems());
            localOptionItems.Add(typeof(ContainerOptionItems).Name, new ContainerOptionItems());
            localOptionItems.Add(typeof(StaticOptionItems).Name, new StaticOptionItems());
        }

        public async Task<IResponse<IEnumerable<OptionItem>>> GetOptionItemsAsync(List<DynamicArg> dynamicArgs)
        {
            var optionsCode = dynamicArgs.First(a => a.Name.Equals(Options.OPTIONS_CODE)).Value.ToString();

            var args = ComponentArgHelper.GetArgs(dynamicArgs);

            if (localOptionItems.ContainsKey(optionsCode))
            {
                return new Response<IEnumerable<OptionItem>>
                {
                    IsSuccess = true,
                    Result = await localOptionItems[optionsCode].GetOptionItemsAsync(args)
                };
            }

            var httpResponseMessage = await httpClient.PostAsJsonAsync(Controllers.OPTIONS, args)
                .ConfigureAwait(false);

            return await GetResponseAsync<IEnumerable<OptionItem>>(httpResponseMessage)
                .ConfigureAwait(false);
        }

        public async Task<IResponse<IEnumerable<T>>> GetOptionItemsAsync<T>(List<DynamicArg> dynamicArgs)
        {
            var args = ComponentArgHelper.GetArgs(dynamicArgs);

            var httpResponseMessage = await httpClient.PostAsJsonAsync(Controllers.OPTIONS_COMPLEXOPTIONS, args)
                .ConfigureAwait(false);

            return await GetResponseAsync<IEnumerable<T>>(httpResponseMessage)
                .ConfigureAwait(false);
        }
    }
}
