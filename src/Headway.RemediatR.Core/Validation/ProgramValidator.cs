using FluentValidation;
using Headway.RemediatR.Core.Model;

namespace Headway.RemediatR.Core.Validation
{
    public class ProgramValidator : AbstractValidator<Program>
    {
        public ProgramValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(1, 50).WithMessage("Name cannot exceed 50 characters");

            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Description is required")
                .Length(1, 150).WithMessage("Description cannot exceed 150 characters");
        }
    }
}
