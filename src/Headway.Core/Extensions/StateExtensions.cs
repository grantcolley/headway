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
        private static readonly IDictionary<string, StateActionConfiguration> stateActionConfigurationCache = new Dictionary<string, StateActionConfiguration>();
        private static object stateActionConfigurationCacheLock = new object();

        public static async Task InitialiseAsync(this State state)
        {
            if(state.StateStatus.Equals(StateStatus.InProgress)) 
            {
                throw new InvalidOperationException($"Can't initialize {state.StateStatus} because it's already {StateStatus.InProgress}.");
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
                throw new InvalidOperationException($"Can't complete {state.StateStatus} because it's already {StateStatus.Completed}.");
            }

            var uncompletedSubStates = state.SubStates.Where(s => s.StateStatus != StateStatus.Completed).ToList();

            if(uncompletedSubStates.Any())
            {
                var uncompletedSubStateDescriptions = uncompletedSubStates.Select(s => $"{s.StateCode}={s.StateStatus}");
                var joinedDescriptions = string.Join(",", uncompletedSubStateDescriptions);
                throw new FlowException(state, $"Can't complete {state.StateCode} because sub states not yet {StateStatus.Completed} : {joinedDescriptions}.");
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
                    throw new FlowException(state, $"Can't complete {state.StateCode} because doesn't support transitioning to {transitionStateCode}.");
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
            if (state.StateStatus.Equals(StateStatus.NotStarted))
            {
                throw new InvalidOperationException($"Can't reset {state.StateStatus} because it's {StateStatus.NotStarted}.");
            }

            await state.ExecuteActionsAsync(StateActionType.Reset).ConfigureAwait(false);

            state.StateStatus = StateStatus.NotStarted;
        }

        public static async Task ExecuteActionsAsync(this State state, StateActionType stateFunctionType)
        {
            if (state.StateActions == null)
            {
                return;
            }

            if (!state.ActionsConfigured)
            {
                state.ConfigureStateActions();
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

        public static void ConfigureStateActions(this State state)
        {
            if(state.ActionsConfigured)
            {
                throw new InvalidOperationException($"{state.StateCode} has already had actions configured.");
            }

            if (!string.IsNullOrWhiteSpace(state.ActionSetupClass))
            {
                StateActionConfiguration actionConfiguration = null;

                lock (stateActionConfigurationCacheLock)
                {
                    if (stateActionConfigurationCache.TryGetValue(
                        state.ActionSetupClass, out StateActionConfiguration stateActionConfiguration))
                    {
                        actionConfiguration = stateActionConfiguration;
                    }
                    else
                    {
                        var type = Type.GetType(state.ActionSetupClass);

                        if (type == null)
                        {
                            throw new ArgumentNullException(nameof(state.ActionSetupClass));
                        }

                        var instance = (IConfigureStateActions)Activator.CreateInstance(type);

                        var methodInfo = type.GetMethod("ConfigureActions");

                        stateActionConfiguration = new StateActionConfiguration
                        {
                            Instance = instance,
                            MethodInfo = methodInfo
                        };

                        stateActionConfigurationCache.Add(state.ActionSetupClass, stateActionConfiguration);

                        actionConfiguration = stateActionConfiguration;
                    }
                }

                _ = actionConfiguration.MethodInfo.Invoke(
                    actionConfiguration.Instance, new object[] { state });

                state.ActionsConfigured = true;
            }
        }
    }
}