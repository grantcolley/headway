using Headway.Core.Constants;
using Headway.Core.Enums;
using Headway.Core.Exceptions;
using Headway.Core.Extensions;
using Headway.Core.Model;
using Headway.Core.Tests.Helpers;
using Headway.SeedData.RemediatR;
using RemediatR.Core.Constants;
using System.Text.Json;

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

            flow.FlowConfigurationClass = "Invalid ConfigureFlowClass";

            try
            {
                // Act
                flow.Bootstrap();
            }
            catch (FlowException ex)
            {
                // Assert
                Assert.AreEqual($"Can't resolve {flow.FlowConfigurationClass}", ex.Message);

                throw;
            }
        }

        [TestMethod]
        public void Bootstrap_Not_Started()
        {
            // Arrange
            var flow = RemediatRFlow.CreateRemediatRFlow();

            // Act
            flow.Bootstrap();

            // Assert
            Assert.AreEqual(FlowStatus.NotStarted, flow.FlowStatus);
            Assert.AreEqual(flow.States.FirstState(), flow.RootState);
            Assert.AreEqual(flow.States.FirstState(), flow.ActiveState);
            Assert.AreEqual(flow.States.LastState(), flow.FinalState);
        }

        [TestMethod]
        public async Task Flow_InProgress()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.Bootstrap();

            // Act
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            //Assert
            Assert.AreEqual(FlowStatus.InProgress, flow.FlowStatus);
        }

        [TestMethod]
        public async Task Flow_Completed()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(2);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";

            flow.Bootstrap();

            // Act
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            //Assert
            Assert.AreEqual(FlowStatus.Completed, flow.FlowStatus);
            Assert.AreEqual(flow.FinalState, flow.ActiveState);
        }

        [TestMethod]
        public async Task Flow_Completed_Regress_To_Prior_State_Flow_InProgress()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(3);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";
            flow.States[1].TransitionStateCodes = $"{flow.States[2].StateCode}";
            flow.States[2].RegressionStateCodes = $"{flow.States[1].StateCode}";

            flow.Bootstrap();

            // Act
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            Assert.AreEqual(FlowStatus.Completed, flow.FlowStatus);

            await flow.FinalState.ResetAsync(flow.States[1].StateCode);

            //Assert
            Assert.AreEqual(FlowStatus.InProgress, flow.FlowStatus);
            Assert.AreEqual(flow.ActiveState, flow.States[1]);
        }

        [TestMethod]
        public async Task Flow_Completed_Regress_To_Root_State_Flow_InProgress()
        {
            // Arrange
            var flow = FlowHelper.CreateFlow(3);

            flow.States[0].TransitionStateCodes = $"{flow.States[1].StateCode}";
            flow.States[1].TransitionStateCodes = $"{flow.States[2].StateCode}";
            flow.States[2].RegressionStateCodes = $"{flow.States[0].StateCode}";

            flow.Bootstrap();

            // Act
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            await flow.ActiveState.CompleteAsync().ConfigureAwait(false);

            Assert.AreEqual(FlowStatus.Completed, flow.FlowStatus);

            await flow.FinalState.ResetAsync(flow.States[0].StateCode);

            //Assert
            Assert.AreEqual(FlowStatus.InProgress, flow.FlowStatus);
            Assert.AreEqual(flow.RootState, flow.ActiveState);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
        }

        [TestMethod]
        public async Task Flow_RemediatR_REDRESS_CREATE_To_FINAL_REVIEW()
        {
            // Arrange
            var historyRedressCreateToFinalReview = GetHistoryRedressCreateToFinalReview();

            var flow = RemediatRFlow.CreateRemediatRFlow();
            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowOwnershipHelper, Headway.Core.Tests";

            // Act
            flow.Bootstrap();
            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            while (flow.FlowStatus.Equals(FlowStatus.InProgress))
            {
                await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            }

            // Assert
            Assert.AreEqual(FlowStatus.Completed, flow.FlowStatus);
            Assert.AreEqual(flow.States.LastState(), flow.ActiveState);
            Assert.AreEqual(StateStatus.Completed, flow.ActiveState.StateStatus);
            Assert.AreEqual(historyRedressCreateToFinalReview.Count, flow.History.Count);

            for (int i = 0; i < flow.History.Count; i++)
            {
                Assert.AreEqual(historyRedressCreateToFinalReview[i].Event, flow.History[i].Event);
                Assert.AreEqual(historyRedressCreateToFinalReview[i].StateCode, flow.History[i].StateCode);
                Assert.AreEqual(historyRedressCreateToFinalReview[i].StateStatus, flow.History[i].StateStatus);
                Assert.AreEqual(historyRedressCreateToFinalReview[i].Owner, flow.History[i].Owner);
                Assert.AreEqual(historyRedressCreateToFinalReview[i].Comment, flow.History[i].Comment);
            }
        }

        [TestMethod]
        public async Task Flow_RemediatR_FINAL_REVIEW_Reset_To_REDRESS_REVIEW()
        {
            // Arrange
            var historyRedressCreateToFinalReview = GetHistoryRedressCreateToFinalReview();
            var historyFinalReviewResetToRedressReview = GetHistoryFinalReviewResetToRedressReview();

            var flow = RemediatRFlow.CreateRemediatRFlow();
            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowOwnershipHelper, Headway.Core.Tests";

            flow.Bootstrap();

            await flow.ActiveState.InitialiseAsync().ConfigureAwait(false);

            while (flow.FlowStatus.Equals(FlowStatus.InProgress))
            {
                await flow.ActiveState.CompleteAsync().ConfigureAwait(false);
            }

            // Act
            await flow.ActiveState.ResetAsync("REDRESS_REVIEW").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(FlowStatus.InProgress, flow.FlowStatus);
            Assert.AreEqual("REDRESS_REVIEW", flow.ActiveState.StateCode);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);

            var historyCount = historyRedressCreateToFinalReview.Count + historyFinalReviewResetToRedressReview.Count;
            Assert.AreEqual(historyCount, flow.History.Count);

            for (int i = 0; i < historyRedressCreateToFinalReview.Count; i++)
            {
                Assert.AreEqual(historyRedressCreateToFinalReview[i].Event, flow.History[i].Event);
                Assert.AreEqual(historyRedressCreateToFinalReview[i].StateCode, flow.History[i].StateCode);
                Assert.AreEqual(historyRedressCreateToFinalReview[i].StateStatus, flow.History[i].StateStatus);
                Assert.AreEqual(historyRedressCreateToFinalReview[i].Owner, flow.History[i].Owner);
                Assert.AreEqual(historyRedressCreateToFinalReview[i].Comment, flow.History[i].Comment);
            }

            for (int i = 0; i < historyFinalReviewResetToRedressReview.Count; i++)
            {
                var n = historyRedressCreateToFinalReview.Count + i;
                Assert.AreEqual(historyFinalReviewResetToRedressReview[i].Event, flow.History[n].Event);
                Assert.AreEqual(historyFinalReviewResetToRedressReview[i].StateCode, flow.History[n].StateCode);
                Assert.AreEqual(historyFinalReviewResetToRedressReview[i].StateStatus, flow.History[n].StateStatus);
                Assert.AreEqual(historyFinalReviewResetToRedressReview[i].Owner, flow.History[n].Owner);
                Assert.AreEqual(historyFinalReviewResetToRedressReview[i].Comment, flow.History[n].Comment);
            }
        }

        [TestMethod]
        public void Flow_RemediatR_REDRESS_CREATE_To_FINAL_REVIEW_ReplayHistory()
        {
            // Arrange
            var history = GetHistoryRedressCreateToFinalReview();
            var expectedReplayHistory = GetHistoryRedressCreateToFinalReviewReplay();

            var flow = RemediatRFlow.CreateRemediatRFlow();
            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowOwnershipHelper, Headway.Core.Tests";

            // Act
            flow.Bootstrap(history);

            // Assert
            Assert.AreEqual(flow.FinalState, flow.ActiveState);
            Assert.AreEqual(StateStatus.Completed, flow.ActiveState.StateStatus);
            Assert.AreEqual(FlowStatus.Completed, flow.FlowStatus);

            var lastIndex = flow.History.Count - 1;

            var lastHistory = flow.ReplayHistory.Last();

            Assert.AreEqual(lastHistory.FlowCode, flow.History[lastIndex].FlowCode);
            Assert.AreEqual(lastHistory.StateCode, flow.History[lastIndex].StateCode);
            Assert.AreEqual(lastHistory.StateStatus, flow.History[lastIndex].StateStatus);
            Assert.AreEqual(lastHistory.Owner, flow.History[lastIndex].Owner);

            Assert.AreEqual(flow.FinalState.Flow.FlowCode, flow.History[lastIndex].FlowCode);
            Assert.AreEqual(flow.FinalState.StateCode, flow.History[lastIndex].StateCode);
            Assert.AreEqual(flow.FinalState.StateStatus, flow.History[lastIndex].StateStatus);
            Assert.AreEqual(flow.FinalState.Owner, flow.History[lastIndex].Owner);
            Assert.AreEqual(FlowHistoryEvents.COMPLETE, flow.History[lastIndex].Event);

            for (int i = 0; i < expectedReplayHistory.Count; i++)
            {
                Assert.AreEqual(expectedReplayHistory[i].Event, flow.ReplayHistory[i].Event);
                Assert.AreEqual(expectedReplayHistory[i].StateCode, flow.ReplayHistory[i].StateCode);
                Assert.AreEqual(expectedReplayHistory[i].StateStatus, flow.ReplayHistory[i].StateStatus);
                Assert.AreEqual(expectedReplayHistory[i].Owner, flow.ReplayHistory[i].Owner);
                Assert.AreEqual(expectedReplayHistory[i].Comment, flow.ReplayHistory[i].Comment);
            }
        }

        [TestMethod]
        public void Flow_RemediatR_REDRESS_CREATE_To_FINAL_REVIEW_To_REDRESS_REVIEW_ReplayHistory()
        {
            // Arrange
            var history = GetHistoryRedressCreateToFinalReview();
            history.AddRange(GetHistoryFinalReviewResetToRedressReview());

            var flow = RemediatRFlow.CreateRemediatRFlow();
            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowOwnershipHelper, Headway.Core.Tests";

            // Act
            flow.Bootstrap(history);

            // Assert

            var redressReview = flow.StateDictionary[RemediatRFlowCodes.REDRESS_REVIEW_CODE];

            Assert.AreEqual(redressReview, flow.ActiveState);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
            Assert.AreEqual(FlowStatus.InProgress, flow.FlowStatus);

            var lastIndex = flow.History.Count - 1;
            var lastHistory = flow.ReplayHistory.Last();

            Assert.AreEqual(lastHistory.FlowCode, flow.History[lastIndex].FlowCode);
            Assert.AreEqual(lastHistory.StateCode, flow.History[lastIndex].StateCode);
            Assert.AreEqual(lastHistory.StateStatus, flow.History[lastIndex].StateStatus);
            Assert.AreEqual(lastHistory.Owner, flow.History[lastIndex].Owner);

            Assert.AreEqual(redressReview.Flow.FlowCode, flow.History[lastIndex].FlowCode);
            Assert.AreEqual(redressReview.StateCode, flow.History[lastIndex].StateCode);
            Assert.AreEqual(redressReview.StateStatus, flow.History[lastIndex].StateStatus);
            Assert.AreEqual(redressReview.Owner, flow.History[lastIndex].Owner);
            Assert.AreEqual(FlowHistoryEvents.START, flow.History[lastIndex].Event);
        }

        [TestMethod]
        public async Task Flow_RemediatR_Regress_REDRESS_CREATE_To_REFUND_REVIEW_Reset_To_REFUND_ASSESSMENT()
        {
            // Arrange
            var history = GetHistoryRedressCreateToRefundReview();

            var flow = RemediatRFlow.CreateRemediatRFlow();
            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowOwnershipHelper, Headway.Core.Tests";
            
            flow.Bootstrap(history);

            // Act
            await flow.ActiveState.ResetAsync(RemediatRFlowCodes.REFUND_ASSESSMENT_CODE).ConfigureAwait(false);

            // Assert
            var refundAssessment = flow.StateDictionary[RemediatRFlowCodes.REFUND_ASSESSMENT_CODE];
            var refundCalculation = flow.StateDictionary[RemediatRFlowCodes.REFUND_CALCULATION_CODE];
            var expectedHistory = GetHistoryRedressCreateToRefundReviewResetToRefundAssessment();
            var expectedReplayHistory = GetHistoryRedressCreateToRefundReviewResetToRefundAssessmentReplay();

            Assert.AreEqual(refundCalculation, flow.ActiveState);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
            Assert.AreEqual(StateStatus.InProgress, refundAssessment.StateStatus);
            Assert.AreEqual(FlowStatus.InProgress, flow.FlowStatus);

            for (int i = 0; i < expectedHistory.Count; i++)
            {
                Assert.AreEqual(expectedHistory[i].Event, flow.History[i].Event);
                Assert.AreEqual(expectedHistory[i].StateCode, flow.History[i].StateCode);
                Assert.AreEqual(expectedHistory[i].StateStatus, flow.History[i].StateStatus);
                Assert.AreEqual(expectedHistory[i].Owner, flow.History[i].Owner);
                Assert.AreEqual(expectedHistory[i].Comment, flow.History[i].Comment);
            }

            for (int i = 0; i < expectedReplayHistory.Count; i++)
            {
                Assert.AreEqual(expectedReplayHistory[i].Event, flow.ReplayHistory[i].Event);
                Assert.AreEqual(expectedReplayHistory[i].StateCode, flow.ReplayHistory[i].StateCode);
                Assert.AreEqual(expectedReplayHistory[i].StateStatus, flow.ReplayHistory[i].StateStatus);
                Assert.AreEqual(expectedReplayHistory[i].Owner, flow.ReplayHistory[i].Owner);
                Assert.AreEqual(expectedReplayHistory[i].Comment, flow.ReplayHistory[i].Comment);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(StateException))]
        public async Task Flow_RemediatR_Expected_Exception_Regress_REFUND_REVIEW_Reset_To_REFUND_VERIFICATION()
        {
            // Arrange
            var history = GetHistoryRedressCreateToRefundReview();

            var flow = RemediatRFlow.CreateRemediatRFlow();
            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowOwnershipHelper, Headway.Core.Tests";

            var refundReview = flow.States.Single(s => s.StateCode.Equals(RemediatRFlowCodes.REFUND_REVIEW_CODE));

            refundReview.RegressionStateCodes = refundReview.RegressionStateCodes + ";" + RemediatRFlowCodes.REFUND_VERIFICATION_CODE;

            flow.Bootstrap(history);

            try
            {
                // Act
                await flow.ActiveState.ResetAsync(RemediatRFlowCodes.REFUND_VERIFICATION_CODE).ConfigureAwait(false);
            }
            catch (StateException ex)
            {
                // Assert
                Assert.AreEqual($"Can't regress to sub state {RemediatRFlowCodes.REFUND_VERIFICATION_CODE} of {RemediatRFlowCodes.REFUND_ASSESSMENT_CODE} because it doesn't share the same parent as {refundReview.StateCode}.", ex.Message);

                throw;
            }
        }

        [TestMethod]
        public async Task Flow_RemediatR_Regress_REFUND_REVIEW_Reset_To_REDRESS_CREATE()
        {
            // Arrange
            var history = GetHistoryRedressCreateToRefundReview();

            var flow = RemediatRFlow.CreateRemediatRFlow();
            flow.FlowConfigurationClass = "Headway.Core.Tests.Helpers.FlowOwnershipHelper, Headway.Core.Tests";

            flow.Bootstrap(history);

            // Act
            await flow.ActiveState.ResetAsync(RemediatRFlowCodes.REDRESS_CREATE_CODE).ConfigureAwait(false);

            // Assert
            var redressCreate = flow.StateDictionary[RemediatRFlowCodes.REDRESS_CREATE_CODE];
            var expectedHistory = GetHistoryRedressCreateToRefundReviewResetToRedressCreate();

            Assert.AreEqual(redressCreate, flow.ActiveState);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
            Assert.AreEqual(FlowStatus.InProgress, flow.FlowStatus);
            Assert.AreEqual(1, flow.ReplayHistory.Count);
            Assert.AreEqual(flow.History.Last(), flow.ReplayHistory.Single());
            Assert.AreEqual(expectedHistory.Count, flow.History.Count);

            for (int i = 0; i < expectedHistory.Count; i++)
            {
                Assert.AreEqual(expectedHistory[i].Event, flow.History[i].Event);
                Assert.AreEqual(expectedHistory[i].StateCode, flow.History[i].StateCode);
                Assert.AreEqual(expectedHistory[i].StateStatus, flow.History[i].StateStatus);
                Assert.AreEqual(expectedHistory[i].Owner, flow.History[i].Owner);
                Assert.AreEqual(expectedHistory[i].Comment, flow.History[i].Comment);
            }
        }

        [TestMethod]
        public async Task Flow_TakeOwnership_State_InProgress_ReplayHistory()
        {
            // Arrange
            var history = GetHistoryRedressCreateToRefundReview();
            var expectedReplayHistoryList = GetReplayHistoryRedressCreateToRefundReviewTakeOwnership();

            var permissions = new List<string>
            {
                RemediatRAuthorisation.REFUND_ASSESSOR_WRITE
            };

            var authorisation = AuthorisationHelper.CreateAuthorisation(permissions);

            var flow = RemediatRFlow.CreateRemediatRFlow();

            flow.Bootstrap(history);

            // Act
            flow.ActiveState.RelinquishOwnership(authorisation);

            await flow.ActiveState.TakeOwnershipAsync(authorisation).ConfigureAwait(false);

            var flowHistory = flow.ReplayFlowHistory();

            // Assert
            var replayHistory = JsonSerializer.Serialize<List<FlowHistory>>(flow.ReplayHistory, new JsonSerializerOptions { WriteIndented = true });
            var expectedReplayHistory = JsonSerializer.Serialize<List<FlowHistory>>(expectedReplayHistoryList, new JsonSerializerOptions { WriteIndented = true });
            var refundReview = flow.StateDictionary[RemediatRFlowCodes.REFUND_REVIEW_CODE];

            Assert.AreSame(refundReview, flow.ActiveState);
            Assert.AreEqual(StateStatus.InProgress, flow.ActiveState.StateStatus);
            Assert.AreEqual(Environment.UserName, flow.ActiveState.Owner);
            Assert.AreEqual(RemediatRFlowCodes.REFUND_REVIEW_CODE, flowHistory.StateCode);
            Assert.AreEqual(FlowHistoryEvents.TAKE_OWNERSHIP, flowHistory.Event);
            Assert.AreEqual(Environment.UserName, flowHistory.Owner);
            Assert.AreEqual(StateStatus.InProgress, flowHistory.StateStatus);
            Assert.AreEqual(expectedReplayHistory, replayHistory);
        }

        private static List<FlowHistory> GetHistoryRedressCreateToFinalReview()
        {
            var jsonHistoryRedressCreateToFinalReview = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "RemediatR_Flow_REDRESS_CREATE_To_FINAL_REVIEW.txt"));
            var historyRedressCreateToFinalReview = JsonSerializer.Deserialize<List<FlowHistory>>(jsonHistoryRedressCreateToFinalReview);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (var history in historyRedressCreateToFinalReview)
            {
                if (history.Owner == "dummy_account")
                {
                    history.Owner = Environment.UserName;
                    if (!string.IsNullOrWhiteSpace(history.Comment))
                    {
                        history.Comment = history.Comment.Replace("dummy_account", Environment.UserName);
                    }
                }
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return historyRedressCreateToFinalReview;
        }

        private static List<FlowHistory> GetHistoryRedressCreateToRefundReview()
        {
            var jsonHistoryRedressCreateToRefundReview = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "RemediatR_Flow_REDRESS_CREATE_To_REFUND_REVIEW.txt"));
            var historyRedressCreateToRefundReview = JsonSerializer.Deserialize<List<FlowHistory>>(jsonHistoryRedressCreateToRefundReview);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (var history in historyRedressCreateToRefundReview)
            {
                if (history.Owner == "dummy_account")
                {
                    history.Owner = Environment.UserName;
                    if (!string.IsNullOrWhiteSpace(history.Comment))
                    {
                        history.Comment = history.Comment.Replace("dummy_account", Environment.UserName);
                    }
                }
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return historyRedressCreateToRefundReview;
        }

        private static List<FlowHistory> GetHistoryFinalReviewResetToRedressReview()
        {
            var jsonHistoryFinalReviewResetToRedressReview = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "RemediatR_Flow_FINAL_REVIEW_Reset_To_REDRESS_REVIEW.txt"));
            var historyFinalReviewResetToRedressReview = JsonSerializer.Deserialize<List<FlowHistory>>(jsonHistoryFinalReviewResetToRedressReview);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (var history in historyFinalReviewResetToRedressReview)
            {
                if (history.Owner == "dummy_account")
                {
                    history.Owner = Environment.UserName;
                    if (!string.IsNullOrWhiteSpace(history.Comment))
                    {
                        history.Comment = history.Comment.Replace("dummy_account", Environment.UserName);
                    }
                }
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return historyFinalReviewResetToRedressReview;
        }

        private static List<FlowHistory> GetHistoryRedressCreateToRefundReviewResetToRedressCreate()
        {
            var jsonHistory = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "RemediatR_Flow_REFUND_REVIEW_Reset_To_REDRESS_CREATE.txt"));
            var history = JsonSerializer.Deserialize<List<FlowHistory>>(jsonHistory);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (var h in history)
            {
                if (h.Owner == "dummy_account")
                {
                    h.Owner = Environment.UserName;
                    if (!string.IsNullOrWhiteSpace(h.Comment))
                    {
                        h.Comment = h.Comment.Replace("dummy_account", Environment.UserName);
                    }
                }
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return history;
        }

        private static List<FlowHistory> GetHistoryRedressCreateToRefundReviewResetToRefundAssessment()
        {
            var jsonHistory = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "RemediatR_Flow_REDRESS_CREATE_To_REFUND_REVIEW_Reset_To_REFUND_ASSESSMENT.txt"));
            var history = JsonSerializer.Deserialize<List<FlowHistory>>(jsonHistory);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (var h in history)
            {
                if (h.Owner == "dummy_account")
                {
                    h.Owner = Environment.UserName;
                    if (!string.IsNullOrWhiteSpace(h.Comment))
                    {
                        h.Comment = h.Comment.Replace("dummy_account", Environment.UserName);
                    }
                }
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return history;
        }

        private static List<FlowHistory> GetHistoryRedressCreateToRefundReviewResetToRefundAssessmentReplay()
        {
            var jsonHistory = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "RemediatR_Flow_REDRESS_CREATE_To_REFUND_REVIEW_Reset_To_REFUND_ASSESSMENT_Replay.txt"));
            var history = JsonSerializer.Deserialize<List<FlowHistory>>(jsonHistory);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (var h in history)
            {
                if (h.Owner == "dummy_account")
                {
                    h.Owner = Environment.UserName;
                    if (!string.IsNullOrWhiteSpace(h.Comment))
                    {
                        h.Comment = h.Comment.Replace("dummy_account", Environment.UserName);
                    }
                }
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return history;
        }

        private static List<FlowHistory> GetHistoryRedressCreateToFinalReviewReplay()
        {
            var jsonHistory = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "RemediatR_Flow_REDRESS_CREATE_To_FINAL_REVIEW_Replay.txt"));
            var history = JsonSerializer.Deserialize<List<FlowHistory>>(jsonHistory);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (var h in history)
            {
                if (h.Owner == "dummy_account")
                {
                    h.Owner = Environment.UserName;
                    if (!string.IsNullOrWhiteSpace(h.Comment))
                    {
                        h.Comment = h.Comment.Replace("dummy_account", Environment.UserName);
                    }
                }
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return history;
        }

        private static List<FlowHistory> GetReplayHistoryRedressCreateToRefundReviewTakeOwnership()
        {
            var jsonHistory = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "RemediatR_Flow_TakeOwnership_State_InProgress_ReplayHistory.txt"));
            var history = JsonSerializer.Deserialize<List<FlowHistory>>(jsonHistory);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (var h in history)
            {
                if (h.Owner == "dummy_account")
                {
                    h.Owner = Environment.UserName;
                    if (!string.IsNullOrWhiteSpace(h.Comment))
                    {
                        h.Comment = h.Comment.Replace("dummy_account", Environment.UserName);
                    }
                }
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return history;
        }
    }
}
