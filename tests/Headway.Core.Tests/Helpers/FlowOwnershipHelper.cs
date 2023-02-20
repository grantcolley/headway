using Headway.Core.Enums;
using Headway.Core.Interface;
using Headway.Core.Model;

namespace Headway.Core.Tests.Helpers
{
    public class FlowOwnershipHelper : IConfigureFlow
    {
        public void Configure(Flow flow)
        {
            foreach(var state in flow.States)
            {
                state.StateActions.Add(new StateAction { Order = 1, StateActionType = StateActionType.Initialize, ActionAsync = SetOwnership });
                state.StateActions.Add(new StateAction { Order = 2, StateActionType = StateActionType.Start, ActionAsync = SetOwnership });
                state.StateActions.Add(new StateAction { Order = 3, StateActionType = StateActionType.Complete, ActionAsync = SetOwnership });
                state.StateActions.Add(new StateAction { Order = 4, StateActionType = StateActionType.Reset, ActionAsync = SetOwnership });
            }
        }

        private Task SetOwnership(State state, StateActionType stateActionType, int order)
        {
            if(state.StateType.Equals(StateType.Standard))
            {
                state.Owner = Environment.UserName;
                state.Comment = $"{state.StateType} {state.StateCode} {stateActionType} by {state.Owner}";
            }
            else if (state.StateType.Equals(StateType.Auto)
                || state.StateType.Equals(StateType.Parent))
            {
                state.Owner = "system_account";
                state.Comment = $"{state.StateType} {state.StateCode} {stateActionType} by {state.Owner}";
            }

            return Task.CompletedTask;
        }
    }
}
