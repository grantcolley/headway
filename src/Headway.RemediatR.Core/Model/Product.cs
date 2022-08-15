using FluentValidation;
using Headway.Core.Attributes;
using Headway.RemediatR.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headway.RemediatR.Core.Model
{
    [DynamicModel]
    public class Product
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public ProductType ProductType { get; set; }
        public RateType RateType { get; set; }
        public RepaymentType RepaymentType { get; set; }
        public Redress? Redress { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Range(3, 360)]
        public int? Duration { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal? Rate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Value { get; set; }
    }

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
