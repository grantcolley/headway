using Headway.Core.Enums;
using Headway.Core.Exceptions;
using Headway.Core.Extensions;
using Headway.Core.Model;
using Headway.Core.Tests.Helpers;
using Headway.SeedData.RemediatR;

namespace Headway.Core.Tests
{
    [TestClass]
    public class StateTests
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
        public async Task Initialise_ActiveState_No_SubStates()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            // Act
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
        }

        [TestMethod]
        public async Task Initialise_State_Has_SubStates()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            // Act
            await flow.States.Find(s => s.StateCode.Equals("REFUND_ASSESSMENT")).InitialiseAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(StateStatus.InProgress, flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")).StateStatus);
            Assert.AreEqual(StateStatus.NotStarted, flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION")).StateStatus);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")), flow.ActiveState);
        }

        [TestMethod]
        [ExpectedException(typeof(StateException))]
        public async Task Initialise_State_Already_InProgress()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";

            flow.ConfigureFlowClass = "Headway.Core.Tests.Helpers.FlowHelper, Headway.Core.Tests";

            flow.Bootstrap();

            flow.States[1].StateStatus = StateStatus.InProgress;

            try
            {
                // Act
                await flow.States[1].InitialiseAsync().ConfigureAwait(false);
            }
            catch (StateException ex)
            {
                // Assert
                Assert.AreEqual($"Can't initialize {flow.States[1].StateStatus} because it's already {StateStatus.InProgress}.", ex.Message);

                throw;
            }
        }

        [TestMethod]
        public async Task Initialise_State_Has_Actions_Configure_At_Bootstrap()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.ConfigureFlowClass = "Headway.Core.Tests.Helpers.FlowHelper, Headway.Core.Tests";

            flow.Bootstrap();

            // Act
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(flow.States.FirstState(), flow.ActiveState);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
            Assert.AreEqual($"1 Initialize {flow.ActiveState.StateCode}; 2 Initialize {flow.ActiveState.StateCode}", flow.ActiveState.Comment);
        }

        [TestMethod]
        [ExpectedException(typeof(StateException))]
        public async Task Initialise_State_Has_Actions_ConfigureStateClass_Invalid()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            flow.ActiveState.ConfigureStateClass = "Invalid ConfigureStateClass";

            try
            {
                // Act
                await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);
            }
            catch (StateException ex)
            {
                // Assert
                Assert.AreEqual($"Can't resolve {flow.ActiveState.ConfigureStateClass}", ex.Message);

                throw;
            }
        }

        [TestMethod]
        public async Task Initialise_State_Has_Actions_Configure_At_State_Initialise()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            flow.ActiveState.ConfigureStateClass = "Headway.Core.Tests.Helpers.StateHelper, Headway.Core.Tests";

            // Act
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(flow.States.FirstState(), flow.ActiveState);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
            Assert.AreEqual($"3 Initialize {flow.ActiveState.StateCode}; 4 Initialize {flow.ActiveState.StateCode}", flow.ActiveState.Comment);
        }

        [TestMethod]
        public async Task Initialise_State_Has_Actions_Configure_At_Bootstrap_And_State_Initialise()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.ConfigureFlowClass = "Headway.Core.Tests.Helpers.FlowHelper, Headway.Core.Tests";

            flow.Bootstrap();

            flow.ActiveState.ConfigureStateClass = "Headway.Core.Tests.Helpers.StateHelper, Headway.Core.Tests";

            var activeStateComment = $"1 Initialize {flow.ActiveState.StateCode}; 2 Initialize {flow.ActiveState.StateCode}; 3 Initialize {flow.ActiveState.StateCode}; 4 Initialize {flow.ActiveState.StateCode}";

            // Act
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(flow.States.FirstState(), flow.ActiveState);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
            Assert.AreEqual(activeStateComment, flow.ActiveState.Comment);
        }

        [TestMethod]
        public async Task Initialise_State_Has_Actions_Configure_All_At_Bootstrap()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.ConfigureStatesDuringBootstrap = true;
            flow.ConfigureFlowClass = "Headway.Core.Tests.Helpers.FlowHelper, Headway.Core.Tests";
            flow.States.FirstState().ConfigureStateClass = "Headway.Core.Tests.Helpers.StateHelper, Headway.Core.Tests";

            flow.Bootstrap();

            var activeStateComment = $"1 Initialize {flow.ActiveState.StateCode}; 2 Initialize {flow.ActiveState.StateCode}; 3 Initialize {flow.ActiveState.StateCode}; 4 Initialize {flow.ActiveState.StateCode}";

            // Act
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(flow.States.FirstState(), flow.ActiveState);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
            Assert.AreEqual(activeStateComment, flow.ActiveState.Comment);
        }

        [TestMethod]
        public async Task Complete_With_Transition_Default()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";

            flow.Bootstrap();

            await flow.ActiveState.InitialiseAsync();

            // Act
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(StateStatus.Completed, flow.States[0].StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States[1].StateStatus);
            Assert.AreEqual(flow.States[1], flow.ActiveState);
        }

        [TestMethod]
        public async Task Complete_With_Transition_To_StateCode()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";

            flow.Bootstrap();

            await flow.ActiveState.InitialiseAsync();

            // Act
            await flow.ActiveState.CompleteAsync(flow.States[1].StateCode).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(StateStatus.Completed, flow.States[0].StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States[1].StateStatus);
            Assert.AreEqual(flow.States[1], flow.ActiveState);
        }

        [TestMethod]
        public async Task Complete_With_Transition_To_State_Has_Substates()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            await flow.ActiveState.InitialiseAsync();

            // Act
            await flow.ActiveState.CompleteAsync("REFUND_ASSESSMENT").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(StateStatus.Completed, flow.States.FirstState().StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")).StateStatus);
            Assert.AreEqual(StateStatus.NotStarted, flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION")).StateStatus);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")), flow.ActiveState);
        }

        [TestMethod]
        [ExpectedException(typeof(StateException))]
        public async Task Complete_Already_Completed()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.Bootstrap();

            flow.ActiveState.StateStatus = StateStatus.Completed;

            try
            {
                // Act
                await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            }
            catch (StateException ex)
            {
                // Assert
                Assert.AreEqual($"Can't complete {flow.ActiveState.StateCode} because it's already {StateStatus.Completed}.", ex.Message);

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(StateException))]
        public async Task Complete_Already_NotStarted()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.Bootstrap();

            flow.ActiveState.StateStatus = StateStatus.NotStarted;

            try
            {
                // Act
                await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            }
            catch (StateException ex)
            {
                // Assert
                Assert.AreEqual($"Can't complete {flow.ActiveState.StateCode} because it's still {StateStatus.NotStarted}.", ex.Message);

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(StateException))]
        public async Task Complete_SubState_Not_Completed()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            flow.ActiveState = flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT"));
            flow.ActiveState.StateStatus = StateStatus.InProgress;

            var rc = flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION"));
            rc.StateStatus = StateStatus.InProgress;

            var rv = flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION"));
            rv.StateStatus = StateStatus.NotStarted;

            var joinedDescriptions = $"{rc.StateCode}={rc.StateStatus},{rv.StateCode}={rv.StateStatus}";

            try
            {
                // Act
                await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            }
            catch (StateException ex)
            {
                // Assert
                Assert.AreEqual($"Can't complete {flow.ActiveState.StateCode} because sub states not yet {StateStatus.Completed} : {joinedDescriptions}.", ex.Message);

                throw;
            }
        }

        [TestMethod]
        public async Task Complete_Last_SubState_Then_Complete_Parent()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            var ra = flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT"));
            ra.StateStatus = StateStatus.InProgress;

            var rv = flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION"));
            rv.StateStatus = StateStatus.Completed;

            flow.ActiveState = flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION"));
            flow.ActiveState.StateStatus = StateStatus.InProgress;

            // Act
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(StateStatus.Completed, flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).StateStatus);
            Assert.AreEqual(StateStatus.Completed, flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")).StateStatus);
            Assert.AreEqual(StateStatus.Completed, flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION")).StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_REVIEW")), flow.ActiveState);
        }

        [TestMethod]
        public async Task Complete_State_Has_Actions()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";

            flow.Bootstrap();

            flow.ActiveState.StateStatus = StateStatus.InProgress;
            flow.ActiveState.ConfigureStateClass = "Headway.Core.Tests.Helpers.StateHelper, Headway.Core.Tests";

            // Act
            await flow.ActiveState.CompleteAsync(flow.States[1].StateCode).ConfigureAwait(false);

            // Assert
            Assert.AreEqual($"1 Complete {flow.States[0].StateCode}; 2 Complete {flow.States[0].StateCode}", flow.States[0].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.States[0].StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States[1].StateStatus);
            Assert.AreEqual(flow.States[1], flow.ActiveState);
        }

        [TestMethod]
        public async Task Auto_Type_Pass_Straight_Through()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(3);

            flow.States[1].StateType = StateType.Auto;

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";
            flow.States[1].TransitionStateCodes = $"{flow.States[2].StateCode}";

            flow.Bootstrap();

            await flow.ActiveState.InitialiseAsync();

            // Act
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            //Assert
            Assert.AreEqual(StateStatus.Completed, flow.States[0].StateStatus);
            Assert.AreEqual(StateStatus.Completed, flow.States[1].StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States[2].StateStatus);
            Assert.AreEqual(flow.States[2], flow.ActiveState);
        }

        [TestMethod]
        public async Task Auto_Type_Runtime_ReRoute_To_Last_State()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(4);

            flow.States[1].StateType = StateType.Auto;

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";
            flow.States[1].TransitionStateCodes = $"{flow.States[2].StateCode};{flow.States[3].StateCode}";

            flow.Bootstrap();

            flow.States[1].ConfigureStateClass = "Headway.Core.Tests.Helpers.StateRoutingHelper, Headway.Core.Tests";

            await flow.ActiveState.InitialiseAsync();

            // Act
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            //Assert
            Assert.AreEqual(StateStatus.Completed, flow.States[0].StateStatus);
            Assert.AreEqual(StateStatus.Completed, flow.States[1].StateStatus);
            Assert.AreEqual(StateStatus.NotStarted, flow.States[2].StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States[3].StateStatus);
            Assert.AreEqual(flow.States[3], flow.ActiveState);
            Assert.AreEqual($"Route {flow.States[1].StateCode} to {flow.ActiveState.StateCode}", flow.States[1].Comment);
            Assert.AreEqual($"Routed from {flow.States[1].StateCode}", flow.ActiveState.Comment);
        }

        [TestMethod]
        public async Task History_Record_Transitions()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            // Act
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            //Assert
            Assert.AreEqual(9, flow.History.Count);

            Assert.AreEqual("Initialize", flow.History[0].Event);
            Assert.AreEqual(flow.States[0].StateCode, flow.History[0].StateCode);
            Assert.AreEqual(StateStatus.InProgress, flow.History[0].StateStatus);

            Assert.AreEqual(StateStatus.Completed, flow.States[0].StateStatus);
            Assert.AreEqual("Completed", flow.History[1].Event);
            Assert.AreEqual(flow.States[0].StateCode, flow.History[1].StateCode);
            Assert.AreEqual(flow.States[0].StateStatus, flow.History[1].StateStatus);

            Assert.AreEqual("Initialize", flow.History[2].Event);
            Assert.AreEqual(flow.States[1].StateCode, flow.History[2].StateCode);
            Assert.AreEqual(StateStatus.InProgress, flow.History[2].StateStatus);

            Assert.AreEqual("Initialize", flow.History[3].Event);
            Assert.AreEqual(flow.States[2].StateCode, flow.History[3].StateCode);
            Assert.AreEqual(StateStatus.InProgress, flow.History[3].StateStatus);

            Assert.AreEqual(StateStatus.Completed, flow.States[2].StateStatus);
            Assert.AreEqual("Completed", flow.History[4].Event);
            Assert.AreEqual(flow.States[2].StateCode, flow.History[4].StateCode);
            Assert.AreEqual(flow.States[2].StateStatus, flow.History[4].StateStatus);

            Assert.AreEqual("Initialize", flow.History[5].Event);
            Assert.AreEqual(flow.States[3].StateCode, flow.History[5].StateCode);
            Assert.AreEqual(StateStatus.InProgress, flow.History[5].StateStatus);

            Assert.AreEqual(StateStatus.Completed, flow.States[3].StateStatus);
            Assert.AreEqual("Completed", flow.History[6].Event);
            Assert.AreEqual(flow.States[3].StateCode, flow.History[6].StateCode);
            Assert.AreEqual(flow.States[3].StateStatus, flow.History[6].StateStatus);

            Assert.AreEqual(StateStatus.Completed, flow.States[1].StateStatus);
            Assert.AreEqual("Completed", flow.History[7].Event);
            Assert.AreEqual(flow.States[1].StateCode, flow.History[7].StateCode);
            Assert.AreEqual(flow.States[1].StateStatus, flow.History[7].StateStatus);

            Assert.AreEqual(StateStatus.InProgress, flow.States[4].StateStatus);
            Assert.AreEqual("Initialize", flow.History[8].Event);
            Assert.AreEqual(flow.States[4].StateCode, flow.History[8].StateCode);
            Assert.AreEqual(flow.States[4].StateStatus, flow.History[8].StateStatus);

            Assert.AreEqual(flow.ActiveState, flow.States[4]);
        }

        [TestMethod]
        public async Task Reset_No_Transition()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            flow.ActiveState.StateStatus = StateStatus.InProgress;
            flow.ActiveState.Comment = "Hello World!";
            flow.ActiveState.Owner = "state owner";

            // Act
            await flow.ActiveState.ResetAsync().ConfigureAwait(false);

            //Assert
            Assert.AreEqual(StateStatus.NotStarted, flow.ActiveState.StateStatus);
            Assert.IsNull(flow.ActiveState.Comment);
            Assert.IsNull(flow.ActiveState.Owner);
        }

        [TestMethod]
        public async Task Reset_No_Transition_State_Has_Actions_Configure_At_State_Reset()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            flow.ActiveState.ConfigureStateClass = "Headway.Core.Tests.Helpers.StateHelper, Headway.Core.Tests";

            // Act
            await flow.ActiveState.ResetAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(StateStatus.NotStarted, flow.ActiveState.StateStatus);
            Assert.IsNull(flow.ActiveState.Owner);
            Assert.AreEqual($"5 Reset {flow.ActiveState.StateCode}; 6 Reset {flow.ActiveState.StateCode}", flow.ActiveState.Comment);
        }

        [TestMethod]
        [ExpectedException(typeof(StateException))]
        public async Task Reset_To_Invalid_StateCode()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            var resetStateCode = "ABC";

            try
            {
                // Act
                await flow.ActiveState.ResetAsync(resetStateCode).ConfigureAwait(false);
            }
            catch (StateException ex)
            {
                // Assert
                Assert.AreEqual($"Can't reset {flow.ActiveState.StateCode} because it doesn't support resetting back to {resetStateCode}.", ex.Message);

                throw;
            }
        }

        [TestMethod]
        public void Test_The_Heck_Out_Of_Something()
        {
            // Arrange

            // Act

            //Assert

        }
    }
}
