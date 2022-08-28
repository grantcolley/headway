using System.Collections.Generic;

namespace Headway.Core.Args
{
    public class DataArgs
    {
        public DataArgs()
        {
            Args = new List<DataArg>();
        }

        public List<DataArg> Args { get; set; }
    }
}