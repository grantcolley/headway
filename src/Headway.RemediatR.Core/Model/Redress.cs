using FluentValidation;
using Headway.Core.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headway.RemediatR.Core.Model
{
    [DynamicModel]
    public class Redress
    {
        public int RedressId { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int ProgramId { get; set; }
        public Program? Program { get; set; }
        public RefundCalculation? RefundCalculation { get; set; }
        public RefundCalculation? RefundVerification { get; set; }

        [MaxLength(50)]
        public string? RedressCaseOwner { get; set; }

        [MaxLength(50)]
        public string? RedressCreateBy { get; set; }

        public DateTime? RedressCreateDate { get; set; }

        [MaxLength(50)]
        public string? RefundReviewStatus { get; set; }

        [StringLength(250)]
        public string? RefundReviewComment { get; set; }

        [MaxLength(50)]
        public string? RefundReviewBy { get; set; }

        public DateTime? RefundReviewDate { get; set; }

        [MaxLength(50)]
        public string? RedressReviewStatus { get; set; }

        [StringLength(250)]
        public string? RedressReviewComment { get; set; }

        [MaxLength(50)]
        public string? RedressReviewBy { get; set; }

        public DateTime? RedressReviewDate { get; set; }

        [MaxLength(50)]
        public string? RedressValidationStatus { get; set; }

        [StringLength(250)]
        public string? RedressValidationComment { get; set; }

        [MaxLength(50)]
        public string? RedressValidationBy { get; set; }

        public DateTime? RedressValidationDate { get; set; }

        [MaxLength(50)]
        public string? CommunicationGenerationStatus { get; set; }

        [MaxLength(50)]
        public string? CommunicationGenerationBy { get; set; }

        public DateTime? CommunicationGenerationDate { get; set; }

        [MaxLength(50)]
        public string? CommunicationDispatchStatus { get; set; }

        [StringLength(250)]
        public string? CommunicationDispatchComment { get; set; }

        [MaxLength(50)]
        public string? CommunicationDispatchBy { get; set; }

        public DateTime? CommunicationDispatchDate { get; set; }

        public bool? ResponseRequired { get; set; }

        public bool? ResponseReceived { get; set; }

        [MaxLength(50)]
        public string? AwaitingResponseStatus { get; set; }

        [StringLength(250)]
        public string? AwaitingResponseComment { get; set; }

        [MaxLength(50)]
        public string? AwaitingResponseBy { get; set; }

        public DateTime? AwaitingResponseDate { get; set; }

        [MaxLength(50)]
        public string? PaymentGenerationStatus { get; set; }

        [MaxLength(50)]
        public string? PaymentGenerationBy { get; set; }

        public DateTime? PaymentGenerationDate { get; set; }

        [MaxLength(50)]
        public string? FinalRedressReviewStatus { get; set; }

        [StringLength(250)]
        public string? FinalRedressReviewComment { get; set; }

        [MaxLength(50)]
        public string? FinalRedressReviewBy { get; set; }

        public DateTime? FinalRedressReviewDate { get; set; }

        [NotMapped]
        public string? CustomerName 
        {
            get
            {
                return Customer != null ? $"{Customer.Title} {Customer.FirstName} {Customer.Surname}" : string.Empty; 
            }
        }

        [NotMapped]
        public string? ProgramName         
        {
            get { return Program != null ? Program.Name : string.Empty; } 
        }

        [NotMapped]
        public string? ProductName 
        {
            get { return Product != null ? Product.Name : string.Empty; } 
        }
    }

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