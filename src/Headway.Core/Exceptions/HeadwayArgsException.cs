using System;

namespace Headway.Core.Exceptions
{
    public class HeadwayArgsException : Exception
    {
        public HeadwayArgsException(string message = "", Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
