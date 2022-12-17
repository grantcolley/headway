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

            // Assert
            Assert.AreEqual(flow.RootState, rootState);
            Assert.AreEqual(flow.ActiveState, rootState);
        }
    }
}