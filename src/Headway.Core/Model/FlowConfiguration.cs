using Headway.Core.Interface;
using System;
using System.Reflection;

namespace Headway.Core.Model
{
    public class FlowConfiguration
    {
        public IConfigureFlow Instance { get; set; }
        public MethodInfo MethodInfo { get; set; }

        public void Configure(Flow flow)
        {
            if(flow == null)
            {
                throw new ArgumentNullException(nameof(flow));
            }

            _ = MethodInfo.Invoke(Instance, new object[] { flow });
        }
    }
}
