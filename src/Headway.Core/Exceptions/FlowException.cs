using System;
using Headway.Core.Model;

namespace Headway.Core.Exceptions
{
    public class FlowException : Exception
    {
        public FlowException(Flow flow, string message = "", Exception innerException = null)
            : base(message, innerException)
        {
            Flow = flow;
        }

        public Flow Flow { get; private set; }
    }
}
