using FluentValidation;
using Headway.RemediatR.Core.Model;

namespace Headway.RemediatR.Core.Validation
{
    public class RedressValidator : AbstractValidator<Redress>
    {
        public RedressValidator()
        {
            RuleFor(v => v.Customer)
                .NotNull().WithMessage("Customer is required");

            RuleFor(v => v.Program)
                .NotNull().WithMessage("Program is required");

            RuleFor(v => v.Product)
                .NotNull().WithMessage("Product is required");

            //RuleFor(v => v.RefundCalculation).SetValidator(new RefundCalculationValidator());
            //RuleFor(v => v.RefundVerification).SetValidator(new RefundCalculationValidator());
        }
    }
}
