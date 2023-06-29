using Headway.Core.Args;
using Headway.Core.Enums;
using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Extensions
{
    public static class FlowContextExtensions
    {
        public static async Task ExecuteAsync(this IFlowContext flowContext, FlowExecutionArgs flowExecutionArgs)
        {
            flowContext.Flow.Bootstrap(flowContext.GetFlowHistory());

            switch(flowExecutionArgs.FlowAction)
            {
                case FlowActionEnum.Initialise:
                    await flowContext.Flow.ActiveState.InitialiseAsync().ConfigureAwait(false);
                    break;
                case FlowActionEnum.Start:
                    await flowContext.Flow.ActiveState.StartAsync().ConfigureAwait(false);
                    break;
                case FlowActionEnum.TakeOwnership:
                    await flowContext.Flow.ActiveState.TakeOwnershipAsync(flowExecutionArgs.Authorisation).ConfigureAwait(false);
                    break;
                case FlowActionEnum.RelinquishOwnership:
                    flowContext.Flow.ActiveState.RelinquishOwnership(flowExecutionArgs.Authorisation);
                    break;
                case FlowActionEnum.Complete:
                    await flowContext.Flow.ActiveState.CompleteAsync().ConfigureAwait(false);
                    break;
                case FlowActionEnum.Reset:
                    await flowContext.Flow.ActiveState.ResetAsync().ConfigureAwait(false);
                    break;
                default: 
                    throw new ArgumentException(nameof(FlowActionEnum.Unknown));
            }
        }

        public static bool IsActiveStateForReadOnlyForValidator(this IFlowContext flowContext)
        {
            if (flowContext == null)
            {
                throw new ArgumentNullException(nameof(flowContext));
            }

            if (flowContext.Flow == null)
            {
                throw new ArgumentNullException(nameof(flowContext.Flow));
            }

            if (flowContext.Flow?.ActiveState == null)
            {
                flowContext.Flow.Bootstrap(flowContext.GetFlowHistory(), true);
            }

            return flowContext.IsActiveStateReadOnly();
        }

        public static bool IsActiveStateReadOnly(this IFlowContext flowContext)
        {
            if (flowContext == null)
            {
                throw new ArgumentNullException(nameof(flowContext));
            }

            if (flowContext.Flow == null)
            {
                throw new ArgumentNullException(nameof(flowContext.Flow));
            }

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
