using Headway.Core.Interface;
using System.Reflection;

namespace Headway.Core.Model
{
    public class FlowActionConfiguration
    {
        public IConfigureFlowActions Instance { get; set; }
        public MethodInfo MethodInfo { get; set; }
    }
}
