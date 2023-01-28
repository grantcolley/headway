using Headway.Core.Exceptions;
using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Extensions
{
    public static class FlowExtensions
    {
        private static readonly IDictionary<string, FlowConfiguration> flowConfigurationCache = new Dictionary<string, FlowConfiguration>();
        private static readonly object flowConfigurationCacheLock = new();

        public static void Bootstrap(this Flow flow, List<FlowHistory> history)
        {
            flow.History.Clear();
            flow.History.AddRange(history);
            flow.Bootstrap();
        }

        public static void Bootstrap(this Flow flow)
        {
            if (flow.Bootstrapped)
            {
                throw new FlowException(flow, $"{flow.Name} already {nameof(flow.Bootstrapped)}.");
            }

            flow.StateDictionary = flow.States.ToDictionary(s => s.StateCode, s => s);

            foreach (var state in flow.States)
            {
                state.StateStatus = default;
                state.Owner = default;
                state.Comment = default;

                state.Flow = flow;

                if (state.Context != null)
                {
                    state.Context = flow.Context;
                }

                if (!string.IsNullOrWhiteSpace(state.ParentStateCode))
                {
                    state.ParentState = flow.StateDictionary[state.ParentStateCode];
                }

                state.SubStates.Clear();
                state.SubStates.AddRange(flow.ToStateList(state.SubStateCodesList));

                state.Transitions.Clear();
                state.Transitions.AddRange(flow.ToStateList(state.TransitionStateCodesList));

                state.Regressions.Clear();
                state.Regressions.AddRange(flow.ToStateList(state.RegressionStateCodesList));
            }

            flow.Configure();

            if (flow.ConfigureStatesDuringBootstrap)
            {
                foreach (var state in flow.States)
                {
                    state.Configure();
                }
            }

            if (flow.History.Any())
            {
                var lastHistory = flow.History.Last();
                var lastState = flow.StateDictionary[lastHistory.StateCode];
                lastState.StateStatus = lastHistory.StateStatus;
                lastState.Owner = lastHistory.Owner;
                lastState.Comment = lastHistory.Comment;
                flow.ActiveState = lastState;
            }
            else
            {
                flow.ActiveState = flow.RootState;
            }

            flow.Bootstrapped = true;
        }

        public static List<State> ToStateList(this Flow flow, List<string> stateCodes)
        {
            if (stateCodes == null)
            {
                throw new FlowException(flow, nameof(stateCodes));
            }

            var states = new List<State>();

            foreach (var stateCode in stateCodes)
            {
                states.Add(flow.StateDictionary[stateCode]);
            }

            return states;
        }

        public static void Configure(this Flow flow)
        {
            if (flow.ActionsConfigured)
            {
                throw new FlowException(flow, $"{flow.Name} has already been configured.");
            }

            if (!string.IsNullOrWhiteSpace(flow.FlowConfigurationClass))
            {
                FlowConfiguration flowConfiguration = null;

                lock (flowConfigurationCacheLock)
                {
                    if (flowConfigurationCache.TryGetValue(
                        flow.FlowConfigurationClass, out FlowConfiguration existingFlowConfiguration))
                    {
                        flowConfiguration = existingFlowConfiguration;
                    }
                    else
                    {
                        var type = Type.GetType(flow.FlowConfigurationClass);

                        if (type == null)
                        {
                            throw new FlowException(flow, $"Can't resolve {flow.FlowConfigurationClass}");
                        }

                        var instance = (IConfigureFlow)Activator.CreateInstance(type);

                        var methodInfo = type.GetMethod("Configure");

                        flowConfiguration = new FlowConfiguration
                        {
                            Instance = instance,
                            MethodInfo = methodInfo
                        };

                        flowConfigurationCache.Add(flow.FlowConfigurationClass, flowConfiguration);
                    }
                }

                flowConfiguration.Configure(flow);

                flow.ActionsConfigured = true;
            }
        }

        public static void RecordInitialise(this List<FlowHistory> history, State state)
        {
            history.RecordHistory(state, "Initialize");
        }

        public static void RecordCompleted(this List<FlowHistory> history, State state)
        {
            history.RecordHistory(state, "Completed");
        }

        public static void RecordReset(this List<FlowHistory> history, State state)
        {
            history.RecordHistory(state, "Reset");
        }

        public static void RecordHistory(this List<FlowHistory> history, State state, string eventname)
        {
            history.Add(new FlowHistory
            {
                Event = eventname,
                Flow = state.Flow,
                FlowCode = state.Flow.FlowCode,
                StateCode = state.StateCode,
                StateStatus = state.StateStatus,
                Owner = state.Owner,
                Comment = state.Comment
            });
        }
    }
}
