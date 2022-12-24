using Headway.Core.Interface;
using System.Reflection;

namespace Headway.Core.Model
{
    public class StateConfiguration
    {
        public IConfigureState Instance { get; set; }
        public MethodInfo MethodInfo { get; set; }

        public void Configure(State state)
        {
            _ = MethodInfo.Invoke(Instance, new object[] { state });
        }
    }
}
