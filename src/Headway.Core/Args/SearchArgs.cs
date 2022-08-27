using System.Collections.Generic;

namespace Headway.Core.Args
{
    public class SearchArgs
    {
        public SearchArgs()
        {
            Args = new List<SearchArg>();
        }

        public string SourceConfig { get; set; }
        public List<SearchArg> Args { get; set; }
    }
}
