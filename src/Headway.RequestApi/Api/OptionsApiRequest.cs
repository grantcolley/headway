using Headway.Core.Args;
using Headway.Core.Constants;
using Headway.Core.Extensions;
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
    public class OptionsApiRequest : LogApiRequest, IOptionsApiRequest
    {
        private readonly Dictionary<string, IOptionItems> localOptionItems = new();
        private readonly Dictionary<string, IOptionTextItems> localOptionTextItems = new();
        private readonly Dictionary<string, IOptionCheckItems> localOptionCheckItems = new();
        private readonly Dictionary<string, IOptionDynamicArgTextItems> localDynamicArgOptionTextItems = new();

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
            localOptionItems.Add(typeof(SearchComponentOptionItems).Name, new SearchComponentOptionItems());
            localOptionItems.Add(typeof(SearchItemComponentOptionItems).Name, new SearchItemComponentOptionItems());
            localOptionItems.Add(typeof(DocumentOptionItems).Name, new DocumentOptionItems());
            localOptionItems.Add(typeof(ContainerOptionItems).Name, new ContainerOptionItems());
            localOptionItems.Add(typeof(StaticOptionItems).Name, new StaticOptionItems());
            localOptionItems.Add(typeof(EnumNamesOptionItems).Name, new EnumNamesOptionItems());
            localOptionItems.Add(typeof(FlowConfigurationOptionItems).Name, new FlowConfigurationOptionItems());
            localOptionItems.Add(typeof(StateConfigurationOptionItems).Name, new StateConfigurationOptionItems());

            localOptionCheckItems.Add(typeof(ModelFieldsOptionCheckItems).Name, new ModelFieldsOptionCheckItems());

            localOptionTextItems.Add(typeof(ModelFieldsOptionTextItems).Name, new ModelFieldsOptionTextItems());

            localDynamicArgOptionTextItems.Add(typeof(DynamicArgStatesOptionTextItems).Name, new DynamicArgStatesOptionTextItems());
        }

        public async Task<IResponse<IEnumerable<string>>> GetOptionTextItemsAsync(List<DynamicArg> dynamicArgs)
        {
            var isLocalDynamicArgOption = dynamicArgs.Any(da => da.Name.Equals(Args.IS_LOCAL_DYNAMICARG_OPTION) 
                                                                && da.Value.Equals(Args.TRUE));
            if(isLocalDynamicArgOption)
            {
                return await GetLocalDynamicArgOptionTextItemsAsync(dynamicArgs)
                    .ConfigureAwait(false);
            }

            var args = ComponentArgHelper.GetArgs(dynamicArgs);
            return await GetOptionTextItemsAsync(args);
        }

        public async Task<IResponse<IEnumerable<string>>> GetOptionTextItemsAsync(List<Arg> args)
        {
            var optionsCode = args.ArgValue(Options.OPTIONS_CODE);

            if (localOptionTextItems.ContainsKey(optionsCode))
            {
                return new Response<IEnumerable<string>>
                {
                    IsSuccess = true,
                    Result = await localOptionTextItems[optionsCode].GetOptionTextItemsAsync(args)
                };
            }

            using var httpResponseMessage = await httpClient.PostAsJsonAsync(Controllers.OPTIONS_TEXTOPTIONS, args)
                .ConfigureAwait(false);

            return await GetResponseAsync<IEnumerable<string>>(httpResponseMessage)
                .ConfigureAwait(false);
        }

        public async Task<IResponse<IEnumerable<OptionCheckItem>>> GetOptionCheckItemsAsync(List<DynamicArg> dynamicArgs)
        {
            var args = ComponentArgHelper.GetArgs(dynamicArgs);
            return await GetOptionCheckItemsAsync(args);
        }

        public async Task<IResponse<IEnumerable<OptionCheckItem>>> GetOptionCheckItemsAsync(List<Arg> args)
        {
            var optionsCode = args.ArgValue(Options.OPTIONS_CODE);

            if (localOptionCheckItems.ContainsKey(optionsCode))
            {
                return new Response<IEnumerable<OptionCheckItem>>
                {
                    IsSuccess = true,
                    Result = await localOptionCheckItems[optionsCode].GetOptionCheckItemsAsync(args)
                };
            }

            using var httpResponseMessage = await httpClient.PostAsJsonAsync(Controllers.OPTIONS_CHECKOPTIONS, args)
                .ConfigureAwait(false);

            return await GetResponseAsync<IEnumerable<OptionCheckItem>>(httpResponseMessage)
                .ConfigureAwait(false);
        }

        public async Task<IResponse<IEnumerable<OptionItem>>> GetOptionItemsAsync(List<DynamicArg> dynamicArgs)
        {
            var args = ComponentArgHelper.GetArgs(dynamicArgs);
            return await GetOptionItemsAsync(args);
        }

        public async Task<IResponse<IEnumerable<OptionItem>>> GetOptionItemsAsync(List<Arg> args)
        {
            var optionsCode = args.ArgValue(Options.OPTIONS_CODE);

            if (localOptionItems.ContainsKey(optionsCode))
            {
                return new Response<IEnumerable<OptionItem>>
                {
                    IsSuccess = true,
                    Result = await localOptionItems[optionsCode].GetOptionItemsAsync(args)
                };
            }

            using var httpResponseMessage = await httpClient.PostAsJsonAsync(Controllers.OPTIONS, args)
                .ConfigureAwait(false);

            return await GetResponseAsync<IEnumerable<OptionItem>>(httpResponseMessage)
                .ConfigureAwait(false);
        }

        public async Task<IResponse<IEnumerable<T>>> GetOptionItemsAsync<T>(List<DynamicArg> dynamicArgs)
        {
            var args = ComponentArgHelper.GetArgs(dynamicArgs);

            using var httpResponseMessage = await httpClient.PostAsJsonAsync(Controllers.OPTIONS_COMPLEXOPTIONS, args)
                .ConfigureAwait(false);

            return await GetResponseAsync<IEnumerable<T>>(httpResponseMessage)
                .ConfigureAwait(false);
        }

        private async Task<IResponse<IEnumerable<string>>> GetLocalDynamicArgOptionTextItemsAsync(List<DynamicArg> dynamicArgs)
        {
            var optionsCode = dynamicArgs.DynamicArgValueToString(Options.OPTIONS_CODE);

            if (localDynamicArgOptionTextItems.ContainsKey(optionsCode))
            {
                return new Response<IEnumerable<string>>
                {
                    IsSuccess = true,
                    Result = await localDynamicArgOptionTextItems[optionsCode].GetOptionDynamicArgTextItemsAsync(dynamicArgs)
                };
            }
            else
            {
                throw new KeyNotFoundException(optionsCode);
            }
        }
    }
}
