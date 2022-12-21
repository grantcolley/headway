using Headway.Core.Enums;
using Headway.Core.Extensions;
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
            var flow = RemediatRFlow.CreateRemediatRFlow();

            // Act
            var rootState = flow.RootState;

            // Assert
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REDRESS_CREATE")), rootState);
        }

        [TestMethod]
        public async Task State_Complete_Transition_REDRESS_CREATE_to_REFUND_ASSESSMENT()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();
            flow.Bootstrap();

            // Act
            await flow.ActiveState.CompleteAsync("REFUND_ASSESSMENT").ConfigureAwait(false);

            //Assert
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REDRESS_CREATE")).StateStatus, StateStatus.Completed);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).StateStatus, StateStatus.InProgress);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")).StateStatus, StateStatus.InProgress);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")), flow.ActiveState);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION")).StateStatus, StateStatus.NotStarted);
        }

        [TestMethod]
        public void State_Complete_Last_SubState()
        {
            // Arrange

            // Act

            //Assert

        }
    }
}