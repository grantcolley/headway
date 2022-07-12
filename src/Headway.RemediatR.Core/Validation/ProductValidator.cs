using FluentValidation;
using Headway.RemediatR.Core.Model;

namespace Headway.RemediatR.Core.Validation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required")
                .Length(1, 50).WithMessage("Name cannot exceed 50 characters");

            RuleFor(v => v.StartDate)
                .NotNull().WithMessage("Start Date is required");
        }
    }
}
