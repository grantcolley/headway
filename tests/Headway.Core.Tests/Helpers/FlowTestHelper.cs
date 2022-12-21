using Headway.Core.Enums;
using Headway.Core.Model;

namespace Headway.Core.Tests.Helpers
{
    public class FlowTestHelper
    {
        public static Flow CreateFlow(int numberOfStates)
        {
            var flow = new Flow();

            for(int i = 1; i < numberOfStates + 1; i++)
            {
                flow.States.Add(new State { Position = i * 10, StateCode = $"State {i}", Flow = flow });
            }

            return flow;
        }
    }
}
