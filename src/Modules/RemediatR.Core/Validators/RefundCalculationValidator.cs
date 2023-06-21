using FluentValidation;
using RemediatR.Core.Constants;
using RemediatR.Core.Model;

namespace RemediatR.Core.Validators
{
    public class RefundCalculationValidator : AbstractValidator<RefundCalculation>
    {
        public RefundCalculationValidator() 
        {
            RuleSet(RemediatRFlowCodes.REFUND_CALCULATION_CODE, () =>
            {
                RuleFor(rc => rc.BasicRefundAmount).NotNull().WithMessage("Basic Refund Amount is required");
                RuleFor(rc => rc.CompensatoryAmount).NotNull().WithMessage("Compensatory Amount is required");
                RuleFor(rc => rc.CompensatoryInterestAmount).NotNull().WithMessage("Compensatory Interest Amount is required");
                RuleFor(rc => rc.TotalCompensatoryAmount).NotNull().WithMessage("Total Compensatory Amount is required");
                RuleFor(rc => rc.TotalRefundAmount).NotNull().WithMessage("Total Refund Amount is required");
            });

            RuleSet(RemediatRFlowCodes.REFUND_VERIFICATION_CODE, () =>
            {
                RuleFor(rc => rc.VerifiedBasicRefundAmount).NotNull().WithMessage("Verified Basic Refund Amount is required");
                RuleFor(rc => rc.VerifiedCompensatoryAmount).NotNull().WithMessage("Verified Compensatory Amount is required");
                RuleFor(rc => rc.VerifiedCompensatoryInterestAmount).NotNull().WithMessage("Verified Compensatory Interest Amount is required");
                RuleFor(rc => rc.VerifiedTotalCompensatoryAmount).NotNull().WithMessage("Verified Total Compensatory Amount is required");
                RuleFor(rc => rc.VerifiedTotalRefundAmount).NotNull().WithMessage("Verified Total Refund Amount is required");
            });
        }
    }
}
