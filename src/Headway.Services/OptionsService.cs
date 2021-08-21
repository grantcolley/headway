using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Core.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class OptionsService : ServiceBase, IOptionsService
    {
        private const string OPTIONS_CODE = "OptionsCode";

        private readonly Dictionary<string, IOptionItems> localOptionItems = new();

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
            localOptionItems.Add(typeof(PageOptionItems).Name, new PageOptionItems());
            localOptionItems.Add(typeof(ModelOptionItems).Name, new ModelOptionItems());
            localOptionItems.Add(typeof(ModelFieldsOptionItems).Name, new ModelFieldsOptionItems());
            localOptionItems.Add(typeof(ComponentOptionItems).Name, new ComponentOptionItems());
            localOptionItems.Add(typeof(ContainerOptionItems).Name, new ContainerOptionItems());
        }

        public async Task<IServiceResult<IEnumerable<OptionItem>>> GetOptionItemsAsync(List<DynamicArg> dynamicArgs)
        {
            var optionsCode = dynamicArgs.Single(a => a.Name.Equals(OPTIONS_CODE)).Value.ToString();
            var optionsArgs = dynamicArgs.Where(a => a.Name.Equals(OPTIONS_CODE));
            var args = ComponentArgHelper.GetArgs(optionsArgs);

            if (localOptionItems.ContainsKey(optionsCode))
            {
                return new ServiceResult<IEnumerable<OptionItem>>
                {
                    IsSuccess = true,
                    Result = await localOptionItems[optionsCode].GetOptionItemsAsync(args)
                };
            }

            string jsonArgs = JsonSerializer.Serialize(args);

            var httpResponseMessage = await httpClient.GetAsync($"Options/{optionsCode}/{jsonArgs}")
                .ConfigureAwait(false);

            return await GetServiceResultAsync<IEnumerable<OptionItem>>(httpResponseMessage)
                .ConfigureAwait(false);
        }
    }
}
