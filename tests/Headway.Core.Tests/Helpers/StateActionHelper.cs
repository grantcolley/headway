using Headway.Core.Enums;
using Headway.Core.Interface;
using Headway.Core.Model;

namespace Headway.Core.Tests.Helpers
{
    public class StateActionHelper : ISetupStateActions
    {
        public void SetupActions(State state)
        {
            static Task action(State state, StateActionType stateActionType, int order)
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

            state.Context = null;
            state.StateActions.Add(new StateAction { Order = 4, StateActionType = StateActionType.Initialize, ActionAsync = action });
            state.StateActions.Add(new StateAction { Order = 3, StateActionType = StateActionType.Initialize, ActionAsync = action });
        }
    }
}