using Headway.Core.Enums;
using Headway.Core.Extensions;
using Headway.SeedData.RemediatR;

namespace Headway.Core.Tests
{
    [TestClass]
    public class RemediatRFlowTests
    {
        [TestMethod]
        public async Task State_Complete_Transition_REDRESS_CREATE_to_REFUND_ASSESSMENT()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();
            flow.Bootstrap();

            await flow.ActiveState.InitialiseAsync();

            // Act
            await flow.ActiveState.CompleteAsync("REFUND_ASSESSMENT").ConfigureAwait(false);

            //Assert
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REDRESS_CREATE")).StateStatus, StateStatus.Completed);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).StateStatus, StateStatus.InProgress);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")).StateStatus, StateStatus.InProgress);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")), flow.ActiveState);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION")).StateStatus, StateStatus.NotStarted);
        }
    }
}