using Headway.Core.Enums;
using Headway.Core.Interface;
using System;
using System.Linq;

namespace Headway.Core.Extensions
{
    public static class FlowContextExtensions
    {
        public static bool IsStateReadOnly(this IFlowContext flowContext, string stateCode)
        {
            bool readOnly = false;

            if (!string.IsNullOrWhiteSpace(stateCode))
            {
                var state = flowContext.Flow.StateDictionary[stateCode];

                var flowHistory = flowContext.Flow.ReplayHistory.FirstOrDefault(f => f.StateCode.Equals(stateCode));

                if (flowHistory == null
                    && state.IsOwnerRestricted)
                {
                    readOnly = true;
                }
                else if (flowHistory != null)
                {
                    if (state.IsOwnerRestricted
                        && flowHistory.StateStatus.Equals(StateStatus.InProgress)
                        && !string.IsNullOrWhiteSpace(flowHistory.Owner)
                        && flowHistory.Owner.Equals(flowContext.Authorisation.User))
                    {
                        readOnly = true;
                    }
                    else if (!state.IsOwnerRestricted
                        && flowHistory.StateStatus.Equals(StateStatus.InProgress))
                    {
                        readOnly = true;
                    }
                }
            }

            return readOnly;
        }

        public static IFlowContext Execute(this IFlowContext flowContext, IFlowContext target)
        {
            throw new NotImplementedException();
        }
    }
}
