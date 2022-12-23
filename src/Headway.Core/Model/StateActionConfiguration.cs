using Headway.Core.Interface;
using System.Reflection;

namespace Headway.Core.Model
{
    public class StateActionConfiguration
    {
        public IConfigureStateActions Instance { get; set; }
        public MethodInfo MethodInfo { get; set; }
    }
}
