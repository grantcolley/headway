using System;

namespace Headway.Core.Exceptions
{
    public class HeadwayException : Exception
    {
        public HeadwayException(string message = "", Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
