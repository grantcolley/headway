using System;
using System.Collections.Generic;

namespace Headway.RemediatR.Core.Model
{
    public class Redress
    {
        public Redress()
        {
            Products = new List<Product>();
        }

        public int RedressId { get; set; }
        public Customer? Customer { get; set; }
        public Program? Program { get; set; }
        public List<Product> Products { get; set; }
        public RefundCalculation? RefundCalculation { get; set; }
        public RefundCalculation? RefundVerification { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? RedressCaseOwner { get; set; }
        public string? RefundReviewedBy { get; set; }
        public DateTime? RefundReviewDate { get; set; }
        public string? RefundReviewComment { get; set; }
        public string? RedressReviewedBy { get; set; }
        public DateTime? RedressReviewDate { get; set; }
        public string? RedressReviewComment { get; set; }
        public DateTime? CommunicationDispatchDate { get; set; }
        public bool ResponseRequired { get; set; }
        public bool ResponseRecieved { get; set; }
        public string? ResponseComment { get; set; }
    }
}