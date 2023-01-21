using Headway.Core.Attributes;
using Headway.Core.Interface;
using Headway.Core.Model;

namespace RemediatR.Core.Flow
{
    [StateConfiguration]
    public class RedressStateConfiguration : IConfigureState
    {
        public void Configure(State state)
        {
            // configure state level actions here...
        }
    }
}
