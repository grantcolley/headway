using System;
using Headway.Core.Model;

namespace Headway.Core.Exceptions
{
    public class FlowHistoryException : Exception
    {
        public FlowHistoryException(FlowHistory flowHistory, string message = "", Exception innerException = null)
            : base(message, innerException)
        {
            FlowHistory = flowHistory;
        }

        public FlowHistory FlowHistory { get; private set; }
    }
}
