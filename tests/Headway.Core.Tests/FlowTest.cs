using Headway.Core.Enums;
using Headway.Core.Extensions;
using Headway.Core.Model;
using Headway.Core.Tests.Helpers;

namespace Headway.Core.Tests
{
    [TestClass]
    public class FlowTest
    {
        [TestMethod]
        public void StateExtensions_FirstState()
        {
            // Arrange
            var states = new List<State>
            {
                new State { Position = 100 },
                new State { Position = 50 },
                new State { Position = 10 },
                new State { Position = 250 }
            };

            // Act
            var firstState = states.FirstState();

            // Assert
            Assert.AreEqual(firstState, states.Single(s => s.Position == 10));
        }

        [TestMethod]
        public void Flow_RootState()
        {
            // Arrange
            var flow = FlowTestData.CreateFlow(2);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";

            flow.ReplayHistory();

            // Act
            var rootState = flow.RootState;

            // Assert
            Assert.AreEqual(flow.States.FirstState(), rootState);
        }

        [TestMethod]
        public async Task Flow_Transition_Default()
        {
            // Arrange
            var flow = FlowTestData.CreateFlow(2);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";

            flow.ReplayHistory();

            // Act
            await flow.ActiveState.CompleteAsync();

            // Assert
            Assert.AreEqual(flow.States[0].StateStatus, StateStatus.Completed);
            Assert.AreEqual(flow.States[1].StateStatus, StateStatus.InProgress);
            Assert.AreEqual(flow.States[1], flow.ActiveState);
        }

        [TestMethod]
        public async Task Flow_Transition_To_StateCode()
        {
            // Arrange
            var flow = FlowTestData.CreateFlow(2);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";

            flow.ReplayHistory();

            // Act
            await flow.ActiveState.CompleteAsync(flow.States[1].StateCode);

            // Assert
            Assert.AreEqual(flow.States[0].StateStatus, StateStatus.Completed);
            Assert.AreEqual(flow.States[1].StateStatus, StateStatus.InProgress);
            Assert.AreEqual(flow.States[1], flow.ActiveState);
        }
    }
}
