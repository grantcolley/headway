using Headway.Core.Enums;
using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Linq;

namespace Headway.Core.Extensions
{
    public static class FlowContextExtensions
    {
        public static IFlowContext Execute(this IFlowContext flowContext, IFlowContext target)
        {
            throw new NotImplementedException();
        }

        public static bool ValidateActiveStateReadOnly(this IFlowContext flowContext)
        {
            if (flowContext == null)
            {
                throw new ArgumentNullException(nameof(flowContext));
            }

            if (flowContext.Flow == null)
            {
                throw new ArgumentNullException(nameof(flowContext.Flow));
            }

            if (flowContext.Flow?.ActiveState != null)
            {
                return flowContext.IsStateReadOnly(flowContext.Flow?.ActiveState);
            }

            flowContext.Flow.Bootstrap(flowContext.GetFlowHistory(), true);

            return flowContext.IsStateReadOnly(flowContext.Flow?.ActiveState);
        }

        public static bool IsStateReadOnly(this IFlowContext flowContext, string stateCode)
        {
            if (string.IsNullOrWhiteSpace(stateCode))
            {
                return true;
            }

            var state = flowContext.Flow.StateDictionary[stateCode];

            var flowHistory = flowContext.Flow.ReplayHistory.FirstOrDefault(f => f.StateCode.Equals(stateCode));

            if (flowHistory == null)
            {
                return true;
            }

            return flowContext.IsStateReadOnly(state);
        }

        private static bool IsStateReadOnly(this IFlowContext flowContext, State state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (state.IsOwnerRestricted
                && !string.IsNullOrWhiteSpace(state.Owner)
                && state.StateStatus.Equals(StateStatus.InProgress)
                && state.Owner.Equals(flowContext.Authorisation.User))
            {
                return false;
            }

            return true;
        }
    }
}
