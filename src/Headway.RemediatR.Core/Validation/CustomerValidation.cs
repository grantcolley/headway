using FluentValidation;
using Headway.RemediatR.Core.Model;

namespace Headway.RemediatR.Core.Validation
{
    public class CustomerValidation : AbstractValidator<Customer>
    {
        public CustomerValidation()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required")
                .Length(1, 5).WithMessage("Title cannot exceed 5 characters");

            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("Firstname is required")
                .Length(1, 50).WithMessage("Firstname cannot exceed 50 characters");

            RuleFor(v => v.Surname)
                .NotEmpty().WithMessage("Surname is required")
                .Length(1, 50).WithMessage("Surname cannot exceed 50 characters");

            RuleFor(v => v.Telephone)
                .Length(1, 50).WithMessage("Telephone cannot exceed 50 characters");

            RuleFor(v => v.Email)
                .Length(1, 50).WithMessage("Email cannot exceed 50 characters");

            RuleFor(v => v.SortCode)
                .Length(6).WithMessage("Sort Code must be 6 digits e.g. 123456");

            RuleFor(v => v.AccountNumber)
                .Length(8).WithMessage("Account number must be 8 digits e.g. 12345678");

            RuleFor(v => v.Address1)
                .Length(1, 50).WithMessage("Address1 cannot exceed 50 characters");

            RuleFor(v => v.Address2)
                .Length(1, 50).WithMessage("Address2 cannot exceed 50 characters");

            RuleFor(v => v.Address3)
                .Length(1, 50).WithMessage("Address3 cannot exceed 50 characters");

            RuleFor(v => v.Address4)
                .Length(1, 50).WithMessage("Address4 cannot exceed 50 characters");

            RuleFor(v => v.Address5)
                .Length(1, 50).WithMessage("Address5 cannot exceed 50 characters");

            RuleFor(v => v.Country)
                .Length(1, 50).WithMessage("Country cannot exceed 50 characters");

            RuleFor(v => v.PostCode)
                .Length(1, 10).WithMessage("Post Code cannot exceed 10 characters");

            RuleForEach(v => v.Products).SetValidator(new ProductValidator());
        }
    }
}
