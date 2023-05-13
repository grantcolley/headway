using Headway.Core.Constants;
using Headway.Core.Exceptions;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Extensions
{
    public static class FlowHistoryExtensions
    {
        /// <summary>
        /// Records a <see cref="State"/> Initialize event.
        /// </summary>
        /// <param name="history">The <see cref="FlowHistory"/></param>
        /// <param name="state">The <see cref="State"/></param>
        public static void RecordInitialise(this List<FlowHistory> history, State state)
        {
            history.RecordHistory(state, FlowHistoryEvents.INITIALIZE);
        }

        /// <summary>
        /// Records a <see cref="State"/> Start event.
        /// </summary>
        /// <param name="history">The <see cref="FlowHistory"/></param>
        /// <param name="state">The <see cref="State"/></param>
        public static void RecordStart(this List<FlowHistory> history, State state)
        {
            history.RecordHistory(state, FlowHistoryEvents.START);
        }

        /// <summary>
        /// Records a <see cref="State"/> Complete event.
        /// </summary>
        /// <param name="history">The <see cref="FlowHistory"/></param>
        /// <param name="state">The <see cref="State"/></param>
        public static void RecordComplete(this List<FlowHistory> history, State state)
        {
            history.RecordHistory(state, FlowHistoryEvents.COMPLETE);
        }

        /// <summary>
        /// Records a <see cref="State"/> Reset event.
        /// </summary>
        /// <param name="history">The <see cref="FlowHistory"/></param>
        /// <param name="state">The <see cref="State"/></param>
        public static void RecordReset(this List<FlowHistory> history, State state)
        {
            history.RecordHistory(state, FlowHistoryEvents.RESET);
        }

        /// <summary>
        /// Records a <see cref="State"/> event.
        /// </summary>
        /// <param name="history">The <see cref="FlowHistory"/></param>
        /// <param name="state">The <see cref="State"/></param>
        public static void RecordHistory(this List<FlowHistory> history, State state, string eventname)
        {
            history.Add(new FlowHistory
            {
                Event = eventname,
                FlowCode = state.Flow.FlowCode,
                StateCode = state.StateCode,
                StateStatus = state.StateStatus,
                Owner = state.Owner,
                Comment = state.Comment
            });
        }

        /// <summary>
        /// Replays the <see cref="Flow"/>'s history. 
        /// </summary>
        /// <param name="history">The <see cref="FlowHistory"/></param>
        /// <returns>
        /// A list of <see cref="State"/>'s. The last <see cref="State"/> is
        /// the current active <see cref="State"/> in the <see cref="Flow"/>. 
        /// All preceding <see cref="State"/>'s should be completed. 
        /// <see cref="State"/>'s that have been reset are removed from the flow.
        /// </returns>
        /// <exception cref="FlowException"></exception>
        public static List<FlowHistory> ReplayHistory(this List<FlowHistory> history)
        {
            Stack<FlowHistory> historyStack = new Stack<FlowHistory>();

            foreach (var flowHistory in history)
            {
                switch (flowHistory.Event)
                {
                    case FlowHistoryEvents.INITIALIZE:
                    case FlowHistoryEvents.START:
                    case FlowHistoryEvents.COMPLETE:
                        if (historyStack.Any())
                        {
                            var peekHistory= historyStack.Peek();

                            if(peekHistory.StateCode.Equals(flowHistory.StateCode))
                            {
                                _ = historyStack.Pop();
                            }
                        }

                        historyStack.Push(flowHistory);

                        break;

                    case FlowHistoryEvents.RESET:
                        var peekResetHistory = historyStack.Peek();

                        if (peekResetHistory.StateCode.Equals(flowHistory.StateCode))
                        {
                            throw new FlowHistoryException(flowHistory, $"Expecting {flowHistory.StateCode} but found {peekResetHistory.StateCode}.");
                        }

                        _ = historyStack.Pop();

                        break;
                };
            }

            return historyStack.ToList();
        }
    }
}
