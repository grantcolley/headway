using Headway.Core.Enums;
using Headway.Core.Exceptions;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Extensions
{
    public static class StateExtensions
    {
        public static async Task InitialiseAsync(this State state)
        {
            var subState = state.SubStates.FirstState();

            await subState.InitialiseAsync().ConfigureAwait(false);

            await state.ExecuteActionsAsync(StateActionType.Initialize).ConfigureAwait(false);

            state.StateStatus = StateStatus.InProgress;
        }

        public static State FirstState(this List<State> states) 
        {
            var firstPosition = states.Min(s => s.Position);

            return states.First(s => s.Position.Equals(firstPosition));
        }

        public static async Task CompleteAsync(this State state, string transitionStateCode = "")
        {
            var uncompletedSubStates = state.SubStates.Where(s => s.StateStatus != StateStatus.Completed).ToList();

            if(uncompletedSubStates.Any())
            {
                var uncompletedSubStateDescriptions = uncompletedSubStates.Select(s => $"{s.StateCode}={s.StateStatus}");
                var joinedDescriptions = string.Join(",", uncompletedSubStateDescriptions);
                throw new FlowException(state, $"{state.StateCode} not all states have completed {joinedDescriptions}");
            }

            State transitionState = null;

            if (string.IsNullOrWhiteSpace(transitionStateCode))
            {
                transitionState = state.Transitions.FirstOrDefault();
            }
            else
            {
                transitionState = state.Transitions.FirstOrDefault(s => s.StateCode.Equals(transitionStateCode));

                if (transitionState == null)
                {
                    throw new FlowException(state, $"{state.StateCode} transition doesn't exist {transitionStateCode}");
                }
            }

            await state.ExecuteActionsAsync(StateActionType.Complete).ConfigureAwait(false);

            state.StateStatus = StateStatus.Completed;

            if(transitionState != null)
            {
                await transitionState.InitialiseAsync().ConfigureAwait(false);
            }
            else
            {
                await state.ParentState.CompleteAsync().ConfigureAwait(false); ;
            }
        }

        public static async Task ResestAsync(this State state)
        {
            await state.ExecuteActionsAsync(StateActionType.Reset).ConfigureAwait(false);

            state.StateStatus = StateStatus.NotStarted;
        }

        public static List<State> GetStates(this Dictionary<string, State> dictionary, List<string> stateCodes)
        {
            var states = new List<State>();

            foreach (var stateCode in stateCodes)
            {
                states.Add(dictionary[stateCode]);
            }

            return states;
        }
    }
}