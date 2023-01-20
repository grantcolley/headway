using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Core.Extensions;
using Headway.Core.Interface;
using Headway.Core.Model;

namespace Headway.Core.Tests.Helpers
{
    [FlowConfiguration]
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
            var state = flow.States.FirstState();

            state.Comment = default;
            state.StateActions.Add(new StateAction { Order = 2, StateActionType = StateActionType.Initialize, ActionAsync = StateAction });
            state.StateActions.Add(new StateAction { Order = 1, StateActionType = StateActionType.Initialize, ActionAsync = StateAction });
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
