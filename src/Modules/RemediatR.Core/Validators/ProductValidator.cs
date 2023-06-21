﻿using FluentValidation;
using RemediatR.Core.Model;

namespace RemediatR.Core.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(v => v.Name)
                .NotNull().WithMessage("Product name is required")
                .Length(1, 50).WithMessage("Name cannot exceed 50 characters");

            RuleFor(v => v.StartDate)
                .NotNull().WithMessage("Product start date is required");
        }
    }
}
