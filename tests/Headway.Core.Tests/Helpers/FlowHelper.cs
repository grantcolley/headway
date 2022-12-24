using Headway.Core.Enums;
using Headway.Core.Extensions;
using Headway.Core.Interface;
using Headway.Core.Model;

namespace Headway.Core.Tests.Helpers
{
    public class FlowHelper : IConfigureFlow
    {
        public static Flow CreateFlow(int numberOfStates)
        {
            var flow = new Flow { Name = "Test Flow" };

            for (int i = 1; i < numberOfStates + 1; i++)
            {
                flow.States.Add(new State { Position = i * 10, StateCode = $"State {i}", Flow = flow });
            }

            return flow;
        }

        public void Configure(Flow flow)
        {
            var states = flow.StateDictionary.Values.ToList();
            var state = states.FirstState();

            state.Context = null;
            state.StateActions.Add(new StateAction { Order = 2, StateActionType = StateActionType.Initialize, ActionAsync = StateAction });
            state.StateActions.Add(new StateAction { Order = 1, StateActionType = StateActionType.Initialize, ActionAsync = StateAction });
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
