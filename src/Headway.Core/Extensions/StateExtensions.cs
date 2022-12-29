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
        private static readonly IDictionary<string, StateConfiguration> stateConfigurationCache = new Dictionary<string, StateConfiguration>();
        private static object stateConfigurationCacheLock = new object();

        public static async Task InitialiseAsync(this State state)
        {
            if(state.StateStatus.Equals(StateStatus.InProgress)) 
            {
                throw new StateException(state, $"Can't initialize {state.StateStatus} because it's already {StateStatus.InProgress}.");
            }

            await state.ExecuteActionsAsync(StateActionType.Initialize).ConfigureAwait(false);

            state.StateStatus = StateStatus.InProgress;

            state.Flow.History.RecordInitialise(state);

            if (state.SubStates.Any())
            {
                var subState = state.SubStates.FirstState();

                await subState.InitialiseAsync().ConfigureAwait(false);
            }

            if (!state.SubStates.Any())
            {
                if(state.Flow.ActiveState != state)
                {
                    state.Flow.ActiveState = state;
                }
            }

            if(state.StateType.Equals(StateType.Auto))
            {
                await state.CompleteAsync().ConfigureAwait(false);
            }
        }

        public static async Task CompleteAsync(this State state, string transitionStateCode = "")
        {
            if (state.StateStatus.Equals(StateStatus.Completed))
            {
                throw new StateException(state, $"Can't complete {state.StateCode} because it's already {StateStatus.Completed}.");
            }

            if (state.StateStatus.Equals(StateStatus.NotStarted))
            {
                throw new StateException(state, $"Can't complete {state.StateCode} because it's still {StateStatus.NotStarted}.");
            }

            var uncompletedSubStates = state.SubStates.Where(s => s.StateStatus != StateStatus.Completed).ToList();

            if(uncompletedSubStates.Any())
            {
                var uncompletedSubStateDescriptions = uncompletedSubStates.Select(s => $"{s.StateCode}={s.StateStatus}");
                var joinedDescriptions = string.Join(",", uncompletedSubStateDescriptions);
                throw new StateException(state, $"Can't complete {state.StateCode} because sub states not yet {StateStatus.Completed} : {joinedDescriptions}.");
            }

            if (!string.IsNullOrWhiteSpace(transitionStateCode)
                && !state.Transitions.Any(s => s.StateCode.Equals(transitionStateCode)))
            {
                throw new StateException(state, $"Can't complete {state.StateCode} because it doesn't support transitioning to {transitionStateCode}.");
            }

            await state.ExecuteActionsAsync(StateActionType.Completed).ConfigureAwait(false);

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
                    throw new StateException(state, $"Can't complete {state.StateCode} because it doesn't support transitioning to {transitionStateCode}.");
                }
            }

            state.StateStatus = StateStatus.Completed;

            state.Flow.History.RecordCompleted(state);

            if (transitionState != null)
            {
                await transitionState.InitialiseAsync().ConfigureAwait(false);
            }
            else
            {
                await state.ParentState.CompleteAsync().ConfigureAwait(false);
            }
        }

        public static async Task ResetAsync(this State state, string resetStateCode = "")
        {
            if (!string.IsNullOrWhiteSpace(resetStateCode)
                && !state.Transitions.Any(s => s.StateCode.Equals(resetStateCode))
                && !state.Flow.History.Any(h => h.StateCode.Equals(resetStateCode)))
            {
                throw new StateException(state, $"Can't reset {state.StateCode} because it doesn't support resetting back to {resetStateCode}.");
            }

            state.StateStatus = default;
            state.Comment = default;
            state.Owner = default;

            await state.ExecuteActionsAsync(StateActionType.Reset).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(resetStateCode))
            {
                if (!state.Transitions.Any(s => s.StateCode.Equals(resetStateCode))
                    && !state.Flow.History.Any(h => h.StateCode.Equals(resetStateCode)))
                {
                    throw new StateException(state, $"Can't reset {state.StateCode} because it doesn't support resetting back to {resetStateCode}.");
                }

                var resetState = state.Transitions.First(s => s.StateCode.Equals(resetStateCode));

                if(resetState.Position > state.Position)
                {
                    throw new StateException(state, $"Can't reset to {resetState.StateCode} (position {resetState.Position}) because it is positioned after {state.StateCode} (position {state.Position}).");
                }

                var stateHistory = state.Flow.History;
                var stateDictionary = state.Flow.StateDictionary;
                var historyIndex = state.Flow.History.Count - 1;

                for (int i = historyIndex; i >= 0; i--)
                {
                    var rs = stateDictionary[stateHistory[i].StateCode];
                    await rs.ResetAsync().ConfigureAwait(false);
                }

                await resetState.InitialiseAsync().ConfigureAwait(false);
            }
        }

        public static async Task ExecuteActionsAsync(this State state, StateActionType stateFunctionType)
        {
            if (state.StateActions == null)
            {
                return;
            }

            if (!state.Configured)
            {
                state.Configure();
            }

            var actions = state.StateActions
                .Where(a => a.StateActionType.Equals(stateFunctionType))
                .OrderBy(a => a.Order)
                .ToList();

            foreach (var action in actions)
            {
                await action.ActionAsync(state, action.StateActionType, action.Order).ConfigureAwait(false);
            }
        }

        public static State FirstState(this List<State> states)
        {
            var firstPosition = states.Min(s => s.Position);

            return states.First(s => s.Position.Equals(firstPosition));
        }

        public static void Configure(this State state)
        {
            if(state.Configured)
            {
                throw new StateException(state, $"{state.StateCode} has already been configured.");
            }

            if (!string.IsNullOrWhiteSpace(state.ConfigureStateClass))
            {
                StateConfiguration stateConfiguration = null;

                lock (stateConfigurationCacheLock)
                {
                    if (stateConfigurationCache.TryGetValue(
                        state.ConfigureStateClass, out StateConfiguration existingStateConfiguration))
                    {
                        stateConfiguration = existingStateConfiguration;
                    }
                    else
                    {
                        var type = Type.GetType(state.ConfigureStateClass);

                        if (type == null)
                        {
                            throw new StateException(state, $"Can't resolve {state.ConfigureStateClass}");
                        }

                        var instance = (IConfigureState)Activator.CreateInstance(type);

                        var methodInfo = type.GetMethod("Configure");

                        stateConfiguration = new StateConfiguration
                        {
                            Instance = instance,
                            MethodInfo = methodInfo
                        };

                        stateConfigurationCache.Add(state.ConfigureStateClass, stateConfiguration);
                    }
                }

                stateConfiguration.Configure(state);

                state.Configured = true;
            }
        }
    }
}