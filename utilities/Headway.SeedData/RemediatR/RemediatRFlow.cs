using Headway.Core.Model;
using RemediatR.Core.Constants;
using RemediatR.Core.Model;

namespace Headway.SeedData.RemediatR
{
    public class RemediatRFlow
    {
        public static Flow FlowCreate()
        {
            var redressCreate = new State { Name = "Redress Create", StateCode = "REDRESS_CREATE", Position = 1, Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var refundAssessment = new State { Name = "Refund Assessment", StateCode = "REFUND_ASSESSMENT", Position = 10, Permissions = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundCalculation = new State { Name = "Refund Calculation", StateCode = "REFUND_CALCULATION", Position = 11, Permissions = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundVerification = new State { Name = "Refund Verification", StateCode = "REFUND_VERIFICATION", Position = 12, Permissions = RemediatRAuthorisation.REFUND_ASSESSOR };
            var refundReview = new State { Name = "Refund Review", StateCode = "REFUND_REVIEW", Position = 20, Permissions = RemediatRAuthorisation.REFUND_REVIEWER };
            var redressReview = new State { Name = "Redress Review", StateCode = "REDRESS_REVIEW", Position = 30, Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };
            var redressValidation = new State { Name = "Redress Validation", StateCode = "REDRESS_VALIDATE", Position = 40, Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };
            var communicationGeneration = new State { Name = "Communication Generation", StateCode = "COMMUNICATION_GENERATION", Position = 50, Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };
            var communicationDispatch = new State { Name = "Communication Dispatch", StateCode = "COMMUNICATION_DISPATCH", Position = 60, Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var responseRequired = new State { Name = "Response Required", StateCode = "RESPONSE_REQUIRED", Position = 70, Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var awaitingResponse = new State { Name = "Awaiting Response", StateCode = "AWAITING_RESPONSE", Position = 80, Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var paymentGeneration = new State { Name = "Payment Generation", StateCode = "PAYMENT_GENERATION", Position = 90, Permissions = RemediatRAuthorisation.REDRESS_CASE_OWNER };
            var finalRedressReview = new State { Name = "Final Redress Review", StateCode = "FINAL_REDRESS_REVIEW", Position = 100, Permissions = RemediatRAuthorisation.REDRESS_REVIEWER };

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
                state.Context = flow.Context;
            }

            return flow;
        }
    }
}
