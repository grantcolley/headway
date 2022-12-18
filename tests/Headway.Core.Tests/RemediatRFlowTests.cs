using Headway.SeedData.RemediatR;

namespace Headway.Core.Tests
{
    [TestClass]
    public class RemediatRFlowTests
    {
        [TestMethod]
        public void RemediatR_Flow_Configure()
        {
            // Arrange
            var flow = RemediatRFlow.FlowCreate();
            var rootState = flow.RootState;

            // Act
            flow.SetActiveState();

            // Assert root and active states
            Assert.AreEqual(flow.RootState, rootState);
            Assert.AreEqual(flow.ActiveState, rootState);

            // Assert sample of parent and substates
            Assert.AreEqual(flow.States.First(s => s.Code.Equals("REFUND_ASSESSMENT")), flow.States.First(s => s.Code.Equals("REFUND_CALCULATION")).ParentState);
            Assert.AreEqual(flow.States.First(s => s.Code.Equals("REFUND_ASSESSMENT")), flow.States.First(s => s.Code.Equals("REFUND_VERIFICATION")).ParentState);
            Assert.IsTrue(flow.States.First(s => s.Code.Equals("REFUND_ASSESSMENT")).SubStates.Contains(flow.States.First(s => s.Code.Equals("REFUND_CALCULATION"))));
            Assert.IsTrue(flow.States.First(s => s.Code.Equals("REFUND_ASSESSMENT")).SubStates.Contains(flow.States.First(s => s.Code.Equals("REFUND_VERIFICATION"))));

            // Assert sample of transition states
            Assert.IsTrue(flow.States.First(s => s.Code.Equals("REDRESS_CREATE")).Transitions.Contains(flow.States.First(s => s.Code.Equals("REFUND_ASSESSMENT"))));
            Assert.IsTrue(flow.States.First(s => s.Code.Equals("REFUND_ASSESSMENT")).Transitions.Contains(flow.States.First(s => s.Code.Equals("REDRESS_CREATE"))));
            Assert.IsTrue(flow.States.First(s => s.Code.Equals("REFUND_ASSESSMENT")).Transitions.Contains(flow.States.First(s => s.Code.Equals("REFUND_REVIEW"))));
        }
    }
}