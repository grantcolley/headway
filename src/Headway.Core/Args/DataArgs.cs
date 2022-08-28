using System.Collections.Generic;

namespace Headway.Core.Args
{
    public class DataArgs
    {
        public DataArgs()
        {
            Args = new List<DataArg>();
        }

        public string SourceConfig { get; set; }
        public List<DataArg> Args { get; set; }
    }
}