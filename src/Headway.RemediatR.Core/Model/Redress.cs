using FluentValidation;
using Headway.Core.Attributes;
using Headway.Core.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Headway.RemediatR.Core.Model
{
    [DynamicModel]
    public class Redress : ModelBase
    {
        public int RedressId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ProgramId { get; set; }
        public Program Program { get; set; }
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
        [JsonIgnore]
        public int? CustomerId
        {
            get
            {
                return Product.Customer?.CustomerId;
            }
        }

        [NotMapped]
        [JsonIgnore]
        public string? CustomerName
        {
            get
            {
                return $"{Product.Customer?.Fullname}";
            }
        }

        [NotMapped]
        [JsonIgnore]
        public Customer? Customer
        {
            get
            {
                return Product.Customer;
            }
        }

        [NotMapped]
        [JsonIgnore]
        public string? ProgramName         
        {
            get { return Program.Name; } 
        }

        [NotMapped]
        [JsonIgnore]
        public string? ProductName 
        {
            get { return Product.Name; } 
        }
    }

    public class RedressValidator : AbstractValidator<Redress>
    {
        public RedressValidator()
        {
            RuleFor(v => v.Program)
                .NotNull().WithMessage("Program is required");

            RuleFor(v => v.Product)
                .NotNull().WithMessage("Product is required");
        }
    }
}