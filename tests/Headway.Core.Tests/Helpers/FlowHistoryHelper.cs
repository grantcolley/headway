using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Core.Interface;
using Headway.Core.Model;

namespace Headway.Core.Tests.Helpers
{
    [FlowConfiguration]
    public class FlowHistoryHelper : IConfigureFlow
    {
        public void Configure(Flow flow)
        {
            foreach(var state in flow.States)
            {
                state.Comment = default;
                state.StateActions.Add(new StateAction { StateActionType = StateActionType.Start, ActionAsync = StateAction });
                state.StateActions.Add(new StateAction { StateActionType = StateActionType.Complete, ActionAsync = StateAction });
                state.StateActions.Add(new StateAction { StateActionType = StateActionType.Reset, ActionAsync = StateAction });
            }
        }

        private Task StateAction(State state, StateActionType stateActionType, int order)
        {
            state.Owner = Environment.UserName;
            state.Comment = $"{stateActionType} {state.StateCode}";

            if(stateActionType.Equals(StateActionType.Complete))
            {
                if(state.StateCode.Equals("REDRESS_VALIDATION"))
                {
                    state.TransitionStateCode = "PAYMENT_GENERATION";
                }
            }

            if(stateActionType.Equals(StateActionType.Reset))
            {
                if(state.StateCode.Equals("PAYMENT_GENERATION"))
                {
                    state.RegressionStateCode = "REDRESS_REVIEW";
                }
            }

            return Task.CompletedTask;
        }
    }
}
