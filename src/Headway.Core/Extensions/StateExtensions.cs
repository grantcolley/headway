using Headway.Core.Enums;
using Headway.Core.Exceptions;
using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Core.Extensions
{
    public static class StateExtensions
    {
        public static async Task InitialiseAsync(this State state)
        {
            if(state.StateStatus.Equals(StateStatus.InProgress)) 
            {
                throw new InvalidOperationException($"Can't initialize {state.StateStatus} because it's already {StateStatus.InProgress}");
            }

            await state.ExecuteActionsAsync(StateActionType.Initialize).ConfigureAwait(false);

            if (state.SubStates.Any()) 
            {
                var subState = state.SubStates.FirstState();

                await subState.InitialiseAsync().ConfigureAwait(false);
            }

            state.StateStatus = StateStatus.InProgress;

            if(!state.SubStates.Any())
            {
                if(state.Flow.ActiveState != state)
                {
                    state.Flow.ActiveState = state;
                }
            }
        }

        public static async Task CompleteAsync(this State state, string transitionStateCode = "")
        {
            if (state.StateStatus.Equals(StateStatus.Completed))
            {
                return;
            }

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

            state.SetupStateActions();

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
            if (state.StateStatus.Equals(StateStatus.NotStarted))
            {
                return;
            }

            await state.ExecuteActionsAsync(StateActionType.Reset).ConfigureAwait(false);

            state.StateStatus = StateStatus.NotStarted;
        }

        public static State FirstState(this List<State> states)
        {
            var firstPosition = states.Min(s => s.Position);

            return states.First(s => s.Position.Equals(firstPosition));
        }

        public static List<State> GetStates(this Dictionary<string, State> dictionary, List<string> stateCodes)
        {
            if(stateCodes == null)
            {
                throw new ArgumentNullException(nameof(stateCodes));
            }

            var states = new List<State>();

            foreach (var stateCode in stateCodes)
            {
                states.Add(dictionary[stateCode]);
            }

            return states;
        }

        public static void SetupFlowActions(this Flow flow)
        {
            if (!string.IsNullOrWhiteSpace(flow.ActionSetupClass))
            {
                var type = Type.GetType(flow.ActionSetupClass);

                if (type == null)
                {
                    throw new ArgumentNullException(nameof(flow.ActionSetupClass));
                }

                var instance = (ISetupFlowActions)Activator.CreateInstance(type);

                var method = type.GetMethod("SetupActions");

                method.Invoke(instance, new object[] { flow.StateDictionary });
            }
        }

        public static void SetupStateActions(this State state)
        {
            if (!string.IsNullOrWhiteSpace(state.ActionSetupClass))
            {
                var type = Type.GetType(state.ActionSetupClass);

                if (type == null)
                {
                    throw new ArgumentNullException(nameof(state.ActionSetupClass));
                }

                var instance = (ISetupStateActions)Activator.CreateInstance(type);

                var method = type.GetMethod("SetupActions");

                method.Invoke(instance, new object[] { state });
            }
        }
    }
}