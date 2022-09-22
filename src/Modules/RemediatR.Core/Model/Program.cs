using FluentValidation;
using Headway.Core.Attributes;
using Headway.Core.Model;
using RemediatR.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RemediatR.Core.Model
{
    [DynamicModel]
    public class Program : ModelBase
    {
        public int ProgramId { get; set; }
        public ProductType ProductType { get; set; }
        public RateType RateType { get; set; }
        public RepaymentType RepaymentType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(150)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Compensation { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal? CompensatoryInterest { get; set; }
    }

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
