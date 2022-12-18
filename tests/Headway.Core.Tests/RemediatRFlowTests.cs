using Headway.SeedData.RemediatR;

namespace Headway.Core.Tests
{
    [TestClass]
    public class RemediatRFlowTests
    {
        [TestMethod]
        public void Flow_RootState()
        {
            // Arrange
            var flow = RemediatRFlow.FlowCreate();

            // Act
            var rootState = flow.RootState;

            // Assert
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REDRESS_CREATE")), rootState);
        }

        [TestMethod]
        public void Flow_ReplayHistory_NoHistory()
        {
            // Arrange
            var flow = RemediatRFlow.FlowCreate();
            var rootState = flow.RootState;

            // Act
            flow.ReplayHistory();

            // Assert active state
            Assert.AreEqual(flow.RootState, rootState);
            Assert.AreEqual(flow.ActiveState, rootState);

            // Assert sample of parent and substates
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")), flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")).ParentState);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")), flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION")).ParentState);
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).SubStates.Contains(flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION"))));
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).SubStates.Contains(flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION"))));

            // Assert sample of transition states
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REDRESS_CREATE")).Transitions.Contains(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT"))));
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).Transitions.Contains(flow.States.First(s => s.StateCode.Equals("REDRESS_CREATE"))));
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).Transitions.Contains(flow.States.First(s => s.StateCode.Equals("REFUND_REVIEW"))));
        }

        public void State_Complete_Transition_ParentState()
        {
            // Arrange

            // Act

            //Assert

        }

        public void State_Complete_Last_SubState()
        {
            // Arrange

            // Act

            //Assert

        }
    }
}