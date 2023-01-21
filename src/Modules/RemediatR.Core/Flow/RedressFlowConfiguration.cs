using Headway.Core.Attributes;
using Headway.Core.Interface;

namespace RemediatR.Core.Flow
{
    [FlowConfiguration]
    public class RedressFlowConfiguration : IConfigureFlow
    {
        public void Configure(Headway.Core.Model.Flow flow)
        {
            // configure flow level state actions here...
        }
    }
}