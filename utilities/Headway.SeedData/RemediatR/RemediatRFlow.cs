using Headway.Core.Model;
using RemediatR.Core.Constants;

namespace Headway.SeedData.RemediatR
{
    public class RemediatRFlow
    {
        public static Flow FlowCreate()
        {
            var redressCreate = new State { Name = "Redress Create", Code = "REDRESS_CREATE", Position = 1, Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var refundAssessment = new State { Name = "Refund Assessment", Code = "REFUND_ASSESSMENT", Position = 10, Permissions = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundCalculation = new State { Name = "Refund Calculation", Code = "REFUND_CALCULATION", Position = 11, Permissions = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundVerification = new State { Name = "Refund Verification", Code = "REFUND_VERIFICATION", Position = 12, Permissions = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundReview = new State { Name = "Refund Review", Code = "REFUND_REVIEW", Position = 20, Permissions = RemediatRAuthorisation.REFUND_REVIEWER };
            var redressReview = new State { Name = "Redress Review", Code = "REDRESS_REVIEW", Position = 30, Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };
            var redressValidation = new State { Name = "Redress Validation", Code = "REDRESS_VALIDATE", Position = 40, Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };
            var communicationGeneration = new State { Name = "Communication Generation", Code = "COMMUNICATION_GENERATION", Position = 50, Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };
            var communicationDispatch = new State { Name = "Communication Dispatch", Code = "COMMUNICATION_DISPATCH", Position = 60, Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var responseRequired = new State { Name = "Response Required", Code = "RESPONSE_REQUIRED", Position = 70, Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var awaitingResponse = new State { Name = "Awaiting Response", Code = "AWAITING_RESPONSE", Position = 80, Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var paymentGeneration = new State { Name = "Payment Generation", Code = "PAYMENT_GENERATION", Position = 90, Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var finalRedressReview = new State { Name = "Final Redress Review", Code = "FINAL_REDRESS_REVIEW", Position = 100, Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };

            redressCreate.TransitionStateCodes = $"{refundAssessment.Code}";

            refundAssessment.SubStateCodes = $"{refundCalculation.Code};{refundVerification.Code}";
            refundAssessment.TransitionStateCodes = $"{redressCreate.Code};{refundReview.Code}";

            refundCalculation.TransitionStateCodes = $"{refundVerification.Code}";

            refundReview.TransitionStateCodes = $"{refundAssessment.Code};{redressReview.Code}";

            redressReview.TransitionStateCodes = $"{redressCreate.Code};{redressValidation.Code}";

            redressValidation.TransitionStateCodes = $"{redressReview.Code};{communicationGeneration.Code};{paymentGeneration.Code}";

            communicationGeneration.TransitionStateCodes = $"{redressReview.Code};{communicationDispatch.Code}";

            communicationDispatch.TransitionStateCodes = $"{redressReview.Code};{responseRequired.Code}";

            responseRequired.TransitionStateCodes = $"{communicationDispatch.Code};{awaitingResponse.Code};{paymentGeneration.Code}";

            awaitingResponse.TransitionStateCodes = $"{redressReview.Code};{paymentGeneration.Code}";

            paymentGeneration.TransitionStateCodes = $"{communicationDispatch.Code};{finalRedressReview.Code}";

            finalRedressReview.TransitionStateCodes = $"{redressReview.Code}";

            var flow = new Flow
            {
                Name = "RemediatR",
                Model = "RemediatR.Core.Model.Redress, RemediatR.Core", //typeof(Redress).Name,
                Permissions = $"{RemediatRAuthorisation.REDRESS_READ}"
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

            return flow;
        }
    }
}
