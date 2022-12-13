using Headway.Core.Enums;
using Headway.Core.Model;
using RemediatR.Core.Constants;
using RemediatR.Core.Model;

namespace Headway.SeedData.RemediatR
{
    public class RemediatRFlow
    {
        public static Flow FlowCreate()
        {
            var flow = new Flow
            {
                Name = "",
                Model = "RemediatR.Core.Model.Redress, RemediatR.Core", //typeof(Redress).Name,
                Permission = ""
            };

            var redressCreate = new State { Name = "Redress Create", Code = "REDRESS_CREATE", Position = 1, Permission = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var refundAssessment = new State { Name = "Refund Assessment", Code = "REFUND_ASSESSMENT", Position = 10, Permission = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundCalculation = new State { Name = "Refund Calculation", Code = "REFUND_CALCULATION", Position = 11, Permission = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundVerification = new State { Name = "Refund Verification", Code = "REFUND_VERIFICATION", Position = 12, Permission = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundReview = new State { Name = "Refund Review", Code = "REFUND_REVIEW", Position = 20, Permission = RemediatRAuthorisation.REFUND_REVIEWER };
            var redressReview = new State { Name = "Redress Review", Code = "REDRESS_REVIEW", Position = 30, Permission = RemediatRAuthorisation.REDRESS_REVIEWER };
            var redressValidation = new State { Name = "Redress Validation", Code = "REDRESS_VALIDATE", Position = 40, Permission = RemediatRAuthorisation.REDRESS_REVIEWER };
            var communicationGeneration = new State { Name = "Communication Generation", Code = "COMMUNICATION_GENERATION", Position = 50, Permission = RemediatRAuthorisation.REDRESS_REVIEWER };
            var communicationDispatch = new State { Name = "Communication Dispatch", Code = "COMMUNICATION_DISPATCH", Position = 60, Permission = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var responseRequired = new State { Name = "Response Required", Code = "RESPONSE_REQUIRED", Position = 70, Permission = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var awaitingResponse = new State { Name = "Awaiting Response", Code = "AWAITING_RESPONSE", Position = 80, Permission = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var paymentGeneration = new State { Name = "Payment Generation", Code = "PAYMENT_GENERATION", Position = 90, Permission = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var finalRedressReview = new State { Name = "Final Redress Review", Code = "FINAL_REDRESS_REVIEW", Position = 100, Permission = RemediatRAuthorisation.REDRESS_REVIEWER };

            redressCreate.Transitions.Add(refundAssessment);

            refundAssessment.SubStates.Add(refundCalculation);
            refundAssessment.SubStates.Add(refundVerification);
            refundAssessment.Transitions.Add(redressCreate);
            refundAssessment.Transitions.Add(refundReview);

            refundReview.Transitions.Add(refundAssessment);
            refundReview.Transitions.Add(redressReview);

            redressReview.Transitions.Add(redressCreate);
            redressReview.Transitions.Add(redressValidation);

            redressValidation.Transitions.Add(redressReview);
            redressValidation.Transitions.Add(communicationGeneration);
            redressValidation.Transitions.Add(paymentGeneration);

            communicationGeneration.Transitions.Add(redressReview);
            communicationGeneration.Transitions.Add(communicationDispatch);

            communicationDispatch.Transitions.Add(redressReview);
            communicationDispatch.Transitions.Add(responseRequired);

            responseRequired.Transitions.Add(communicationDispatch);
            responseRequired.Transitions.Add(awaitingResponse);
            responseRequired.Transitions.Add(paymentGeneration);

            awaitingResponse.Transitions.Add(redressReview);
            awaitingResponse.Transitions.Add(paymentGeneration);

            paymentGeneration.Transitions.Add(communicationDispatch);
            paymentGeneration.Transitions.Add(finalRedressReview);

            finalRedressReview.Transitions.Add(redressReview);

            return flow;
        }
    }
}
