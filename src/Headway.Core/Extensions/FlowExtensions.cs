using Headway.Core.Constants;
using Headway.Core.Exceptions;
using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Extensions
{
    /// <summary>
    /// <see cref="FlowExtensions"/> contains extension methods for a <see cref="Flow"/>. 
    /// </summary>
    public static class FlowExtensions
    {
        private static readonly IDictionary<string, FlowConfiguration> flowConfigurationCache = new Dictionary<string, FlowConfiguration>();
        private static readonly object flowConfigurationCacheLock = new();

        /// <summary>
        /// Adds the history and runs the <see cref="Flow"/>'s <see cref="Bootstrap(Flow)" routine./>
        /// </summary>
        /// <param name="flow">The <see cref="Flow"/></param>
        /// <param name="history">The <see cref="List{FlowHistory}"/>.</param>
        public static void Bootstrap(this Flow flow, List<FlowHistory> history, bool isClientSide = false)
        {
            flow.History.Clear();
            flow.History.AddRange(history);
            flow.Bootstrap(isClientSide);
        }

        /// <summary>
        /// The <see cref="Flow"/>'s bootstrap routine.
        /// 
        /// Sequece:    For each state:
        ///                 - set the <see cref="State.StateStatus"/> to Uninitialized 
        ///                 - set the <see cref="State.Owner"/> and <see cref="State.Comment"/> to null
        ///                 - set the <see cref="State.Flow"/>
        ///                 - if the <see cref="State.Context"/> inherit it from <see cref="Flow.Context"/>
        ///                 - set the <see cref="State.ParentState"/> if applicable
        ///                 - if applicable set the substates, transition states and regression states
        ///                 - configure <see cref="Flow"/> actions
        ///                 - if the <see cref="Flow"/> has history set the <see cref="State"/> in the last history event as the <see cref="Flow.ActiveState"/> 
        ///                 - else if there is no <see cref="Flow"/> history set the <see cref="Flow.RootState"/> as the <see cref="Flow.ActiveState"/>
        ///                 - set <see cref="Flow.Bootstrapped"/> to true.
        ///                 
        /// <see cref="FlowException"/> thrown when:
        ///                 - <see cref="Flow.Bootstrapped"/> is already true
        /// </summary>
        /// <param name="flow">The <see cref="Flow"/></param>
        /// <exception cref="FlowException"></exception>
        public static void Bootstrap(this Flow flow, bool isClientSide = false)
        {
            if (flow.Bootstrapped)
            {
                throw new FlowException(flow, $"{flow.Name} already {nameof(flow.Bootstrapped)}.");
            }

            foreach (var state in flow.States)
            {
                state.Flow = flow;
                flow.StateDictionary.Add(state.StateCode, state);
            }

            if (!isClientSide)
            {
                flow.Configure();
            }

            flow.ReplayHistory();

            var lastHistory = flow.ReplayHistory.LastOrDefault();

            if (lastHistory != null)
            {
                var lastState = flow.StateDictionary[lastHistory.StateCode].Bootstrap();
                lastState.StateStatus = lastHistory.StateStatus;
                lastState.Owner = lastHistory.Owner;
                lastState.Comment = lastHistory.Comment;
                flow.ActiveState = lastState;
            }
            else
            {
                flow.ActiveState = flow.RootState.Bootstrap();
            }

            flow.Bootstrapped = true;
        }

        /// <summary>
        /// Takes a list of <see cref="State"/> codes and returns a list of states.
        /// 
        /// <see cref="FlowException"/> thrown when:
        ///     - when stateCodes is null
        ///     
        /// </summary>
        /// <param name="flow">The <see cref="Flow"/></param>
        /// <param name="stateCodes">A list of state codes</param>
        /// <returns>A list of states</returns>
        /// <exception cref="FlowException"></exception>
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

        /// <summary>
        /// Creates an instance of the <see cref="Flow.FlowConfigurationClass"/>
        /// which implements <see cref="IConfigureFlow"/> and caches it.
        /// The <see cref="Flow.ActionsConfigured"/> field is set to true.
        /// 
        /// <see cref="FlowException"/> thrown when:
        ///     - <see cref="Flow.ActionsConfigured"/> is already true
        ///     - <see cref="Flow.FlowConfigurationClass"/> cannot be resolved.
        ///         
        /// </summary>
        /// <param name="flow"></param>
        /// <exception cref="FlowException"></exception>
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

        /// <summary>
        /// Replays the <see cref="Flow"/>'s history. 
        /// </summary>
        /// <param name="flow">The <see cref="Flow"/></param>
        /// <exception cref="FlowHistoryException"></exception>
        public static void ReplayHistory(this Flow flow)
        {
            flow.ReplayHistory.Clear();

            foreach (var flowHistory in flow.History)
            {
                switch (flowHistory.Event)
                {
                    case FlowHistoryEvents.INITIALIZE:
                    case FlowHistoryEvents.START:
                    case FlowHistoryEvents.COMPLETE:
                        if(!flow.ReplayHistory.Any())
                        {
                            flow.ReplayHistory.Add(flowHistory);
                            continue;
                        }

                        var lastHistory = flow.ReplayHistory.Last();

                        if (lastHistory.StateCode.Equals(flowHistory.StateCode))
                        {
                            _ = flow.ReplayHistory.Remove(lastHistory);
                            flow.ReplayHistory.Add(flowHistory);
                        }
                        else
                        {
                            var state = flow.StateDictionary[lastHistory.StateCode];

                            if(!string.IsNullOrEmpty(state.ParentStateCode)
                                && lastHistory.StateCode.Equals(state.ParentStateCode))
                            {
                                var parentHistory = flow.ReplayHistory.First(h => h.StateCode.Equals(flowHistory.StateCode));
                                flow.ReplayHistory[flow.ReplayHistory.IndexOf(parentHistory)] = flowHistory;
                            }
                            else
                            {
                                flow.ReplayHistory.Add(flowHistory);
                            }
                        }

                        break;

                    case FlowHistoryEvents.RESET:
                        var lastResetHistory = flow.ReplayHistory.LastOrDefault();

                        if (lastResetHistory != null)
                        {
                            if (lastResetHistory.StateCode.Equals(flowHistory.StateCode))
                            {
                                throw new FlowHistoryException(flowHistory, $"Expecting {flowHistory.StateCode} but found {lastResetHistory.StateCode}.");
                            }
                        }

                        _ = flow.ReplayHistory.Remove(lastResetHistory);

                        break;
                };
            }
        }
    }
}
