using Headway.Core.Constants;
using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Core.Extensions
{
    public static class FlowHistoryExtensions
    {
        /// <summary>
        /// Records a <see cref="State"/> take ownership event.
        /// </summary>
        /// <param name="history">The <see cref="FlowHistory"/></param>
        /// <param name="state">The <see cref="State"/></param>
        public static void RecordTakeOwnership(this List<FlowHistory> history, State state)
        {
            history.RecordHistory(state, FlowHistoryEvents.TAKE_OWNERSHIP);
        }

        /// <summary>
        /// Records a <see cref="State"/> relinquish ownership event.
        /// </summary>
        /// <param name="history">The <see cref="FlowHistory"/></param>
        /// <param name="state">The <see cref="State"/></param>
        public static void RecordRelinquishOwnership(this List<FlowHistory> history, State state)
        {
            history.RecordHistory(state, FlowHistoryEvents.RELINQUISH_OWNERSHIP);
        }

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
            int index = history.Count + 1;

            history.Add(new FlowHistory
            {
                Index = index,
                Event = eventname,
                FlowCode = state.Flow.FlowCode,
                StateCode = state.StateCode,
                StateStatus = state.StateStatus,
                Owner = state.Owner,
                Comment = state.Comment
            });
        }
    }
}
