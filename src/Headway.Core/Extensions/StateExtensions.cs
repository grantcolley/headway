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
    /// <summary>
    /// <see cref="StateExtensions"/> contains extension methods for a <see cref="State"/>. 
    /// </summary>
    public static class StateExtensions
    {
        private static readonly IDictionary<string, StateConfiguration> stateConfigurationCache = new Dictionary<string, StateConfiguration>();
        private static object stateConfigurationCacheLock = new object();

        /// <summary>
        /// Set <see cref="State"/> defaults e.g. <see cref="State.StateStatus"/>, <see cref="State.Owner"/>  and <see cref="State.Comment"/>.
        /// Assign the <see cref="Flow.Context"/> to the <see cref="State.Context"/> and set the <see cref="State.ParentState"/> if applicable.
        /// Populate the <see cref="State.SubStates"/>, <see cref="State.Transitions"/> and <see cref="State.Regressions"/>.
        /// </summary>
        /// <param name="state"></param>
        public static void Bootstrap(this State state)
        {
            state.StateStatus = default;
            state.Owner = default;
            state.Comment = default;

            state.Context = state.Flow.Context;

            if (!string.IsNullOrWhiteSpace(state.ParentStateCode))
            {
                state.ParentState = state.Flow.StateDictionary[state.ParentStateCode];
            }

            state.SubStates.Clear();
            state.SubStates.AddRange(state.Flow.ToStateList(state.SubStateCodesList));

            state.Transitions.Clear();
            state.Transitions.AddRange(state.Flow.ToStateList(state.TransitionStateCodesList));

            state.Regressions.Clear();
            state.Regressions.AddRange(state.Flow.ToStateList(state.RegressionStateCodesList));

            state.Bootstrapped = true;
        }

        /// <summary>
        /// Initialization routine for a <see cref="State"/>.
        /// 
        /// Sequence:   Execute any initialisation state actions
        ///             Set the status to <see cref="StateStatus.Initialized"/>
        ///             Record the initialization event in the <see cref="FlowHistory"/>
        ///             Set the state as the <see cref="Flow.ActiveState"/>
        ///             If the first state in the <see cref="Flow"/> set it to <see cref="FlowStatus.InProgress"/>
        ///             If <see cref="State.IsOwnerRestricted"/> is false or an owner has been assigned execute the <see cref="StartAsync"/> routine. 
        ///             
        /// <see cref="StateException"/> thrown when:
        ///             - the state is already <see cref="StateStatus.Initialized"/> or <see cref="StateStatus.InProgress"/>. 
        ///             - if <see cref="State.IsOwnerRestricted"/> is true and an owner has not been assigned (only checked after executing start actions).
        ///             - if the <see cref="StateType"/> is <see cref="StateType.Parent"/> but it doesn't have any associated sub states. 
        /// </summary>
        /// <param name="state">The state to initialize.</param>
        /// <returns>A Task.</returns>
        /// <exception cref="StateException"></exception>
        public static async Task InitialiseAsync(this State state)
        {
            if(state.StateStatus.Equals(StateStatus.Initialized)
                || state.StateStatus.Equals(StateStatus.InProgress)) 
            {
                throw new StateException(state, $"Can't initialize {state.StateCode} because it's {state.StateStatus}.");
            }

            state.Bootstrap();

            if (state.StateType.Equals(StateType.Parent)
                && !state.SubStates.Any())
            {
                throw new StateException(state, $"{state.StateCode} is configured as a {StateType.Parent} but has no sub states.");
            }

            await state.ExecuteActionsAsync(StateActionType.Initialize).ConfigureAwait(false);

            state.StateStatus = StateStatus.Initialized;

            state.Flow.History.RecordInitialise(state);

            state.Flow.ActiveState = state;

            if (!state.Flow.FlowStatus.Equals(FlowStatus.InProgress))
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

        /// <summary>
        /// Start routine for a <see cref="State"/>.
        /// 
        /// Sequence:   Execute any start state actions
        ///             Set the status to <see cref="StateStatus.InProgress"/>
        ///             Record the start event in the <see cref="FlowHistory"/>
        ///             If the <see cref="State"/> is <see cref="StateType.Parent"/> then run the initialization routine for it's first sub state. 
        ///             Else if the <see cref="State"/> is <see cref="StateType.Auto"/> then either auto complete or auto regress, as determined by the <see cref="State.AutoActionResult"/>. 
        ///             
        /// <see cref="StateException"/> thrown when:
        ///             - the state is not the <see cref="Flow.ActiveState"/> of if the state status.
        ///             - the state is not <see cref="StateStatus.Initialized"/>. 
        ///             - if <see cref="State.IsOwnerRestricted"/> is true and an owner has not been assigned (only checked after executing start actions).
        ///             - the <see cref="State"/> is <see cref="StateType.Auto"/> but it's <see cref="State.AutoActionResult"/> = <see cref="StateAutoActionResult.Unknown"/>.
        /// </summary>
        /// <param name="state">The state to start.</param>
        /// <returns>A Task.</returns>
        /// <exception cref="StateException"></exception>
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

            await state.ExecuteActionsAsync(StateActionType.Start).ConfigureAwait(false);

            if (state.StateType.Equals(StateType.Auto)
                && state.AutoActionResult.Equals(StateAutoActionResult.Unknown))
            {
                throw new StateException(state, $"{state.StateCode} is a Auto state but it's AutoActionResult is Unknown.");
            }

            if (state.IsOwnerRestricted
                && string.IsNullOrWhiteSpace(state.Owner))
            {
                throw new StateException(state, $"Can't start owner restricted state {state.StateCode} without an owner.");
            }

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

        /// <summary>
        /// Completion routine for a <see cref="State"/>.
        /// 
        /// Sequence:   If <see cref="transitionStateCode"/> is not null or empty assign it to <see cref="State.TransitionStateCode"/>.
        ///             Execute any complete state actions
        ///             If <see cref="State.TransitionStateCode"/> is still unassigned, assign it to the first <see cref="State"/> in <see cref="State.Transitions"/>.
        ///             Set the status to <see cref="StateStatus.Completed"/>
        ///             Record the completed event in the <see cref="FlowHistory"/>
        ///             If it has a state to transition to then run the initialization routine for the transition state.
        ///             Else if the state has a <see cref="State.ParentState"/> then run the completion routine for the <see cref="State.ParentState"/>.
        ///             If the <see cref="State"/> is the <see cref="Flow.FinalState"/> then set the <see cref="Flow.FlowStatus"/> to <see cref="FlowStatus.Completed"/>.
        ///             
        /// <see cref="StateException"/> thrown when:
        ///             - the state is not the <see cref="Flow.ActiveState"/> of if the state status is not <see cref="StateStatus.InProgress"/>. 
        ///             - if it tries to transition to a state that is not in its configured list of transition states.
        ///             - if <see cref="State.IsOwnerRestricted"/> is true and an owner has not been assigned (only checked after executing complete actions). 
        /// </summary>
        /// <param name="state">The state to complete.</param>
        /// <param name="transitionStateCode">Optional state code of the state to transition to once the state has been completed.</param>
        /// <returns>A Task.</returns>
        /// <exception cref="StateException"></exception>
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

            if (state.IsOwnerRestricted
                && string.IsNullOrWhiteSpace(state.Owner))
            {
                throw new StateException(state, $"Can't complete owner restricted state {state.StateCode} without an owner.");
            }

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

        /// <summary>
        /// Reset routine for a <see cref="State"/>.
        /// 
        /// Sequence:   If <see cref="regressStateCode"/> is not null or empty assign it to <see cref="State.RegressionStateCode"/>.
        ///             Execute any reset state actions.
        ///             Set the <see cref="State.StateStatus"/> to <see cref="StateStatus.Uninitialized"/>.
        ///             Record the reset event in the <see cref="FlowHistory"/>.
        ///             Set the <see cref="State.Owner"/> and <see cref="State.Comment"/> to null.
        ///             If <see cref="State.RegressionStateCode"/> is assigned then traverse backwards in the <see cref="Flow.History"/> 
        ///                 calling the reset routine on all prior states in the <see cref="Flow.History"/>. When it reaches the target 
        ///                 regression state, call the initialization routine on the target regression state. Finally, ensure the 
        ///                 <see cref="Flow.FlowStatus"/> is set to <see cref="FlowStatus.InProgress"/>.
        ///             
        /// <see cref="StateException"/> thrown when:
        ///             - the <see cref="regressStateCode"/> is null and the state status is <see cref="StateStatus.Uninitialized"/>. 
        ///             - if it tries to regress to a state that is not in its configured list of regression states or in the <see cref="Flow.ReplayHistory"/>.
        ///             - if the <see cref="State.Position"/> of the regression state is greater than that of the <see cref="State"/>. 
        /// </summary>
        /// <param name="state">The state to reset.</param>
        /// <param name="regressStateCode">Optional state code of the state to regress to.</param>
        /// <returns>A Task.</returns>
        /// <exception cref="StateException">
        /// Thrown if
        /// - the state to be reset is <see cref="StateStatus.Uninitialized"/>
        /// - there is no <see cref="Flow.ReplayHistory"/>.
        /// - the regressStateCode is not in the <see cref="State.Regressions"/>.
        /// - the target <see cref="State.Position"/> is greater then the <see cref="State.Position"/> being reset from.
        /// </exception>
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

            state.Flow.ReplayFlowHistory();

            if (!state.Flow.ReplayHistory.Any())
            {
                throw new StateException(state, "Can't reset as there is no flow history.");
            }

            if (!string.IsNullOrWhiteSpace(state.RegressionStateCode))
            {
                if (!state.Regressions.Any(s => s.StateCode.Equals(state.RegressionStateCode))
                    || !state.Flow.ReplayHistory.Any(h => h.StateCode.Equals(state.RegressionStateCode)))
                {
                    throw new StateException(state, $"Can't reset {state.StateCode} because it doesn't support resetting back to {regressStateCode}.");
                }

                var regressionState = state.Regressions.First(s => s.StateCode.Equals(state.RegressionStateCode));

                if (!string.IsNullOrEmpty(regressionState.ParentStateCode))
                {
                    if (string.IsNullOrEmpty(state.ParentStateCode))
                    {
                        throw new StateException(state, $"Can't regress to sub state {regressionState.StateCode} of {regressionState.ParentStateCode} because it doesn't share the same parent as {state.StateCode}.");
                    }
                    else if (!state.ParentStateCode.Equals(regressionState.ParentStateCode))
                    {
                        throw new StateException(state, $"Can't regress to sub state {regressionState.StateCode} of {regressionState.ParentStateCode} because it doesn't share the same parent as {state.StateCode} which is {state.ParentStateCode}.");
                    }
                }

                if (regressionState.Position > state.Position)
                {
                    throw new StateException(state, $"Can't regress to {regressionState.StateCode} (position {regressionState.Position}) because it is positioned after {state.StateCode} (position {state.Position}).");
                }
            }

            var reverseHistoryIndex = state.Flow.ReplayHistory.Count - 1;

            for (int i = reverseHistoryIndex; i >= 0; i--)
            {
                var rs = state.Flow.StateDictionary[state.Flow.ReplayHistory[i].StateCode];

                await rs.ExecuteActionsAsync(StateActionType.Reset).ConfigureAwait(false);

                rs.StateStatus = default;

                rs.Flow.History.RecordReset(rs);

                rs.Owner = default;
                rs.Comment = default;

                if (!string.IsNullOrEmpty(state.RegressionStateCode)
                    && rs.StateCode.Equals(state.RegressionStateCode))
                {
                    await rs.InitialiseAsync().ConfigureAwait(false);

                    break;
                }
            }

            state.Flow.ReplayFlowHistory();
        }

        /// <summary>
        /// Executes <see cref="State"/> actions sequentially in the specified order.
        /// </summary>
        /// <param name="state">The <see cref="State"/>.</param>
        /// <param name="stateFunctionType">The <see cref="StateActionType"/>.</param>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Gets the <see cref="State"/> with the minimum position.
        /// </summary>
        /// <param name="states">A list of <see cref="State"/>'s</param>
        /// <returns>The <see cref="State"/> with the minimum position</returns>
        public static State FirstState(this List<State> states)
        {
            var firstPosition = states.Min(s => s.Position);

            return states.First(s => s.Position.Equals(firstPosition));
        }

        /// <summary>
        /// Gets the <see cref="State"/> with the maximum position.
        /// </summary>
        /// <param name="states">A list of <see cref="State"/>'s</param>
        /// <returns>The <see cref="State"/> with the maximum position</returns>
        public static State LastState(this List<State> states)
        {
            var lastPosition = states.Max(s => s.Position);

            return states.First(s => s.Position.Equals(lastPosition));
        }

        /// <summary>
        /// Creates an instance of the <see cref="State.StateConfigurationClass"/>
        /// which implements <see cref="IConfigureState"/> and caches it.
        /// The <see cref="State.ActionsConfigured"/> field is set to true.
        /// 
        /// <see cref="StateException"/> thrown when:
        ///     - <see cref="State.ActionsConfigured"/> is already true
        ///     - <see cref="State.StateConfigurationClass"/> cannot be resolved.
        ///         
        /// </summary>
        /// <param name="state">The <see cref="State"/>.</param>
        /// <exception cref="StateException"></exception>
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