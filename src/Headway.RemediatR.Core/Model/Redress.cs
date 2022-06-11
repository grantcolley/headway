using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.RemediatR.Core.Model
{
    public class Redress
    {
        public Redress()
        {
            RedressProducts = new List<RedressProduct>();
        }

        public int RedressId { get; set; }
        public Customer? Customer { get; set; }
        public Program? Program { get; set; }
        public List<RedressProduct>? RedressProducts { get; set; }
        public RefundCalculation? RefundCalculation { get; set; }
        public RefundCalculation? RefundVerification { get; set; }

        [MaxLength(50)]
        public string? RedressCaseOwner { get; set; }

        [MaxLength(50)]
        public string? RedressLoadBy { get; set; }
        public DateTime? RedressLoadDate { get; set; }

        [MaxLength(50)]
        public string? RefundReviewStatus { get; set; }

        [StringLength(250, ErrorMessage = "Refund Review Comment cannot exceed 250 characters")]
        public string? RefundReviewComment { get; set; }

        [MaxLength(50)]
        public string? RefundReviewBy { get; set; }
        public DateTime? RefundReviewDate { get; set; }

        [MaxLength(50)]
        public string? RedressReviewStatus { get; set; }

        [StringLength(250, ErrorMessage = "Redress Review Comment cannot exceed 250 characters")]
        public string? RedressReviewComment { get; set; }

        [MaxLength(50)]
        public string? RedressReviewBy { get; set; }
        public DateTime? RedressReviewDate { get; set; }

        [MaxLength(50)]
        public string? RedressValidationStatus { get; set; }

        [StringLength(250, ErrorMessage = "Redress Validation Comment cannot exceed 250 characters")]
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

        [StringLength(250, ErrorMessage = "Communication Dispatch Comment cannot exceed 250 characters")]
        public string? CommunicationDispatchComment { get; set; }

        [MaxLength(50)]
        public string? CommunicationDispatchBy { get; set; }
        public DateTime? CommunicationDispatchDate { get; set; }

        public bool? ResponseRequired { get; set; }
        public bool? ResponseRecieved { get; set; }

        [MaxLength(50)]
        public string? AwaitingResponseStatus { get; set; }

        [StringLength(250, ErrorMessage = "Awaiting Response Comment cannot exceed 250 characters")]
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

        [StringLength(250, ErrorMessage = "Final Redress Review Comment cannot exceed 250 characters")]
        public string? FinalRedressReviewComment { get; set; }

        [MaxLength(50)]
        public string? FinalRedressReviewBy { get; set; }
        public DateTime? FinalRedressReviewDate { get; set; }
    }
}