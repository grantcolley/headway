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
            var redressCreate = new State { Position = 1, Name = RemediatRFlowCodes.REDRESS_CREATE, StateCode = RemediatRFlowCodes.REDRESS_CREATE_CODE };
            var refundAssessment = new State { Position = 10, Name = RemediatRFlowCodes.REFUND_ASSESSMENT, StateCode = RemediatRFlowCodes.REFUND_ASSESSMENT_CODE };
            var refundCalculation = new State { Position = 11, Name = RemediatRFlowCodes.REFUND_CALCULATION, StateCode = RemediatRFlowCodes.REFUND_CALCULATION_CODE };
            var refundVerification = new State { Position = 12, Name = RemediatRFlowCodes.REFUND_VERIFICATION, StateCode = RemediatRFlowCodes.REFUND_VERIFICATION_CODE };
            var refundReview = new State { Position = 20, Name = RemediatRFlowCodes.REFUND_REVIEW, StateCode = RemediatRFlowCodes.REFUND_REVIEW_CODE };
            var redressReview = new State { Position = 30, Name = RemediatRFlowCodes.REDRESS_REVIEW, StateCode = RemediatRFlowCodes.REDRESS_REVIEW_CODE };
            var redressValidation = new State { Position = 40, Name = RemediatRFlowCodes.REDRESS_VALIDATION, StateCode = RemediatRFlowCodes.REDRESS_VALIDATION_CODE };
            var communicationGeneration = new State { Position = 50, Name = RemediatRFlowCodes.COMMUNICATION_GENERATION, StateCode = RemediatRFlowCodes.COMMUNICATION_GENERATION_CODE };
            var communicationDispatch = new State { Position = 60, Name = RemediatRFlowCodes.COMMUNICATION_DISPATCH, StateCode = RemediatRFlowCodes.COMMUNICATION_DISPATCH_CODE };
            var responseRequired = new State { Position = 70, Name = RemediatRFlowCodes.RESPONSE_REQUIRED, StateCode = RemediatRFlowCodes.RESPONSE_REQUIRED_CODE };
            var awaitingResponse = new State { Position = 80, Name = RemediatRFlowCodes.AWAITING_RESPONSE, StateCode = RemediatRFlowCodes.AWAITING_RESPONSE_CODE };
            var paymentGeneration = new State { Position = 90, Name = RemediatRFlowCodes.PAYMENT_GENERATION, StateCode = RemediatRFlowCodes.PAYMENT_GENERATION_CODE };
            var finalRedressReview = new State { Position = 100, Name = RemediatRFlowCodes.FINAL_REDRESS_REVIEW, StateCode = RemediatRFlowCodes.FINAL_REDRESS_REVIEW_CODE };

            redressCreate.StateType = StateType.Standard;
            redressCreate.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            redressCreate.WritePermission = RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE;
            redressCreate.TransitionStateCodes = $"{refundAssessment.StateCode}";
            redressCreate.IsOwnerRestricted = true;

            refundAssessment.StateType= StateType.Parent;
            refundAssessment.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            refundAssessment.WritePermission = RemediatRAuthorisation.REFUND_ASSESSOR_WRITE;
            refundAssessment.SubStateCodes = $"{refundCalculation.StateCode};{refundVerification.StateCode}";
            refundAssessment.TransitionStateCodes = $"{refundReview.StateCode}";

            refundCalculation.StateType = StateType.Standard;
            refundCalculation.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            refundCalculation.WritePermission = RemediatRAuthorisation.REFUND_ASSESSOR_WRITE;
            refundCalculation.ParentStateCode = $"{refundAssessment.StateCode}";
            refundCalculation.TransitionStateCodes = $"{refundVerification.StateCode}";
            refundCalculation.IsOwnerRestricted = true;

            refundVerification.StateType = StateType.Standard;
            refundVerification.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            refundVerification.WritePermission = RemediatRAuthorisation.REFUND_ASSESSOR_WRITE;
            refundVerification.ParentStateCode = $"{refundAssessment.StateCode}";
            refundVerification.IsOwnerRestricted = true;

            refundReview.StateType = StateType.Standard;
            refundReview.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            refundReview.WritePermission = RemediatRAuthorisation.REFUND_ASSESSOR_WRITE;
            refundReview.TransitionStateCodes = $"{redressReview.StateCode}";
            refundReview.RegressionStateCodes = $"{refundAssessment.StateCode};{redressCreate.StateCode}";
            refundReview.IsOwnerRestricted = true;

            redressReview.StateType = StateType.Standard;
            redressReview.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            redressReview.WritePermission = RemediatRAuthorisation.REDRESS_REVIEWER_WRITE;
            redressReview.TransitionStateCodes = $"{redressValidation.StateCode}";
            redressReview.RegressionStateCodes = $"{redressCreate.StateCode}";
            redressReview.IsOwnerRestricted = true;

            redressValidation.StateType = StateType.Auto;
            redressValidation.AutoActionResult = StateAutoActionResult.AutoComplete;
            redressValidation.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            redressValidation.WritePermission = RemediatRAuthorisation.REDRESS_REVIEWER_WRITE;
            redressValidation.TransitionStateCodes = $"{communicationGeneration.StateCode};{paymentGeneration.StateCode}";
            redressValidation.RegressionStateCodes = $"{redressReview.StateCode}";

            communicationGeneration.StateType = StateType.Standard;
            communicationGeneration.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            communicationGeneration.WritePermission = RemediatRAuthorisation.REDRESS_REVIEWER_WRITE;
            communicationGeneration.TransitionStateCodes = $"{communicationDispatch.StateCode}";
            communicationGeneration.RegressionStateCodes = $"{redressReview.StateCode}";

            communicationDispatch.StateType = StateType.Standard;
            communicationDispatch.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            communicationDispatch.WritePermission = RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE;
            communicationDispatch.TransitionStateCodes = $"{responseRequired.StateCode}";
            communicationDispatch.RegressionStateCodes = $"{redressReview.StateCode}";
            communicationDispatch.IsOwnerRestricted = true;

            responseRequired.StateType = StateType.Auto;
            responseRequired.AutoActionResult = StateAutoActionResult.AutoComplete;
            responseRequired.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            responseRequired.WritePermission = RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE;
            responseRequired.TransitionStateCodes = $"{awaitingResponse.StateCode};{paymentGeneration.StateCode}";
            responseRequired.RegressionStateCodes = $"{communicationDispatch.StateCode}";

            awaitingResponse.StateType = StateType.Standard;
            awaitingResponse.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            awaitingResponse.WritePermission = RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE;
            awaitingResponse.TransitionStateCodes = $"{paymentGeneration.StateCode}";
            awaitingResponse.RegressionStateCodes = $"{redressReview.StateCode}";
            awaitingResponse.IsOwnerRestricted = true;

            paymentGeneration.StateType = StateType.Standard;
            paymentGeneration.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            paymentGeneration.WritePermission = RemediatRAuthorisation.REDRESS_CASE_OWNER_WRITE;
            paymentGeneration.TransitionStateCodes = $"{finalRedressReview.StateCode}";
            paymentGeneration.RegressionStateCodes = $"{communicationDispatch.StateCode};{redressReview.StateCode}";

            finalRedressReview.StateType = StateType.Standard;
            finalRedressReview.ReadPermission = RemediatRAuthorisation.REDRESS_READ;
            finalRedressReview.WritePermission = RemediatRAuthorisation.REDRESS_REVIEWER_WRITE;
            finalRedressReview.RegressionStateCodes = $"{redressReview.StateCode}";
            finalRedressReview.IsOwnerRestricted = true;

            ResressFlow = new Flow
            {
                Name = RemediatRFlowCodes.REMEDIATR,
                FlowCode = RemediatRFlowCodes.REMEDIATR_CODE,
                Permission = $"{RemediatRAuthorisation.REDRESS_READ}",
                FlowConfigurationClass = "RemediatR.Core.Flow.RedressFlowConfiguration, RemediatR.Core",
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
