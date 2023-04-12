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
            if(state.StateStatus.Equals(StateStatus.Initialized)
                || state.StateStatus.Equals(StateStatus.InProgress)) 
            {
                throw new StateException(state, $"Can't initialize {state.StateCode} because it's {state.StateStatus}.");
            }

            if (state.StateType.Equals(StateType.Parent)
                && !state.SubStates.Any())
            {
                throw new StateException(state, $"{state.StateCode} is configured as a {StateType.Parent} but has no sub states.");
            }

            await state.ExecuteActionsAsync(StateActionType.Initialize).ConfigureAwait(false);

            state.StateStatus = StateStatus.Initialized;

            state.Flow.History.RecordInitialise(state);

            if (state.Flow.ActiveState != state)
            {
                state.Flow.ActiveState = state;
            }

            if (state.Equals(state.Flow.RootState)
                && !state.Flow.FlowStatus.Equals(FlowStatus.InProgress))
            {
                state.Flow.FlowStatus = FlowStatus.InProgress;
            }

            if (!state.IsOwnerRestricted
                || (state.IsOwnerRestricted
                && !string.IsNullOrWhiteSpace(state.Owner)))
            {
                await state.StartAsync().ConfigureAwait(false);
            }
        }

        public static async Task StartAsync(this State state)
        {
            if (!state.Flow.ActiveState.Equals(state))
            {
                throw new StateException(state, $"Can't start {state.StateCode} because it's not the active state in {state.Flow.Name}.");
            }

            if(state.StateStatus.Equals(StateStatus.Uninitialized)
                || state.StateStatus.Equals(StateStatus.InProgress)
                || state.StateStatus.Equals(StateStatus.Completed))
            {
                throw new StateException(state, $"Can't start {state.StateCode} because it's {state.StateStatus}.");
            }

            if(state.IsOwnerRestricted
                && string.IsNullOrWhiteSpace(state.Owner))
            {
                throw new StateException(state, $"Can't start owner restricted state {state.StateCode} without an owner.");
            }

            await state.ExecuteActionsAsync(StateActionType.Start).ConfigureAwait(false);

            state.StateStatus = StateStatus.InProgress;

            state.Flow.History.RecordStart(state);

            if (state.StateType.Equals(StateType.Parent))
            {
                var subState = state.SubStates.FirstState();

                await subState.InitialiseAsync().ConfigureAwait(false);
            }
            else if (state.StateType.Equals(StateType.Auto))
            {
                if (state.AutoActionResult.Equals(StateAutoActionResult.AutoComplete))
                {
                    await state.CompleteAsync().ConfigureAwait(false);
                }
                else if (state.AutoActionResult.Equals(StateAutoActionResult.AutoRegress))
                {
                    await state.ResetAsync().ConfigureAwait(false);
                }
            }
        }

        public static async Task CompleteAsync(this State state, string transitionStateCode = "")
        {
            if (state.StateStatus.Equals(StateStatus.Completed)
                || state.StateStatus.Equals(StateStatus.Initialized)
                || state.StateStatus.Equals(StateStatus.Uninitialized))
            {
                throw new StateException(state, $"Can't complete {state.StateCode} because it's {state.StateStatus}.");
            }

            var uncompletedSubStates = state.SubStates.Where(s => s.StateStatus != StateStatus.Completed).ToList();

            if(uncompletedSubStates.Any())
            {
                var uncompletedSubStateDescriptions = uncompletedSubStates.Select(s => $"{s.StateCode}={s.StateStatus}");
                var joinedDescriptions = string.Join(",", uncompletedSubStateDescriptions);
                throw new StateException(state, $"Can't complete {state.StateCode} because sub states not yet {StateStatus.Completed} : {joinedDescriptions}.");
            }

            if (!string.IsNullOrWhiteSpace(transitionStateCode))
            {
                state.TransitionStateCode = transitionStateCode;
            }

            if (!string.IsNullOrWhiteSpace(state.TransitionStateCode)
                && !state.Transitions.Any(s => s.StateCode.Equals(state.TransitionStateCode)))
            {
                throw new StateException(state, $"Can't complete {state.StateCode} because it doesn't support transitioning to {state.TransitionStateCode}.");
            }

            await state.ExecuteActionsAsync(StateActionType.Complete).ConfigureAwait(false);

            State transitionState = null;

            if (string.IsNullOrWhiteSpace(state.TransitionStateCode))
            {
                transitionState = state.Transitions.FirstOrDefault();
            }
            else
            {
                transitionState = state.Transitions.FirstOrDefault(s => s.StateCode.Equals(state.TransitionStateCode));

                if (transitionState == null)
                {
                    throw new StateException(state, $"Can't complete {state.StateCode} because it doesn't support transitioning to {transitionStateCode}.");
                }
            }

            state.StateStatus = StateStatus.Completed;

            state.Flow.History.RecordComplete(state);

            if (transitionState != null)
            {
                await transitionState.InitialiseAsync().ConfigureAwait(false);
            }
            else
            {
                if (state.ParentState != null)
                {
                    await state.ParentState.CompleteAsync().ConfigureAwait(false);
                }
            }

            if (state.Equals(state.Flow.FinalState))
            {
                state.Flow.FlowStatus = FlowStatus.Completed;
            }
        }

        public static async Task ResetAsync(this State state, string regressStateCode = "")
        {
            if(string.IsNullOrWhiteSpace(regressStateCode)
                && state.StateStatus.Equals(StateStatus.Uninitialized))
            {
                throw new StateException(state, $"Can't reset {state.StateCode} because it is {StateStatus.Uninitialized}.");
            }

            if (!string.IsNullOrWhiteSpace(regressStateCode))
            {
                state.RegressionStateCode = regressStateCode;
            }

            if (!string.IsNullOrWhiteSpace(state.RegressionStateCode)
                && (!state.Regressions.Any(s => s.StateCode.Equals(state.RegressionStateCode))
                || !state.Flow.History.Any(h => h.StateCode.Equals(state.RegressionStateCode))))
            {
                throw new StateException(state, $"Can't reset {state.StateCode} because it doesn't support regressing back to {regressStateCode}.");
            }

            await state.ExecuteActionsAsync(StateActionType.Reset).ConfigureAwait(false);

            state.StateStatus = default;

            state.Flow.History.RecordReset(state);

            state.Owner = default;
            state.Comment = default;

            if (!string.IsNullOrWhiteSpace(state.RegressionStateCode))
            {
                if (!state.Regressions.Any(s => s.StateCode.Equals(state.RegressionStateCode))
                    || !state.Flow.History.Any(h => h.StateCode.Equals(state.RegressionStateCode)))
                {
                    throw new StateException(state, $"Can't reset {state.StateCode} because it doesn't support regressing back to {regressStateCode}.");
                }

                var regressionState = state.Regressions.First(s => s.StateCode.Equals(state.RegressionStateCode));

                if(regressionState.Position > state.Position)
                {
                    throw new StateException(state, $"Can't regress to {regressionState.StateCode} (position {regressionState.Position}) because it is positioned after {state.StateCode} (position {state.Position}).");
                }

                var distinctStateHistory = state.Flow.History.Select(h => h.StateCode).Distinct().ToArray();
                var stateDictionary = state.Flow.StateDictionary;
                var distinctHistoryIndex = distinctStateHistory.Count() - 1;

                for (int i = distinctHistoryIndex; i >= 0; i--)
                {
                    var rs = stateDictionary[distinctStateHistory[i]];

                    if (distinctHistoryIndex.Equals(i)
                        && rs.StateCode.Equals(state.StateCode))
                    {
                        continue;
                    }

                    await rs.ResetAsync().ConfigureAwait(false);

                    if (rs.Equals(regressionState))
                    {
                        state.Flow.ActiveState = regressionState;

                        await state.Flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

                        if(!state.Flow.FlowStatus.Equals(FlowStatus.InProgress))
                        {
                            state.Flow.FlowStatus = FlowStatus.InProgress;
                        }

                        break;
                    }
                }
            }
        }

        public static async Task ExecuteActionsAsync(this State state, StateActionType stateFunctionType)
        {
            if (state.StateActions == null)
            {
                return;
            }

            if (!state.ActionsConfigured)
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

        public static State LastState(this List<State> states)
        {
            var lastPosition = states.Max(s => s.Position);

            return states.First(s => s.Position.Equals(lastPosition));
        }

        public static void Configure(this State state)
        {
            if(state.ActionsConfigured)
            {
                throw new StateException(state, $"{state.StateCode} has already been configured.");
            }

            if (!string.IsNullOrWhiteSpace(state.StateConfigurationClass))
            {
                StateConfiguration stateConfiguration = null;

                lock (stateConfigurationCacheLock)
                {
                    if (stateConfigurationCache.TryGetValue(
                        state.StateConfigurationClass, out StateConfiguration existingStateConfiguration))
                    {
                        stateConfiguration = existingStateConfiguration;
                    }
                    else
                    {
                        var type = Type.GetType(state.StateConfigurationClass);

                        if (type == null)
                        {
                            throw new StateException(state, $"Can't resolve {state.StateConfigurationClass}");
                        }

                        var instance = (IConfigureState)Activator.CreateInstance(type);

                        var methodInfo = type.GetMethod("Configure");

                        stateConfiguration = new StateConfiguration
                        {
                            Instance = instance,
                            MethodInfo = methodInfo
                        };

                        stateConfigurationCache.Add(state.StateConfigurationClass, stateConfiguration);
                    }
                }

                stateConfiguration.Configure(state);

                state.ActionsConfigured = true;
            }
        }
    }
}