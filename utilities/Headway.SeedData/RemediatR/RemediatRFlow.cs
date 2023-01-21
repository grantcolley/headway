using Headway.Core.Enums;
using Headway.Core.Model;
using RemediatR.Core.Constants;
using RemediatR.Core.Model;

namespace Headway.SeedData.RemediatR
{
    public class RemediatRFlow
    {
        public static Flow ResressFlow {get;set;}

        public static Flow CreateRemediatRFlow()
        {
            var redressCreate = new State { Position = 1, Name = "Redress Create", StateCode = "REDRESS_CREATE" };
            var refundAssessment = new State { Position = 10, Name = "Refund Assessment", StateCode = "REFUND_ASSESSMENT" };
            var refundCalculation = new State { Position = 11, Name = "Refund Calculation", StateCode = "REFUND_CALCULATION" };
            var refundVerification = new State { Position = 12, Name = "Refund Verification", StateCode = "REFUND_VERIFICATION" };
            var refundReview = new State { Position = 20, Name = "Refund Review", StateCode = "REFUND_REVIEW" };
            var redressReview = new State { Position = 30, Name = "Redress Review", StateCode = "REDRESS_REVIEW" };
            var redressValidation = new State { Position = 40, Name = "Redress Validation", StateCode = "REDRESS_VALIDATION" };
            var communicationGeneration = new State { Position = 50, Name = "Communication Generation", StateCode = "COMMUNICATION_GENERATION" };
            var communicationDispatch = new State { Position = 60, Name = "Communication Dispatch", StateCode = "COMMUNICATION_DISPATCH" };
            var responseRequired = new State { Position = 70, Name = "Response Required", StateCode = "RESPONSE_REQUIRED" };
            var awaitingResponse = new State { Position = 80, Name = "Awaiting Response", StateCode = "AWAITING_RESPONSE" };
            var paymentGeneration = new State { Position = 90, Name = "Payment Generation", StateCode = "PAYMENT_GENERATION" };
            var finalRedressReview = new State { Position = 100, Name = "Final Redress Review", StateCode = "FINAL_REDRESS_REVIEW" };

            redressCreate.StateType = StateType.Standard;
            redressCreate.Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER;
            redressCreate.TransitionStateCodes = $"{refundAssessment.StateCode}";

            refundAssessment.StateType= StateType.Standard;
            refundAssessment.Permissions = RemediatRAuthorisation.REFUND_ASSESSOR;
            refundAssessment.SubStateCodes = $"{refundCalculation.StateCode};{refundVerification.StateCode}";
            refundAssessment.TransitionStateCodes = $"{refundReview.StateCode}";

            refundCalculation.StateType = StateType.Standard;
            refundCalculation.Permissions = RemediatRAuthorisation.REFUND_ASSESSOR;
            refundCalculation.ParentStateCode = $"{refundAssessment.StateCode}";
            refundCalculation.TransitionStateCodes = $"{refundVerification.StateCode}";

            refundVerification.StateType = StateType.Standard;
            refundVerification.Permissions = RemediatRAuthorisation.REFUND_ASSESSOR;
            refundVerification.ParentStateCode = $"{refundAssessment.StateCode}";

            refundReview.StateType = StateType.Standard;
            refundReview.Permissions = RemediatRAuthorisation.REFUND_REVIEWER;
            refundReview.TransitionStateCodes = $"{redressReview.StateCode}";
            refundReview.RegressionStateCodes = $"{refundAssessment.StateCode};{redressCreate.StateCode}";

            redressReview.StateType = StateType.Standard;
            redressReview.Permissions = RemediatRAuthorisation.REDRESS_REVIEWER;
            redressReview.TransitionStateCodes = $"{redressValidation.StateCode}";
            redressReview.RegressionStateCodes = $"{redressCreate.StateCode}";

            redressValidation.StateType = StateType.Auto;
            redressValidation.Permissions = RemediatRAuthorisation.REDRESS_REVIEWER;
            redressValidation.TransitionStateCodes = $"{communicationGeneration.StateCode};{paymentGeneration.StateCode}";
            redressValidation.RegressionStateCodes = $"{redressReview.StateCode}";

            communicationGeneration.StateType = StateType.Standard;
            communicationGeneration.Permissions = RemediatRAuthorisation.REDRESS_REVIEWER;
            communicationGeneration.TransitionStateCodes = $"{communicationDispatch.StateCode}";
            communicationGeneration.RegressionStateCodes = $"{redressReview.StateCode}";

            communicationDispatch.StateType = StateType.Standard;
            communicationDispatch.Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER;
            communicationDispatch.TransitionStateCodes = $"{responseRequired.StateCode}";
            communicationDispatch.RegressionStateCodes = $"{redressReview.StateCode}";

            responseRequired.StateType = StateType.Auto;
            responseRequired.Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER;
            responseRequired.TransitionStateCodes = $"{awaitingResponse.StateCode};{paymentGeneration.StateCode}";
            responseRequired.RegressionStateCodes = $"{communicationDispatch.StateCode}";

            awaitingResponse.StateType = StateType.Standard;
            awaitingResponse.Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER;
            awaitingResponse.TransitionStateCodes = $"{paymentGeneration.StateCode}";
            awaitingResponse.RegressionStateCodes = $"{redressReview.StateCode}";

            paymentGeneration.StateType = StateType.Standard;
            paymentGeneration.Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER;
            paymentGeneration.TransitionStateCodes = $"{finalRedressReview.StateCode}";
            paymentGeneration.RegressionStateCodes = $"{communicationDispatch.StateCode};{redressReview.StateCode}";

            finalRedressReview.StateType = StateType.Standard;
            finalRedressReview.Permissions = RemediatRAuthorisation.REDRESS_REVIEWER;
            finalRedressReview.RegressionStateCodes = $"{redressReview.StateCode}";

            ResressFlow = new Flow
            {
                Name = "RemediatR",
                Permissions = $"{RemediatRAuthorisation.REDRESS_READ}",
                FlowConfigurationClass = "RemediatR.Core.Flow.RedressFlowConfiguration, RemediatR.Core",
                ConfigureStatesDuringBootstrap = true,
                Context = new Redress()
            };

            ResressFlow.States.Add(redressCreate);
            ResressFlow.States.Add(refundAssessment);
            ResressFlow.States.Add(refundCalculation);
            ResressFlow.States.Add(refundVerification);
            ResressFlow.States.Add(refundReview);
            ResressFlow.States.Add(redressReview);
            ResressFlow.States.Add(redressValidation);
            ResressFlow.States.Add(communicationGeneration);
            ResressFlow.States.Add(communicationDispatch);
            ResressFlow.States.Add(responseRequired);
            ResressFlow.States.Add(awaitingResponse);
            ResressFlow.States.Add(paymentGeneration);
            ResressFlow.States.Add(finalRedressReview);

            foreach(var state in ResressFlow.States)
            {
                state.Flow = ResressFlow;
                state.Context = ResressFlow.Context;
            }

            return ResressFlow;
        }
    }
}
