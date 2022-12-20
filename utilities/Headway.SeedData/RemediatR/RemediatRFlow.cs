using Headway.Core.Model;
using RemediatR.Core.Constants;
using RemediatR.Core.Model;

namespace Headway.SeedData.RemediatR
{
    public class RemediatRFlow
    {
        public static Flow FlowCreate()
        {
            var redressCreate = new State { Position = 1, Name = "Redress Create", StateCode = "REDRESS_CREATE", Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var refundAssessment = new State { Position = 10, Name = "Refund Assessment", StateCode = "REFUND_ASSESSMENT", Permissions = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundCalculation = new State { Position = 11, Name = "Refund Calculation", StateCode = "REFUND_CALCULATION", Permissions = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundVerification = new State { Position = 12, Name = "Refund Verification", StateCode = "REFUND_VERIFICATION", Permissions = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundReview = new State { Position = 20, Name = "Refund Review", StateCode = "REFUND_REVIEW", Permissions = RemediatRAuthorisation.REFUND_REVIEWER };
            var redressReview = new State { Position = 30, Name = "Redress Review", StateCode = "REDRESS_REVIEW", Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };
            var redressValidation = new State { Position = 40, Name = "Redress Validation", StateCode = "REDRESS_VALIDATE", Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };
            var communicationGeneration = new State { Position = 50, Name = "Communication Generation", StateCode = "COMMUNICATION_GENERATION", Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };
            var communicationDispatch = new State { Position = 60, Name = "Communication Dispatch", StateCode = "COMMUNICATION_DISPATCH", Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var responseRequired = new State { Position = 70, Name = "Response Required", StateCode = "RESPONSE_REQUIRED", Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var awaitingResponse = new State { Position = 80, Name = "Awaiting Response", StateCode = "AWAITING_RESPONSE", Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var paymentGeneration = new State { Position = 90, Name = "Payment Generation", StateCode = "PAYMENT_GENERATION", Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var finalRedressReview = new State { Position = 100, Name = "Final Redress Review", StateCode = "FINAL_REDRESS_REVIEW", Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };

            redressCreate.TransitionStateCodes = $"{refundAssessment.StateCode}";

            refundAssessment.SubStateCodes = $"{refundCalculation.StateCode};{refundVerification.StateCode}";
            refundAssessment.TransitionStateCodes = $"{redressCreate.StateCode};{refundReview.StateCode}";

            refundCalculation.ParentStateCode = $"{refundAssessment.StateCode}";
            refundCalculation.TransitionStateCodes = $"{refundVerification.StateCode}";

            refundVerification.ParentStateCode = $"{refundAssessment.StateCode}";

            refundReview.TransitionStateCodes = $"{refundAssessment.StateCode};{redressReview.StateCode}";

            redressReview.TransitionStateCodes = $"{redressCreate.StateCode};{redressValidation.StateCode}";

            redressValidation.TransitionStateCodes = $"{redressReview.StateCode};{communicationGeneration.StateCode};{paymentGeneration.StateCode}";

            communicationGeneration.TransitionStateCodes = $"{redressReview.StateCode};{communicationDispatch.StateCode}";

            communicationDispatch.TransitionStateCodes = $"{redressReview.StateCode};{responseRequired.StateCode}";

            responseRequired.TransitionStateCodes = $"{communicationDispatch.StateCode};{awaitingResponse.StateCode};{paymentGeneration.StateCode}";

            awaitingResponse.TransitionStateCodes = $"{redressReview.StateCode};{paymentGeneration.StateCode}";

            paymentGeneration.TransitionStateCodes = $"{communicationDispatch.StateCode};{finalRedressReview.StateCode}";

            finalRedressReview.TransitionStateCodes = $"{redressReview.StateCode}";

            var flow = new Flow
            {
                Name = "RemediatR",
                Model = "RemediatR.Core.Model.Redress, RemediatR.Core", //typeof(Redress).Name,
                Permissions = $"{RemediatRAuthorisation.REDRESS_READ}",
                Context = new Redress()
            };

            flow.States.Add(redressCreate);
            flow.States.Add(refundAssessment);
            flow.States.Add(refundCalculation);
            flow.States.Add(refundVerification);
            flow.States.Add(refundReview);
            flow.States.Add(redressReview);
            flow.States.Add(redressValidation);
            flow.States.Add(communicationGeneration);
            flow.States.Add(communicationDispatch);
            flow.States.Add(responseRequired);
            flow.States.Add(awaitingResponse);
            flow.States.Add(paymentGeneration);
            flow.States.Add(finalRedressReview);

            foreach(var state in flow.States)
            {
                state.Flow = flow;
                state.Context = flow.Context;
            }

            return flow;
        }
    }
}
