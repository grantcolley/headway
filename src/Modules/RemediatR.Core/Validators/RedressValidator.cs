using FluentValidation;
using FluentValidation.Results;
using Headway.Core.Constants;
using RemediatR.Core.Constants;
using RemediatR.Core.Model;

namespace RemediatR.Core.Validators
{
    public class RedressValidator : AbstractValidator<Redress>
    {
        public RedressValidator()
        {
            When((redress, context) =>
            {
                return context.RootContextData[FlowConstants.FLOW_STATE_CODE].Equals(RemediatRFlowCodes.REDRESS_CREATE_CODE);
            }, () =>
            {
                RuleFor(r => r.Program).NotNull().WithMessage("Program is required");
                RuleFor(r => r.Product).NotNull().WithMessage("Product is required");
                RuleFor(v => v.RefundCalculation).NotNull().WithMessage("Refund calculation is required");
            });

            When((redress, context) =>
            {
                return context.RootContextData[FlowConstants.FLOW_STATE_CODE].Equals(RemediatRFlowCodes.REFUND_CALCULATION_CODE);
            }, () =>
            {
                RuleFor(r => r.RefundCalculation).SetValidator(new RefundCalculationValidator(), new[] { RemediatRFlowCodes.REFUND_CALCULATION_CODE } );
            });

            When((redress, context) =>
            {
                return context.RootContextData[FlowConstants.FLOW_STATE_CODE].Equals(RemediatRFlowCodes.REFUND_VERIFICATION_CODE);
            }, () =>
            {
                RuleFor(r => r.RefundCalculation).SetValidator(new RefundCalculationValidator(), new[] { RemediatRFlowCodes.REFUND_VERIFICATION_CODE });
            });
        }

        protected override bool PreValidate(ValidationContext<Redress> context, ValidationResult result)
        {
            var stateCode = context.InstanceToValidate?.RedressFlowContext?.Flow?.ActiveState?.StateCode;

            if(string.IsNullOrWhiteSpace(stateCode))
            {
                result.Errors.Add(new ValidationFailure(typeof(RedressFlowContext).Name, $"No active {FlowConstants.FLOW_STATE_CODE}. The flow must be initialised."));
                return false;
            }

            context.RootContextData[FlowConstants.FLOW_STATE_CODE] = stateCode;

            return base.PreValidate(context, result);
        }
    }
}
