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
        private static object flowConfigurationCacheLock = new object();

        public static void Bootstrap(this Flow flow)
        {
            if (flow.Bootstrapped)
            {
                throw new InvalidOperationException($"{flow.Name} already {nameof(flow.Bootstrapped)}.");
            }

            flow.StateDictionary = flow.States.ToDictionary(s => s.StateCode, s => s);

            foreach (var state in flow.StateDictionary)
            {
                state.Value.StateStatus = default;
                state.Value.Owner = default;
                state.Value.Flow = flow;

                if (state.Value.Context != null)
                {
                    state.Value.Context = flow.Context;
                }

                if (!string.IsNullOrWhiteSpace(state.Value.ParentStateCode))
                {
                    state.Value.ParentState = flow.StateDictionary[state.Value.ParentStateCode];
                }

                state.Value.SubStates.Clear();
                state.Value.SubStates.AddRange(flow.ToStateList(state.Value.SubStateCodesList));

                state.Value.Transitions.Clear();
                state.Value.Transitions.AddRange(flow.ToStateList(state.Value.TransitionStateCodesList));
            }

            flow.Configure();

            if (flow.History.Any())
            {
                var lastIndex = flow.History.Count - 1;

                for (int i = 0; i < lastIndex - 1; i++)
                {
                    var history = flow.History[i];

                    var state = flow.StateDictionary[history.StateCode];
                    state.StateStatus = history.StateStatus;
                    state.Owner = history.Owner;

                    if (i.Equals(lastIndex))
                    {
                        flow.ActiveState = state;
                    }
                }
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
                throw new ArgumentNullException(nameof(stateCodes));
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
            if (flow.Configured)
            {
                throw new InvalidOperationException($"{flow.Name} has already been configured.");
            }

            if (!string.IsNullOrWhiteSpace(flow.ConfigureFlowClass))
            {
                FlowConfiguration flowConfiguration = null;

                lock (flowConfigurationCacheLock)
                {
                    if (flowConfigurationCache.TryGetValue(
                        flow.ConfigureFlowClass, out FlowConfiguration existingFlowConfiguration))
                    {
                        flowConfiguration = existingFlowConfiguration;
                    }
                    else
                    {
                        var type = Type.GetType(flow.ConfigureFlowClass);

                        if (type == null)
                        {
                            throw new ArgumentNullException($"{nameof(type)} for {flow.ConfigureFlowClass}");
                        }

                        var instance = (IConfigureFlow)Activator.CreateInstance(type);

                        var methodInfo = type.GetMethod("Configure");

                        flowConfiguration = new FlowConfiguration
                        {
                            Instance = instance,
                            MethodInfo = methodInfo
                        };

                        flowConfigurationCache.Add(flow.ConfigureFlowClass, flowConfiguration);
                    }
                }

                flowConfiguration.Configure(flow);

                flow.Configured = true;
            }
        }
    }
}
