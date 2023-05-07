﻿using Headway.Core.Enums;
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
                Name = "RemediatR",
                FlowCode = "REMEDIATR",
                Permissions = $"{RemediatRAuthorisation.REDRESS_READ}",
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
