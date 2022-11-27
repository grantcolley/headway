using System;

namespace Headway.Core.Flow
{
    public class StateException : Exception
    {
        public StateException(State state, string message = "", Exception innerException = null)
            : base(message, innerException)
        {
            State = state;
        }

        public State State { get; private set; }
    }
}
