using Headway.Core.Enums;
using Headway.Core.Interface;
using Headway.Core.Model;

namespace Headway.Core.Tests.Helpers
{
    public class StateHelper : IConfigureState
    {
        public void Configure(State state)
        {
            state.Comment = default;
            state.StateActions.Add(new StateAction { Order = 2, StateActionType = StateActionType.Completed, ActionAsync = StateAction });
            state.StateActions.Add(new StateAction { Order = 1, StateActionType = StateActionType.Completed, ActionAsync = StateAction });
            state.StateActions.Add(new StateAction { Order = 4, StateActionType = StateActionType.Initialize, ActionAsync = StateAction });
            state.StateActions.Add(new StateAction { Order = 3, StateActionType = StateActionType.Initialize, ActionAsync = StateAction });
            state.StateActions.Add(new StateAction { Order = 6, StateActionType = StateActionType.Reset, ActionAsync = StateAction });
            state.StateActions.Add(new StateAction { Order = 5, StateActionType = StateActionType.Reset, ActionAsync = StateAction });
        }

        private Task StateAction(State state, StateActionType stateActionType, int order)
        {
            if (state.Comment == null)
            {
                state.Comment = $"{order} {stateActionType} {state.StateCode}";
            }
            else
            {
                state.Comment += $"; {order} {stateActionType} {state.StateCode}";
            }

            return Task.CompletedTask;
        }
    }
}