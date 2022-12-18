using System;
using Headway.Core.Model;

namespace Headway.Core.Exceptions
{
    public class FlowException : Exception
    {
        public FlowException(State state, string message = "", Exception innerException = null)
            : base(message, innerException)
        {
            State = state;
        }

        public State State { get; private set; }
    }
}
