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
            Assert.AreEqual(StateStatus.Uninitialized, flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION")).StateStatus);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")), flow.ActiveState);
        }

        [TestMethod]
        [ExpectedException(typeof(StateException))]
        public async Task Initialise_State_Already_InProgress()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";

            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowHelper, Headway.Core.Tests";

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
                Assert.AreEqual($"Can't initialize {flow.States[1].StateCode} because it's {StateStatus.InProgress}.", ex.Message);

                throw;
            }
        }

        [TestMethod]
        public async Task Initialise_State_Has_Actions_Configure_At_Bootstrap()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowHelper, Headway.Core.Tests";

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

            flow.ActiveState.StateConfigurationClass = "Invalid ConfigureStateClass";

            try
            {
                // Act
                await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);
            }
            catch (StateException ex)
            {
                // Assert
                Assert.AreEqual($"Can't resolve {flow.ActiveState.StateConfigurationClass}", ex.Message);

                throw;
            }
        }

        [TestMethod]
        public async Task Initialise_State_Has_Actions_Configure_At_State_Initialise()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            flow.ActiveState.StateConfigurationClass = "Headway.Core.Tests.Helpers.StateHelper, Headway.Core.Tests";

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

            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowHelper, Headway.Core.Tests";

            flow.Bootstrap();

            flow.ActiveState.StateConfigurationClass = "Headway.Core.Tests.Helpers.StateHelper, Headway.Core.Tests";

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
            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowHelper, Headway.Core.Tests";
            flow.States.FirstState().StateConfigurationClass = "Headway.Core.Tests.Helpers.StateHelper, Headway.Core.Tests";

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

            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

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

            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

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

            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            // Act
            await flow.ActiveState.CompleteAsync("REFUND_ASSESSMENT").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(StateStatus.Completed, flow.States.FirstState().StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")).StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION")).StateStatus);
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
                Assert.AreEqual($"Can't complete {flow.ActiveState.StateCode} because it's {StateStatus.Completed}.", ex.Message);

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

            flow.ActiveState.StateStatus = StateStatus.Uninitialized;

            try
            {
                // Act
                await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            }
            catch (StateException ex)
            {
                // Assert
                Assert.AreEqual($"Can't complete {flow.ActiveState.StateCode} because it's {StateStatus.Uninitialized}.", ex.Message);

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(StateException))]
        public async Task Complete_Invalid_Transition_StateCode()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.Bootstrap();

            flow.ActiveState.StateStatus = StateStatus.InProgress;
            flow.ActiveState.TransitionStateCode = "ABC";

            try
            {
                // Act
                await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            }
            catch (StateException ex)
            {
                // Assert
                Assert.AreEqual($"Can't complete {flow.ActiveState.StateCode} because it doesn't support transitioning to {flow.ActiveState.TransitionStateCode}.", ex.Message);

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
            rv.StateStatus = StateStatus.Uninitialized;

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

            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowOwnershipHelper, Headway.Core.Tests";

            flow.Bootstrap();

            // Act
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(StateStatus.Completed, flow.States.First(s => s.StateCode.Equals("REFUND_ASSESSMENT")).StateStatus);
            Assert.AreEqual(StateStatus.Completed, flow.States.First(s => s.StateCode.Equals("REFUND_CALCULATION")).StateStatus);
            Assert.AreEqual(StateStatus.Completed, flow.States.First(s => s.StateCode.Equals("REFUND_VERIFICATION")).StateStatus);
            Assert.AreEqual(flow.States.First(s => s.StateCode.Equals("REFUND_REVIEW")), flow.ActiveState);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
        }

        [TestMethod]
        public async Task Complete_State_Has_Actions()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";

            flow.Bootstrap();

            flow.ActiveState.StateStatus = StateStatus.InProgress;
            flow.ActiveState.StateConfigurationClass = "Headway.Core.Tests.Helpers.StateHelper, Headway.Core.Tests";

            // Act
            await flow.ActiveState.CompleteAsync(flow.States[1].StateCode).ConfigureAwait(false);

            // Assert
            Assert.AreEqual($"1 Completed {flow.States[0].StateCode}; 2 Completed {flow.States[0].StateCode}", flow.States[0].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.States[0].StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States[1].StateStatus);
            Assert.AreEqual(flow.States[1], flow.ActiveState);
        }

        [TestMethod]
        public async Task Auto_Type_StateAutoActionResult_AutoComplete()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(3);

            flow.States[1].StateType = StateType.Auto;

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";
            flow.States[1].TransitionStateCodes = $"{flow.States[2].StateCode}";

            flow.Bootstrap();

            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            // Act
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            //Assert
            Assert.AreEqual(StateStatus.Completed, flow.States[0].StateStatus);
            Assert.AreEqual(StateStatus.Completed, flow.States[1].StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States[2].StateStatus);
            Assert.AreEqual(flow.States[2], flow.ActiveState);
        }

        [TestMethod]
        public async Task Auto_Type_StateAutoActionResult_AutoRegress()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(3);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";

            flow.States[1].StateType = StateType.Auto;
            flow.States[1].RegressionStateCodes = flow.States[0].StateCode;
            flow.States[1].TransitionStateCodes = $"{flow.States[2].StateCode}";

            flow.Bootstrap();

            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            flow.States[1].AutoActionResult = StateAutoActionResult.AutoRegress;
            flow.States[1].RegressionStateCode = flow.States[0].StateCode;

            // Act
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            //Assert
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[0].StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[1].StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[2].StateStatus);
            Assert.AreEqual(flow.States[0], flow.ActiveState);
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

            flow.States[1].StateConfigurationClass = "Headway.Core.Tests.Helpers.StateRoutingHelper, Headway.Core.Tests";

            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            // Act
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            //Assert
            Assert.AreEqual(StateStatus.Completed, flow.States[0].StateStatus);
            Assert.AreEqual(StateStatus.Completed, flow.States[1].StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[2].StateStatus);
            Assert.AreEqual(StateStatus.InProgress, flow.States[3].StateStatus);
            Assert.AreEqual(flow.States[3], flow.ActiveState);
            Assert.AreEqual($"Route {flow.States[1].StateCode} to {flow.ActiveState.StateCode}", flow.States[1].Comment);
            Assert.AreEqual($"Routed from {flow.States[1].StateCode}", flow.ActiveState.Comment);
        }

        [TestMethod]
        public async Task History_Transition_States_Has_Actions_Assert_History()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowHistoryHelper, Headway.Core.Tests";

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
            Assert.AreEqual(flow.States[0].Owner, flow.History[0].Owner);
            Assert.AreEqual($"{flow.History[0].Event} {flow.States[0].StateCode}", flow.History[0].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[0].StateStatus);

            Assert.AreEqual(StateStatus.Completed, flow.States[0].StateStatus);
            Assert.AreEqual("Completed", flow.History[1].Event);
            Assert.AreEqual(flow.States[0].StateCode, flow.History[1].StateCode);
            Assert.AreEqual(flow.States[0].StateStatus, flow.History[1].StateStatus);
            Assert.AreEqual(flow.States[0].Owner, flow.History[1].Owner);
            Assert.AreEqual($"{flow.History[1].Event} {flow.States[0].StateCode}", flow.History[1].Comment);

            Assert.AreEqual("Initialize", flow.History[2].Event);
            Assert.AreEqual(flow.States[1].StateCode, flow.History[2].StateCode);
            Assert.AreEqual(flow.States[1].Owner, flow.History[2].Owner);
            Assert.AreEqual($"{flow.History[2].Event} {flow.States[1].StateCode}", flow.History[2].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[2].StateStatus);

            Assert.AreEqual("Initialize", flow.History[3].Event);
            Assert.AreEqual(flow.States[2].StateCode, flow.History[3].StateCode);
            Assert.AreEqual(flow.States[2].Owner, flow.History[3].Owner);
            Assert.AreEqual($"{flow.History[3].Event} {flow.States[2].StateCode}", flow.History[3].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[3].StateStatus);

            Assert.AreEqual(StateStatus.Completed, flow.States[2].StateStatus);
            Assert.AreEqual("Completed", flow.History[4].Event);
            Assert.AreEqual(flow.States[2].StateCode, flow.History[4].StateCode);
            Assert.AreEqual(flow.States[2].StateStatus, flow.History[4].StateStatus);
            Assert.AreEqual(flow.States[2].Owner, flow.History[4].Owner);
            Assert.AreEqual($"{flow.History[4].Event} {flow.States[2].StateCode}", flow.History[4].Comment);

            Assert.AreEqual("Initialize", flow.History[5].Event);
            Assert.AreEqual(flow.States[3].StateCode, flow.History[5].StateCode);
            Assert.AreEqual(flow.States[3].Owner, flow.History[5].Owner);
            Assert.AreEqual($"{flow.History[5].Event} {flow.States[3].StateCode}", flow.History[5].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[5].StateStatus);

            Assert.AreEqual(StateStatus.Completed, flow.States[3].StateStatus);
            Assert.AreEqual("Completed", flow.History[6].Event);
            Assert.AreEqual(flow.States[3].StateCode, flow.History[6].StateCode);
            Assert.AreEqual(flow.States[3].StateStatus, flow.History[6].StateStatus);
            Assert.AreEqual(flow.States[3].Owner, flow.History[6].Owner);
            Assert.AreEqual($"{flow.History[6].Event} {flow.States[3].StateCode}", flow.History[6].Comment);

            Assert.AreEqual(StateStatus.Completed, flow.States[1].StateStatus);
            Assert.AreEqual("Completed", flow.History[7].Event);
            Assert.AreEqual(flow.States[1].StateCode, flow.History[7].StateCode);
            Assert.AreEqual(flow.States[1].StateStatus, flow.History[7].StateStatus);
            Assert.AreEqual(flow.States[1].Owner, flow.History[7].Owner);
            Assert.AreEqual($"{flow.History[7].Event} {flow.States[1].StateCode}", flow.History[7].Comment);

            Assert.AreEqual(StateStatus.InProgress, flow.States[4].StateStatus);
            Assert.AreEqual("Initialize", flow.History[8].Event);
            Assert.AreEqual(flow.States[4].StateCode, flow.History[8].StateCode);
            Assert.AreEqual(flow.States[4].StateStatus, flow.History[8].StateStatus);
            Assert.AreEqual(flow.States[4].Owner, flow.History[8].Owner);
            Assert.AreEqual($"{flow.History[8].Event} {flow.States[4].StateCode}", flow.History[8].Comment);

            Assert.AreEqual(flow.ActiveState, flow.States[4]);
        }

        [TestMethod]
        public async Task Regression_To_Root_State_Assert_History()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowHistoryHelper, Headway.Core.Tests";

            flow.Bootstrap();

            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            Assert.AreEqual(flow.ActiveState, flow.States[4]);

            // Act
            await flow.ActiveState.ResetAsync(flow.RootState.StateCode).ConfigureAwait(false);

            //Assert
            Assert.AreEqual(14, flow.History.Count);

            Assert.AreEqual("Initialize", flow.History[0].Event);
            Assert.AreEqual(flow.States[0].StateCode, flow.History[0].StateCode);
            Assert.AreEqual($"{flow.History[0].Event} {flow.States[0].StateCode}", flow.History[0].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[0].StateStatus);

            Assert.AreEqual("Completed", flow.History[1].Event);
            Assert.AreEqual(flow.States[0].StateCode, flow.History[1].StateCode);
            Assert.AreEqual($"{flow.History[1].Event} {flow.States[0].StateCode}", flow.History[1].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.History[1].StateStatus);

            Assert.AreEqual("Initialize", flow.History[2].Event);
            Assert.AreEqual(flow.States[1].StateCode, flow.History[2].StateCode);
            Assert.AreEqual($"{flow.History[2].Event} {flow.States[1].StateCode}", flow.History[2].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[2].StateStatus);

            Assert.AreEqual("Initialize", flow.History[3].Event);
            Assert.AreEqual(flow.States[2].StateCode, flow.History[3].StateCode);
            Assert.AreEqual($"{flow.History[3].Event} {flow.States[2].StateCode}", flow.History[3].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[3].StateStatus);

            Assert.AreEqual("Completed", flow.History[4].Event);
            Assert.AreEqual(flow.States[2].StateCode, flow.History[4].StateCode);
            Assert.AreEqual($"{flow.History[4].Event} {flow.States[2].StateCode}", flow.History[4].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.History[4].StateStatus);

            Assert.AreEqual("Initialize", flow.History[5].Event);
            Assert.AreEqual(flow.States[3].StateCode, flow.History[5].StateCode);
            Assert.AreEqual($"{flow.History[5].Event} {flow.States[3].StateCode}", flow.History[5].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[5].StateStatus);

            Assert.AreEqual("Completed", flow.History[6].Event);
            Assert.AreEqual(flow.States[3].StateCode, flow.History[6].StateCode);
            Assert.AreEqual($"{flow.History[6].Event} {flow.States[3].StateCode}", flow.History[6].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.History[6].StateStatus);

            Assert.AreEqual("Completed", flow.History[7].Event);
            Assert.AreEqual(flow.States[1].StateCode, flow.History[7].StateCode);
            Assert.AreEqual($"{flow.History[7].Event} {flow.States[1].StateCode}", flow.History[7].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.History[7].StateStatus);

            Assert.AreEqual("Initialize", flow.History[8].Event);
            Assert.AreEqual(flow.States[4].StateCode, flow.History[8].StateCode);
            Assert.AreEqual($"{flow.History[8].Event} {flow.States[4].StateCode}", flow.History[8].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[8].StateStatus);

            Assert.AreEqual("Reset", flow.History[9].Event);
            Assert.AreEqual(Environment.UserName, flow.History[9].Owner);
            Assert.AreEqual($"{flow.History[9].Event} {flow.States[4].StateCode}", flow.History[9].Comment);
            Assert.IsNull(flow.States[4].Owner);
            Assert.IsNull(flow.States[4].Comment);
            Assert.AreEqual(flow.States[4].StateStatus, flow.History[9].StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[4].StateStatus);

            Assert.AreEqual("Reset", flow.History[10].Event);
            Assert.AreEqual(Environment.UserName, flow.History[10].Owner);
            Assert.AreEqual($"{flow.History[10].Event} {flow.States[3].StateCode}", flow.History[10].Comment);
            Assert.IsNull(flow.States[3].Owner);
            Assert.IsNull(flow.States[3].Comment);
            Assert.AreEqual(flow.States[3].StateStatus, flow.History[10].StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[3].StateStatus);

            Assert.AreEqual("Reset", flow.History[11].Event);
            Assert.AreEqual(Environment.UserName, flow.History[11].Owner);
            Assert.AreEqual($"{flow.History[11].Event} {flow.States[2].StateCode}", flow.History[11].Comment);
            Assert.IsNull(flow.States[2].Owner);
            Assert.IsNull(flow.States[2].Comment);
            Assert.AreEqual(flow.States[2].StateStatus, flow.History[11].StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[2].StateStatus);

            Assert.AreEqual("Reset", flow.History[12].Event);
            Assert.AreEqual(Environment.UserName, flow.History[12].Owner);
            Assert.AreEqual($"{flow.History[12].Event} {flow.States[1].StateCode}", flow.History[12].Comment);
            Assert.IsNull(flow.States[1].Owner);
            Assert.IsNull(flow.States[1].Comment);
            Assert.AreEqual(flow.States[1].StateStatus, flow.History[12].StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[1].StateStatus);

            Assert.AreEqual("Reset", flow.History[13].Event);
            Assert.AreEqual(Environment.UserName, flow.History[13].Owner);
            Assert.AreEqual($"{flow.History[13].Event} {flow.States[0].StateCode}", flow.History[13].Comment);
            Assert.IsNull(flow.States[0].Owner);
            Assert.IsNull(flow.States[0].Comment);
            Assert.AreEqual(flow.States[0].StateStatus, flow.History[13].StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[0].StateStatus);

            Assert.AreEqual(flow.ActiveState, flow.RootState);
        }

        [TestMethod]
        public async Task Regression_To_Previous_State_Assert_History()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowHistoryHelper, Headway.Core.Tests";

            flow.Bootstrap();

            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            Assert.AreEqual(flow.ActiveState, flow.States[11]);

            // Act
            await flow.ActiveState.ResetAsync().ConfigureAwait(false);

            //Assert
            Assert.AreEqual(18, flow.History.Count);

            Assert.AreEqual("Initialize", flow.History[0].Event);
            Assert.AreEqual(flow.States[0].StateCode, flow.History[0].StateCode);
            Assert.AreEqual($"{flow.History[0].Event} {flow.States[0].StateCode}", flow.History[0].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[0].StateStatus);

            Assert.AreEqual("Completed", flow.History[1].Event);
            Assert.AreEqual(flow.States[0].StateCode, flow.History[1].StateCode);
            Assert.AreEqual($"{flow.History[1].Event} {flow.States[0].StateCode}", flow.History[1].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.History[1].StateStatus);

            Assert.AreEqual("Initialize", flow.History[2].Event);
            Assert.AreEqual(flow.States[1].StateCode, flow.History[2].StateCode);
            Assert.AreEqual($"{flow.History[2].Event} {flow.States[1].StateCode}", flow.History[2].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[2].StateStatus);

            Assert.AreEqual("Initialize", flow.History[3].Event);
            Assert.AreEqual(flow.States[2].StateCode, flow.History[3].StateCode);
            Assert.AreEqual($"{flow.History[3].Event} {flow.States[2].StateCode}", flow.History[3].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[3].StateStatus);

            Assert.AreEqual("Completed", flow.History[4].Event);
            Assert.AreEqual(flow.States[2].StateCode, flow.History[4].StateCode);
            Assert.AreEqual($"{flow.History[4].Event} {flow.States[2].StateCode}", flow.History[4].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.History[4].StateStatus);

            Assert.AreEqual("Initialize", flow.History[5].Event);
            Assert.AreEqual(flow.States[3].StateCode, flow.History[5].StateCode);
            Assert.AreEqual($"{flow.History[5].Event} {flow.States[3].StateCode}", flow.History[5].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[5].StateStatus);

            Assert.AreEqual("Completed", flow.History[6].Event);
            Assert.AreEqual(flow.States[3].StateCode, flow.History[6].StateCode);
            Assert.AreEqual($"{flow.History[6].Event} {flow.States[3].StateCode}", flow.History[6].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.History[6].StateStatus);

            Assert.AreEqual("Completed", flow.History[7].Event);
            Assert.AreEqual(flow.States[1].StateCode, flow.History[7].StateCode);
            Assert.AreEqual($"{flow.History[7].Event} {flow.States[1].StateCode}", flow.History[7].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.History[7].StateStatus);

            Assert.AreEqual("Initialize", flow.History[8].Event);
            Assert.AreEqual(flow.States[4].StateCode, flow.History[8].StateCode);
            Assert.AreEqual($"{flow.History[8].Event} {flow.States[4].StateCode}", flow.History[8].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[8].StateStatus);

            Assert.AreEqual("Completed", flow.History[9].Event);
            Assert.AreEqual(flow.States[4].StateCode, flow.History[9].StateCode);
            Assert.AreEqual($"{flow.History[9].Event} {flow.States[4].StateCode}", flow.History[9].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.History[9].StateStatus);

            Assert.AreEqual("Initialize", flow.History[10].Event);
            Assert.AreEqual(flow.States[5].StateCode, flow.History[10].StateCode);
            Assert.AreEqual($"{flow.History[10].Event} {flow.States[5].StateCode}", flow.History[10].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[10].StateStatus);

            Assert.AreEqual("Completed", flow.History[11].Event);
            Assert.AreEqual(flow.States[5].StateCode, flow.History[11].StateCode);
            Assert.AreEqual($"{flow.History[11].Event} {flow.States[5].StateCode}", flow.History[11].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.History[11].StateStatus);

            Assert.AreEqual("Initialize", flow.History[12].Event);
            Assert.AreEqual(flow.States[6].StateCode, flow.History[12].StateCode);
            Assert.AreEqual($"{flow.History[12].Event} {flow.States[6].StateCode}", flow.History[12].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[12].StateStatus);

            Assert.AreEqual("Completed", flow.History[13].Event);
            Assert.AreEqual(flow.States[6].StateCode, flow.History[13].StateCode);
            Assert.AreEqual($"{flow.History[13].Event} {flow.States[6].StateCode}", flow.History[13].Comment);
            Assert.AreEqual(StateStatus.Completed, flow.History[13].StateStatus);


            Assert.AreEqual("Initialize", flow.History[14].Event);
            Assert.AreEqual(flow.States[11].StateCode, flow.History[14].StateCode);
            Assert.AreEqual($"{flow.History[14].Event} {flow.States[11].StateCode}", flow.History[14].Comment);
            Assert.AreEqual(StateStatus.InProgress, flow.History[14].StateStatus);

            Assert.AreEqual("Reset", flow.History[15].Event);
            Assert.AreEqual(Environment.UserName, flow.History[15].Owner);
            Assert.AreEqual($"{flow.History[15].Event} {flow.States[11].StateCode}", flow.History[15].Comment);
            Assert.IsNull(flow.States[11].Owner);
            Assert.IsNull(flow.States[11].Comment);
            Assert.AreEqual(flow.States[11].StateStatus, flow.History[15].StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[11].StateStatus);

            Assert.AreEqual("Reset", flow.History[16].Event);
            Assert.AreEqual(Environment.UserName, flow.History[16].Owner);
            Assert.AreEqual($"{flow.History[16].Event} {flow.States[6].StateCode}", flow.History[16].Comment);
            Assert.IsNull(flow.States[6].Owner);
            Assert.IsNull(flow.States[6].Comment);
            Assert.AreEqual(flow.States[6].StateStatus, flow.History[16].StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[6].StateStatus);

            Assert.AreEqual("Reset", flow.History[17].Event);
            Assert.AreEqual(Environment.UserName, flow.History[17].Owner);
            Assert.AreEqual($"{flow.History[17].Event} {flow.States[5].StateCode}", flow.History[17].Comment);
            Assert.IsNull(flow.States[5].Owner);
            Assert.IsNull(flow.States[5].Comment);
            Assert.AreEqual(flow.States[5].StateStatus, flow.History[17].StateStatus);
            Assert.AreEqual(StateStatus.Uninitialized, flow.States[5].StateStatus);

            Assert.AreEqual(flow.ActiveState, flow.States[5]);
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
            Assert.AreEqual(StateStatus.Uninitialized, flow.ActiveState.StateStatus);
            Assert.IsNull(flow.ActiveState.Comment);
            Assert.IsNull(flow.ActiveState.Owner);
        }

        [TestMethod]
        public async Task Reset_No_Transition_State_Has_Actions_Configure_At_State_Reset()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            flow.ActiveState.StateConfigurationClass = "Headway.Core.Tests.Helpers.StateHelper, Headway.Core.Tests";

            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            // Act
            await flow.ActiveState.ResetAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(StateStatus.Uninitialized, flow.ActiveState.StateStatus);
            Assert.IsNull(flow.ActiveState.Owner);
            Assert.IsNull(flow.ActiveState.Comment);
            Assert.AreEqual($"3 Initialize {flow.ActiveState.StateCode}; 4 Initialize {flow.ActiveState.StateCode}; 5 Reset {flow.ActiveState.StateCode}; 6 Reset {flow.ActiveState.StateCode}", flow.History.Last().Comment);
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
                Assert.AreEqual($"Can't reset {flow.ActiveState.StateCode} because it doesn't support regressing back to {resetStateCode}.", ex.Message);

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(StateException))]
        public async Task Reset_State_NotStarted()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap();

            try
            {
                // Act
                await flow.ActiveState.ResetAsync().ConfigureAwait(false);
            }
            catch (StateException ex)
            {
                // Assert
                Assert.AreEqual($"Can't reset {flow.ActiveState.StateCode} because it is {StateStatus.Uninitialized}.", ex.Message);

                throw;
            }
        }
    }
}
