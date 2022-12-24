using Headway.Core.Exceptions;
using Headway.Core.Extensions;
using Headway.Core.Tests.Helpers;
using Headway.SeedData.RemediatR;

namespace Headway.Core.Tests
{
    [TestClass]
    public class FlowTests
    {
        [TestMethod]
        public void RootState()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            // Act
            var rootState = flow.RootState;

            // Assert
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REDRESS_CREATE")), rootState);
        }

        [TestMethod]
        [ExpectedException(typeof(FlowException))]
        public void Bootstrap_Already_Bootstrapped()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.Bootstrapped = true;

            try
            {
                // Act
                flow.Bootstrap();
            }
            catch (FlowException ex)
            {
                // Assert
                Assert.AreEqual($"{flow.Name} already {nameof(flow.Bootstrapped)}.", ex.Message);

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FlowException))]
        public void Bootstrap_ConfigureFlowClass_Invalid()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.ConfigureFlowClass = "Invalid ConfigureFlowClass";

            try
            {
                // Act
                flow.Bootstrap();
            }
            catch (FlowException ex)
            {
                // Assert
                Assert.AreEqual($"Can't resolve {flow.ConfigureFlowClass}", ex.Message);

                throw;
            }
        }

        [TestMethod]
        public void Bootstrap_No_History()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            // Act
            flow.Bootstrap();

            // Assert active state
            Assert.AreEqual(flow.RootState, flow.States.FirstState());
            Assert.AreEqual(flow.ActiveState, flow.States.FirstState());

            // Assert sample of parent and substates
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")), flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")).ParentState);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")), flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION")).ParentState);
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).SubStates.Contains(flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION"))));
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).SubStates.Contains(flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION"))));

            // Assert sample of transition states
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REDRESS_CREATE")).Transitions.Contains(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT"))));
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).Transitions.Contains(flow.States.First(s => s.StateCode.Equals("REFUND_REVIEW"))));
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REFUND_REVIEW")).Transitions.Contains(flow.States.First(s => s.StateCode.Equals("REDRESS_CREATE"))));
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REFUND_REVIEW")).Transitions.Contains(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT"))));
            Assert.IsTrue(flow.States.First(s => s.StateCode.Equals("REFUND_REVIEW")).Transitions.Contains(flow.States.First(s => s.StateCode.Equals("REDRESS_REVIEW"))));
        }

        [TestMethod]
        public void Bootstrap_With_History()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
