using System;
using System.Collections.Generic;

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
        public DateTime? CreatedDate { get; set; }
        public string? RedressCaseOwner { get; set; }
        public string? RefundReview { get; set; }
        public string? RefundReviewBy { get; set; }
        public DateTime? RefundReviewDate { get; set; }
        public string? RefundReviewComment { get; set; }
        public string? RedressReview { get; set; }
        public string? RedressReviewBy { get; set; }
        public DateTime? RedressReviewDate { get; set; }
        public string? RedressReviewComment { get; set; }
        public string? RedressValidation { get; set; }
        public string? RedressValidationComment { get; set; }
        public DateTime? CommunicationGenerationDate { get; set; }
        public DateTime? CommunicationDispatchDate { get; set; }
        public bool? ResponseRequired { get; set; }
        public bool? ResponseRecieved { get; set; }
        public string? AwaitingResponse { get; set; }
        public string? AwaitingResponseComment { get; set; }
        public DateTime? PaymentGenerationDate { get; set; }
        public string? FinalRedressReview { get; set; }
        public string? FinalRedressReviewBy { get; set; }
        public DateTime? FinalRedressReviewDate { get; set; }
        public string? FinalRedressReviewComment { get; set; }
    }
}