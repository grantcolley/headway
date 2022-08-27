using System.Collections.Generic;

namespace Headway.Core.Args
{
    public class DataArgs
    {
        public DataArgs()
        {
            Args = new List<Arg>();
        }

        public List<Arg> Args { get; set; }
    }
}