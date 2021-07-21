using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Core.Interface
{
    public interface IConfigCache
    {
        Config GetConfig(string configName);
        void AddConfig(Config config);
        void AddConfigs(IEnumerable<Config> configs);
    }
}
