using Headway.Core.Args;
using Headway.Core.Exceptions;
using Headway.Core.Extensions;
using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Options
{
    public class DynamicArgStatesOptionTextItems : IOptionDynamicArgTextItems
    {
        public Task<IEnumerable<string>> GetOptionDynamicArgTextItemsAsync(IEnumerable<DynamicArg> dynamicArgs)
        {
            var isLocalDynamicArg = dynamicArgs.GetDynamicArgOrDefault(Constants.Args.IS_LOCAL_DYNAMICARG_OPTION);

            if(isLocalDynamicArg == null
                || string.IsNullOrWhiteSpace(isLocalDynamicArg.Value.ToString())
                || !isLocalDynamicArg.Value.Equals(Constants.Args.TRUE)) 
            {
                throw new HeadwayArgsException($"{Constants.Args.IS_LOCAL_DYNAMICARG_OPTION} must be {Constants.Args.TRUE}");
            }

            var statesArg = dynamicArgs.GetDynamicArgOrDefault(Constants.Args.LINK_VALUE);

            var states = statesArg.Value as IEnumerable<State>;
            
            if(states == null)
            {
                return Task.FromResult((new List<string> { string.Empty }).AsEnumerable());
            }

            List<string> optionItems= new();

            optionItems.AddRange(states.Select(s => s.StateCode));

            return Task.FromResult(optionItems.AsEnumerable());
        }
    }
}
