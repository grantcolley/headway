using Headway.Core.Enums;
using Headway.Core.Interface;
using Headway.Core.Model;

namespace Headway.Core.Tests.Helpers
{
    public class StateHelper : IConfigureState
    {
        public void Configure(State state)
        {
            state.Context = null;
            state.StateActions.Add(new StateAction { Order = 4, StateActionType = StateActionType.Initialize, ActionAsync = StateAction });
            state.StateActions.Add(new StateAction { Order = 3, StateActionType = StateActionType.Initialize, ActionAsync = StateAction });
        }

        private Task StateAction(State state, StateActionType stateActionType, int order)
        {
            if (state.Context == null)
            {
                state.Context = $"{order} {stateActionType} {state.StateCode}";
            }
            else
            {
                state.Context += $"; {order} {stateActionType} {state.StateCode}";
            }

            return Task.CompletedTask;
        }
    }
}